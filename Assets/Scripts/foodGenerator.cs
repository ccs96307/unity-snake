using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodGenerator : MonoBehaviour
{
    public GameObject foodObject;
    public Sprite[] spriteArray;
    public float bound = 197f;

    // Start is called before the first frame update
    void Start()
    {
        spriteArray = Resources.LoadAll<Sprite>("Bodies/");
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        if (transform.childCount < 2000)
        {
            foodObject.GetComponent<SpriteRenderer>().sprite = spriteArray[Random.Range(0, spriteArray.Length-1)];
            for (int i=0; i<10; ++i)
            {
                Instantiate(
                    foodObject,
                    new Vector3(Random.Range(-bound, bound), Random.Range(-bound, bound), 0f),
                    transform.rotation,
                    transform
                );
            }

        }
    }
}
