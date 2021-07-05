using UnityEngine;
using UnityEngine.UI;


public class SettingDoll : SnakeBase
{
    // Player Data
    GameData currentPlayerData = new GameData();

    // Player Info
    int cameraSize;
    int cameraChangeStep;

    // Init
    void Start()
    {
        // Load Save File
        SaveManager.LoadGame();
        if (SaveManager.dataList.Count != 0) currentPlayerData = SaveManager.dataList[0];

        // Score
        if (currentPlayerData.HighestScore != 0) this._score = currentPlayerData.HighestScore;
        else this._score = 0;

        print("Highest Score: " + currentPlayerData.HighestScore.ToString());

        // Sprites
        this._spriteIndex = 2;
        this._index2snakeSprite = this._InitializeIndex2Sprite();
        string colorName = this._index2snakeSprite[this._spriteIndex];

        this._headSprite = Resources.Load<Sprite>(this._colorName2HeadSprite(colorName));
        this._bodyGameObject = Resources.Load<GameObject>(this._colorName2BodySprite(colorName));

        this._smallFoodObject = Resources.Load<GameObject>(this._colorName2SmallFoodObject(colorName));
        this._bigFoodObject = Resources.Load<GameObject>(this._colorName2BigFoodObject(colorName));

        gameObject.GetComponent<SpriteRenderer>().sprite = this._headSprite;

        // Body
        this._bodyNum = 4;
        this._basicDistanceWithEachOther = 8;
        this._distanceWithEachOtherStep = 200;
        this._bodyGrowSize = 0.05f;
        this._bodyGrowStep = 50;
        this._positionRecordEnable = true;

        // Movement
        this._oldDirection = new Vector3(0, -1, 0);

        // Camera (Player Only)
        cameraSize = 9;
        cameraChangeStep = 500;

        // Name (Default)
        this._username = "Clay";
        gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>().text = this._username;
    }

    void FixedUpdate()
    {
        // Camera (Player Only)
        Camera.main.orthographicSize = cameraSize + this._score / cameraChangeStep;
        Camera.main.gameObject.transform.position = transform.position;

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
}

