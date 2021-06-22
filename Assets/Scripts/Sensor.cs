using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int collisionStatus;
    public GameObject colliderObject;

    // Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Sensor is hit!!");

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "wall" || collision.gameObject.tag == "Snake")
        {
            collisionStatus = 1;
            colliderObject = collision.gameObject;
        }

        else if (collision.gameObject.tag == "10_food")
        {
            collisionStatus = 2;
            colliderObject = collision.gameObject;
        }
        else if (collision.gameObject.tag == "1_food")
        {
            collisionStatus = 3;
            colliderObject = collision.gameObject;
        }
    }

    private void FixedUpdate()
    {
        //collisionStatus = 0;
    }
}
