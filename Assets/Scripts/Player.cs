using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    // Speed
    public static float moveSpeed = 0.08f;
    
    // Joystick input
    public VJHandler jsMovement;
    private Vector3 direction;
    private Vector3 newDirection;
    private Vector3 oldDirection = new Vector3(-0.01f, 0.0f, 0.0f);

    // Player (snake head) position and rotation
    public List<Vector3> positionList;
    private float playerRotation = 0f;

    // Player Info
    public int score = 0;
    public int bodyNum = 4;

    // Body Info
    public GameObject body;
    public List<GameObject> bodyList;
    public List<int> bodyPositionIndex;
    public float bodySize;
    public int distanceWithEachOther = 7;
    private bool recordEnable = true;


    void Start()
    {

    }


    void Update() {
        // Scale
        transform.localScale = new Vector3(
            3.0f + (0.1f * score / 500),
            3.0f + (0.1f * score / 500),
            0f
        );

        for (int i=0; i<bodyList.Count; ++i)
        {
            bodyList[i].transform.localScale = new Vector3(
                3.0f + (0.1f * score / 500),
                3.0f + (0.1f * score / 500),
                0f
            );
        }

        distanceWithEachOther = 7 + score / 2000;

        // Text
        GameObject.Find("Canvas/ScoreText").GetComponent<Text>().text = score.ToString();
        
        bodyNum = ScoreChangeBodyNum(score);
        newDirection = jsMovement.InputDirection; //InputDirection can be used as per the need of your project

        // Direction
        if (newDirection.magnitude == 0)
        {
            direction = oldDirection;
        }
        else
        {
            // Rotation
            playerRotation -= Vector2.SignedAngle(newDirection, oldDirection);

            // Direction
            direction = newDirection;
            oldDirection = newDirection;
        }

        direction = Normalization(direction);
        transform.position += direction * moveSpeed;
        transform.localEulerAngles = new Vector3(0, 0, playerRotation);


        // Position List
        positionList.Add(transform.position);

        if (bodyPositionIndex.Count < bodyNum && positionList.Count > (bodyPositionIndex.Count+1)*7)
        {
            bodyPositionIndex.Add(0);
        }
        else if (bodyPositionIndex.Count == bodyNum)
        {
            if (!recordEnable)
            {
                positionList.RemoveAt(0);
            }
        }
        else if (bodyPositionIndex.Count > bodyNum)
        {
            bodyPositionIndex.RemoveAt(bodyPositionIndex.Count - 1);
            Destroy(bodyList[0]);
            bodyList.RemoveAt(0);
        }


        // Move body balls to their position
        for (int i=0; i < bodyPositionIndex.Count; ++i)
        {
            int positionIndex = positionList.Count - ((i + 1) * distanceWithEachOther);
            if (positionIndex < 0)
            {
                recordEnable = true;
                break;
            }


            if (bodyList.Count <= i)
            {
                GameObject newBody = Instantiate(
                    body,
                    transform.position,
                    gameObject.transform.parent.gameObject.transform.rotation,
                    gameObject.transform.parent.gameObject.transform
                );

                newBody.transform.SetSiblingIndex(transform.childCount - 1);
                bodyList.Insert(0, newBody);
            }

            bodyList[i].gameObject.transform.position = positionList[positionIndex];
            recordEnable = false;
        }
    }


    // Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake" || collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "10_food")
        {
            score += 100;
        }
    }



    Vector3 Normalization(Vector3 direction)
    {
        double normal = direction.x * direction.x + direction.y * direction.y;
        normal = System.Math.Sqrt(normal);

        direction.x *= (1 / (float)normal);
        direction.y *= (1 / (float)normal);
        
        return direction;
    }

    int ScoreChangeBodyNum(int score)
    {
        int moreBody = score / 100 + 4;
        return moreBody;
    }

    int ScoreAdjustSize(int score)
    {
        int size = score / 100;
        return size;
    }

}