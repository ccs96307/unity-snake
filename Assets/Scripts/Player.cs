using UnityEngine;
using UnityEngine.UI;


public class Player : SnakeBase {
    // Joystick input
    public VJHandler jsMovement;

    // SpeedUp Button
    public SpeedUpButtonEvent SpeedUpButton;

    // Player Info
    int cameraSize;
    int cameraChangeStep;

    // Init
    void Start()
    {
        // Score
        this._score = 0;

        // Sprites
        this._spriteIndex = 2;
        this._index2snakeSprite = this._InitializeIndex2Sprite();
        string colorName = this._index2snakeSprite[this._spriteIndex];

        this._headSprite = Resources.Load<Sprite>(this._colorName2HeadSprite(colorName));
        this._bodySprite = Resources.Load<Sprite>(this._colorName2BodySprite(colorName));

        this._bodyGameObject.GetComponent<SpriteRenderer>().sprite = this._bodySprite;
        this._bigFoodObject.GetComponent<SpriteRenderer>().sprite = this._bodySprite;
        gameObject.GetComponent<SpriteRenderer>().sprite = this._headSprite;

        // Speed
        this._basicMoveSpeed = 0.08f;

        // Body
        this._bodyNum = 4;
        this._basicDistanceWithEachOther = 7;
        this._distanceWithEachOtherStep = 200;
        this._bodyGrowSize = 0.1f;
        this._bodyGrowStep = 50;
        this._positionRecordEnable = true;

        // Camera (Player Only)
        cameraSize = 7;
        cameraChangeStep = 500;

        // Name (Default)
        this._username = "Clay";
        gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>().text = this._username;
    }

    void FixedUpdate() {
        // Camera (Player Only)
        Camera.main.orthographicSize = cameraSize + this._score / cameraChangeStep;
        Camera.main.gameObject.transform.position = transform.position;

        // Head and Body Scales
        Vector3 scaleSize = new Vector3(
            3.0f + (this._bodyGrowSize * this._score / this._bodyGrowStep),
            3.0f + (this._bodyGrowSize * this._score / this._bodyGrowStep),
            0f
        );

        transform.localScale = scaleSize;
        for (int i = 0; i < this._bodyList.Count; ++i) this._bodyList[i].transform.localScale = scaleSize;

        // If SpeedUp Button is pressed down
        if (SpeedUpButton.pressed == true && this._score > 0)
        {
            // Speed Up
            this._moveSpeed = 2 * this._basicMoveSpeed;

            // Speed Up distances
            this._distanceWithEachOther = this._basicDistanceWithEachOther-2 + (this._score / 200);

            // When drop frames bigger than 25 frames, generate a 10-socres-food
            ++this._dropFoodFrame;
            if (this._dropFoodFrame >= 25)
            {
                this._smallFoodObject.GetComponent<SpriteRenderer>().sprite = this._bodySprite;
                Instantiate(
                    this._smallFoodObject,
                    this._bodyList[this._bodyList.Count-1].transform.position,
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

        // Change the score text
        GameObject.Find("Canvas/ScoreText").GetComponent<Text>().text = this._score.ToString();

        // Change the Body number according scores
        this._bodyNum = this._ScoreChangeBodyNum(this._score);

        // Get the Joystick input
        this._newDirection = jsMovement.InputDirection;

        // Direction
        if (this._newDirection.magnitude == 0)
        {
            this._direction = this._oldDirection;
        }
        else
        {
            // Rotation
            this._playerRotation -= Vector2.SignedAngle(this._newDirection, this._oldDirection);

            // Direction
            this._direction = this._newDirection;
            this._oldDirection = this._newDirection;
        }

        // Move the object
        this._direction = this._Normalization(this._direction);
        transform.position += this._direction * this._moveSpeed;
        transform.localEulerAngles = new Vector3(0, 0, this._playerRotation);


        // Position List
        this._positionList.Add(transform.position);

        if (this._bodyPositionIndex.Count < this._bodyNum && this._positionList.Count > (this._bodyPositionIndex.Count+1)* this._distanceWithEachOther)
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
        for (int i=0; i < this._bodyPositionIndex.Count; ++i)
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
            for (int i=0; i<this._bodyList.Count; ++i)
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
            this._score += 10;
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
        this._bigFoodObject.GetComponent<SpriteRenderer>().sprite = this._bodySprite;
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

