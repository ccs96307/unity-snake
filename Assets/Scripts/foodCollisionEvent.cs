using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodCollisionEvent : MonoBehaviour
{
    // Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake" || collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
