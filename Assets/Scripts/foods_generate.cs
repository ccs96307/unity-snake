using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foods_generate : MonoBehaviour
{
    public GameObject foodObject;
    public float bound = 197f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount < 10000)
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
