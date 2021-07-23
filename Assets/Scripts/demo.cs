using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo : MonoBehaviour
{
    // ShopManager
    public ShopManager shopManager;

    // Demo
    public List<GameObject> bodyList = new List<GameObject>();
    public List<float> offsetList = new List<float>();
    public int distance;
    public float step;
    public float bound;

    public Dictionary<int, string> headSprites = new Dictionary<int, string>
    {
        { 0, "Heads/hide_snake_head" },
        { 1, "Heads/blue_snake_head" },
        { 2, "Heads/cyan_snake_head" },
        { 3, "Heads/green_snake_head" },
        { 4, "Heads/grey_snake_head" },
        { 5, "Heads/iron_snake_head" },
        { 6, "Heads/orange_snake_head" },
        { 7, "Heads/pink_snake_head" },
        { 8, "Heads/purple_snake_head" },
        { 9, "Heads/red_snake_head" },
        { 10, "Heads/white_snake_head" },
        { 11, "Heads/yellow_snake_head" }
    };
    public Dictionary<int, string> bodySprites = new Dictionary<int, string>
    {
        { 0, "Bodies/hide_body" },
        { 1, "Bodies/blue_body" },
        { 2, "Bodies/cyan_body" },
        { 3, "Bodies/green_body" },
        { 4, "Bodies/grey_body" },
        { 5, "Bodies/iron_body" },
        { 6, "Bodies/orange_body" },
        { 7, "Bodies/pink_body" },
        { 8, "Bodies/purple_body" },
        { 9, "Bodies/red_body" },
        { 10, "Bodies/white_body" },
        { 11, "Bodies/yellow_body" }
    };

    // Color
    public int selectedSkinIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Init
        step = 0.03f;
        distance = 20;
        bound = 0.8f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move
        offsetList.Add(transform.position.y);

        if ((step < 0 && transform.position.y <= -bound) ||
            (step > 0 && transform.position.y >= bound))
        {
            step = -step;
        }

        transform.position += new Vector3(0, step, 0);

        if (offsetList.Count > distance*6)
        {
            offsetList.RemoveAt(0);
        }

        for (int i=0; i<bodyList.Count; ++i)
        {
            if (offsetList.Count - 1 - (distance * (i+1)) < 0) break;
            bodyList[i].transform.position = new Vector3(
                bodyList[i].transform.position.x,
                offsetList[offsetList.Count-1-(distance*(i+1))],
                0
            );
        }

        // Change skin
        if (shopManager.needToChangeColor == true)
        {
            shopManager.needToChangeColor = false;
            changeSkin();
        }
    }

    void changeSkin()
    {
        // Index
        int index = shopManager.selectedSkinIndex;

        // Change Sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(headSprites[index]);
        Sprite bodySprite = Resources.Load<GameObject>(bodySprites[index]).GetComponent<SpriteRenderer>().sprite;

        for (int i = 0; i < bodyList.Count; ++i)
        {
            bodyList[i].gameObject.GetComponent<SpriteRenderer>().sprite = bodySprite;
        }
    }
}
