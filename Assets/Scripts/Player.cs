using UnityEngine;
using System.Collections;
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
    private float pr = 0f;

    // Player Info
    public int score = 0;
    public int bodyNum = 4;

    // Body Info
    public GameObject body;
    public List<GameObject> bodyList;
    public List<int> bodyPositionIndex;
    public float bodySize;
    public float distanceWithHead = 0.5f;



    public bool flag = true;


    void Start()
    {

    }


    void Update() {
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
            pr -= Vector2.SignedAngle(newDirection, oldDirection);

            // Direction
            direction = newDirection;
            oldDirection = newDirection;
        }

        direction = Normalization(direction);
        transform.position += direction * moveSpeed;
        transform.localEulerAngles = new Vector3(0, 0, pr);


        // Position List
        positionList.Add(transform.position);

        if (bodyPositionIndex.Count < bodyNum && positionList.Count > (bodyPositionIndex.Count+1)*7)
        {
            bodyPositionIndex.Add(0);
        }
        else if (bodyPositionIndex.Count == bodyNum)
        {
            positionList.RemoveAt(0);
        }
        else if (bodyPositionIndex.Count > bodyNum)
        {
            bodyPositionIndex.RemoveAt(bodyPositionIndex.Count - 1);
            Destroy(bodyList[0]);
            bodyList.RemoveAt(0);
        }


        /*// Record body ball position
        if (bodyPositionIndex.Count == 0)
        {
            if ((transform.position - positionList[0]).magnitude >= distanceWithHead)
            {
                bodyPositionIndex.Add(0);
            }
        }
        else if (bodyPositionIndex.Count < bodyNum)
        {
            if ((positionList[bodyPositionIndex[bodyPositionIndex.Count - 1]] - positionList[0]).magnitude >= distanceWithHead)
            {
                bodyPositionIndex.Add(0);
            }
            
            // Update body ball position
            for (int i = 0; i < bodyPositionIndex.Count; ++i)
            {
                ++bodyPositionIndex[i];
            }
        }

        // Don't need to record more trace
        else if (bodyPositionIndex.Count == bodyNum)
        {
            positionList.RemoveAt(0);
        }

        // Remove extra balls
        else if (bodyPositionIndex.Count > bodyNum)
        {
            positionList.RemoveAt(0);
            Destroy(bodyList[0]);
            bodyList.RemoveAt(0);
            bodyPositionIndex.RemoveAt(bodyPositionIndex.Count - 1);
        }*/

        // Move body balls to their position
        for (int i=0; i < bodyPositionIndex.Count; ++i)
        {
            if (bodyList.Count > i)
            {
                //bodyList[i].gameObject.transform.position = positionList[bodyPositionIndex[i]];
                bodyList[i].gameObject.transform.position = positionList[positionList.Count - ((i + 1) * 7)];
            }
            else
            {
                GameObject newBody = Instantiate(
                    body,
                    transform.position,
                    gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.rotation,
                    gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform
                );

                bodyList.Insert(0, newBody);
                //bodyList[i].gameObject.transform.position = positionList[bodyPositionIndex[bodyPositionIndex.Count-i-1]];
                bodyList[i].gameObject.transform.position = positionList[positionList.Count - ((i + 1) * 7)];
            }
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