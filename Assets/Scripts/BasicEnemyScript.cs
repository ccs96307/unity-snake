using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : SnakeBase
{
    // Head Sensor
    public Sensor sensor;

    // User Name
    public GameObject usernameObject;

    // Strategy
    public bool isSpeedUp = false;
    private List<Vector3> colliderPosition = new List<Vector3>();
    int currentFrame = 0;
    int LastFrame = 0;
    int collisionFrameClosed = 0;

    // Init
    void Start()
    {
        // Score
        this._score = Random.Range(0, 600);

        // Sprites
        this._spriteIndex = Random.Range(1, 11);
        this._index2snakeSprite = this._InitializeIndex2Sprite();
        string colorName = this._index2snakeSprite[this._spriteIndex];

        this._headSprite = Resources.Load<Sprite>(this._colorName2HeadSprite(colorName));
        this._bodyGameObject = Resources.Load<GameObject>(this._colorName2BodySprite(colorName));

        this._smallFoodObject = Resources.Load<GameObject>(this._colorName2SmallFoodObject(colorName));
        this._bigFoodObject = Resources.Load<GameObject>(this._colorName2BigFoodObject(colorName));

        gameObject.GetComponent<SpriteRenderer>().sprite = this._headSprite;

        // Speed
        this._basicMoveSpeed = 0.08f;

        // Body
        this._bodyNum = 4;
        this._basicDistanceWithEachOther = 7;
        this._distanceWithEachOtherStep = 200;
        this._bodyGrowSize = 0.05f;
        this._bodyGrowStep = 50;
        this._positionRecordEnable = true;

        // Name (Default)
        this._username = "John Wick";
        usernameObject = gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject;
        usernameObject.transform.GetComponent<TextMesh>().text = this._username;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ++currentFrame;
        // Head and Body Scales
        Vector3 scaleSize = new Vector3(
            3.0f + (this._bodyGrowSize * this._score / this._bodyGrowStep),
            3.0f + (this._bodyGrowSize * this._score / this._bodyGrowStep),
            0f
        );

        transform.localScale = scaleSize;
        for (int i = 0; i < this._bodyList.Count; ++i) this._bodyList[i].transform.localScale = scaleSize;

        // If SpeedUp
        if (isSpeedUp == true && this._score > 0)
        {
            // Speed Up
            this._moveSpeed = 2 * this._basicMoveSpeed;

            // Speed Up distances
            this._distanceWithEachOther = this._basicDistanceWithEachOther - 2 + (this._score / 200);

            // When drop frames bigger than 25 frames, generate a 10-socres-food
            ++this._dropFoodFrame;
            if (this._dropFoodFrame >= 25)
            {
                Instantiate(
                    this._smallFoodObject,
                    this._bodyList[this._bodyList.Count - 1].transform.position,
                    gameObject.transform.parent.gameObject.transform.rotation,
                    GameObject.Find("Battle Scene").gameObject.transform
                );
                this._dropFoodFrame = 0;
                --this._score;
            }
        }
        else
        {
            // Normally move speed
            this._moveSpeed = this._basicMoveSpeed;

            // Normally distances of Head and Body
            this._distanceWithEachOther = this._basicDistanceWithEachOther + (this._score / this._distanceWithEachOtherStep);
        }

        // Change the body number according scores
        this._bodyNum = this._ScoreChangeBodyNum(this._score);

        // Decise the direction
        if (sensor.collisionStatus == 1)
        {
            colliderPosition.Add(sensor.colliderObject.transform.position);

            bool safe = false;
            for (int i = 0; i < this._bodyList.Count; ++i)
            {
                if (sensor.colliderObject.GetInstanceID() == this._bodyList[i].GetInstanceID())
                {
                    safe = true;
                    break;
                }
            }

            if (safe) print("safe");

            if (!safe)
            {
                if (currentFrame - LastFrame < 5) ++collisionFrameClosed;
            }

            LastFrame = currentFrame;


            if (!safe && collisionFrameClosed >= 3)
            {
                this._direction = this._positionList[this._positionList.Count - 20] - this._positionList[this._positionList.Count - 1];
                this._direction = Quaternion.Euler(0, Random.Range(-80, 80), 0) * this._direction;
                currentFrame = 0;
                LastFrame = 0;
                collisionFrameClosed = 0;
            }
            else if (!safe)
            {
                Vector3 positionDiff = transform.position - sensor.colliderObject.transform.position;
                positionDiff = Quaternion.Euler(0, Random.Range(-80, 80), 0) * positionDiff;
                this._direction = positionDiff;

                //if (currentFrame - LastFrame < 5)
                //{
                //    ++collisionFrameClosed;
                //    LastFrame = currentFrame;
                //}
            }

            else
            {
                print("撞到自己！");
                print(this._oldDirection);
                if (Random.Range(0, 1000) > 850)
                {
                    this._direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                }
                else
                {
                    this._direction = this._oldDirection;
                    isSpeedUp = false;
                }
                colliderPosition.Clear();
            }
        }
        else if (sensor.collisionStatus == 2)
        {
            this._direction = sensor.colliderObject.transform.position - transform.position;
            isSpeedUp = true;
            colliderPosition.Clear();
        }
        else if (sensor.collisionStatus == 3)
        {
            this._direction = sensor.colliderObject.transform.position - transform.position;
            isSpeedUp = false;
            colliderPosition.Clear();
        }
        else if (sensor.collisionStatus == 0)
        {
            if (Random.Range(0, 1000) > 997)
            {
                this._direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            }
            else
            {
                this._direction = this._oldDirection;
                isSpeedUp = false;
            }
            colliderPosition.Clear();
        }

        // Move the object
        sensor.collisionStatus = 0;

        this._playerRotation -= Vector2.SignedAngle(this._direction, this._oldDirection);

        this._oldDirection = this._direction;
        this._direction = this._Normalization(this._direction);
        transform.position += this._direction * this._moveSpeed;
        usernameObject.transform.position += this._direction * this._moveSpeed;
        transform.localEulerAngles = new Vector3(0, 0, this._playerRotation);


        // Position List
        this._positionList.Add(transform.position);

        if (this._bodyPositionIndex.Count < this._bodyNum && this._positionList.Count > (this._bodyPositionIndex.Count + 1) * this._distanceWithEachOther)
        {
            this._bodyPositionIndex.Add(0);
        }
        else if (this._bodyPositionIndex.Count == this._bodyNum)
        {
            if (!this._positionRecordEnable)
            {
                this._positionList.RemoveAt(0);
            }
        }
        else if (this._bodyPositionIndex.Count > this._bodyNum)
        {
            this._bodyPositionIndex.RemoveAt(this._bodyPositionIndex.Count - 1);
            Destroy(this._bodyList[0]);
            this._bodyList.RemoveAt(0);
        }

        // Move body balls to their position
        for (int i = 0; i < this._bodyPositionIndex.Count; ++i)
        {
            int positionIndex = this._positionList.Count - 1 - ((i + 1) * this._distanceWithEachOther);
            if (positionIndex < 0)
            {
                this._positionRecordEnable = true;
                break;
            }


            if (this._bodyList.Count <= i)
            {
                GameObject newBody = Instantiate(
                    this._bodyGameObject,
                    transform.position,
                    gameObject.transform.parent.gameObject.transform.rotation,
                    gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform
                );

                newBody.gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;
                this._bodyList.Insert(0, newBody);
            }

            this._bodyList[i].gameObject.transform.position = this._positionList[positionIndex];
            this._positionRecordEnable = false;
        }
    }


    // Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "wall")
        {
            deadAndDestroy();
        }
        else if (collision.gameObject.tag == "Snake")
        {
            bool safe = false;
            for (int i = 0; i < this._bodyList.Count; ++i)
            {
                if (collision.gameObject == this._bodyList[i])
                {
                    safe = true;
                    break;
                }
            }

            if (!safe) deadAndDestroy();
        }


        else if (collision.gameObject.tag == "10_food")
        {
            this._score += 5;
        }
        else if (collision.gameObject.tag == "1_food")
        {
            ++this._score;
        }
    }


    // Dead
    void deadAndDestroy()
    {
        // Generate 3 big food on the head
        for (int i = 0; i < 3; ++i)
        {
            Instantiate(
                this._bigFoodObject,
                transform.position + new Vector3(Random.Range(-0.005f, 0.005f), Random.Range(-0.005f, 0.005f), 0f),
                gameObject.transform.parent.gameObject.transform.rotation,
                GameObject.Find("Battle Scene").gameObject.transform
            );
        }

        // Generate the dead-big-foods
        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < this._bodyList.Count; ++j)
            {
                Instantiate(
                    this._bigFoodObject,
                    this._bodyList[j].gameObject.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f),
                    this._bodyList[j].gameObject.transform.parent.gameObject.transform.rotation,
                    GameObject.Find("Battle Scene").gameObject.transform
                );
            }
        }

        // Destroy head and body
        Destroy(gameObject);

        for (int i = 0; i < this._bodyList.Count; ++i)
        {
            Destroy(this._bodyList[i]);
        }
    }
}
