using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public GameObject petObject;
    public List<Vector3> positionList;
    public int distance = 20;
    public float speed = 0.1f;

    void FixedUpdate()
    {
        // Move
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, speed, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -speed, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed, 0, 0);
        }

        // Pet following
        positionList.Add(transform.position);

        if (positionList.Count > distance)
        {
            positionList.RemoveAt(0);
            petObject.transform.position = positionList[0];
        }
    }
}
