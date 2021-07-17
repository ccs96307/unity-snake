using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo : MonoBehaviour
{
    // Player Data
    GameData currentPlayerData = new GameData();

    // Demo
    public List<GameObject> bodyList = new List<GameObject>();
    public List<float> offsetList = new List<float>();
    public int distance;
    public float step;
    public float bound;

    public Dictionary<int, string> headSprites = new Dictionary<int, string>();
    public Dictionary<int, string> bodySprites = new Dictionary<int, string>();

    // Button
    public Button previousButton;
    public Button nextButton;
    public Button selectedButton;

    // Color
    public int selectedSkinIndex;

    // Coin
    public Text coinNumText;

    // Start is called before the first frame update
    void Start()
    {
        // Load data
        SaveManager.LoadGame();
        currentPlayerData = SaveManager.dataList[0];

        // Init
        step = 0.03f;
        distance = 20;
        bound = 0.8f;

        // Index to Sprite
        // Head
        headSprites.Add(0, "Heads/hide_snake_head");
        headSprites.Add(1, "Heads/blue_snake_head");
        headSprites.Add(2, "Heads/cyan_snake_head");
        headSprites.Add(3, "Heads/green_snake_head");
        headSprites.Add(4, "Heads/grey_snake_head");
        headSprites.Add(5, "Heads/iron_snake_head");
        headSprites.Add(6, "Heads/orange_snake_head");
        headSprites.Add(7, "Heads/pink_snake_head");
        headSprites.Add(8, "Heads/purple_snake_head");
        headSprites.Add(9, "Heads/red_snake_head");
        headSprites.Add(10, "Heads/white_snake_head");
        headSprites.Add(11, "Heads/yellow_snake_head");

        // Body gameobject
        bodySprites.Add(0, "Bodies/hide_body");
        bodySprites.Add(1, "Bodies/blue_body");
        bodySprites.Add(2, "Bodies/cyan_body");
        bodySprites.Add(3, "Bodies/green_body");
        bodySprites.Add(4, "Bodies/grey_body");
        bodySprites.Add(5, "Bodies/iron_body");
        bodySprites.Add(6, "Bodies/orange_body");
        bodySprites.Add(7, "Bodies/pink_body");
        bodySprites.Add(8, "Bodies/purple_body");
        bodySprites.Add(9, "Bodies/red_body");
        bodySprites.Add(10, "Bodies/white_body");
        bodySprites.Add(11, "Bodies/yellow_body");

        // Button
        previousButton.onClick.AddListener(previousButtonEvent);
        nextButton.onClick.AddListener(nextButtonEvent);
        selectedButton.onClick.AddListener(selectButtonEvent);

        // Current selected skin index
        print(currentPlayerData.CurrentUsedSkin);
        selectedSkinIndex = currentPlayerData.CurrentUsedSkin;
        changeSkin();

        // Coin
        changeCoinText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
    }

    void previousButtonEvent()
    {
        //nextButton.gameObject.SetActive(true);
        nextButton.interactable = true;

        if (selectedSkinIndex > 2)
        {
            --selectedSkinIndex;
        }
        else if (selectedSkinIndex == 2)
        {
            --selectedSkinIndex;
            previousButton.interactable = false; 
            //previousButton.gameObject.SetActive(false);
        }

        changeSkin();
        selectButtonEvent();
    }

    void nextButtonEvent()
    {
        //previousButton.gameObject.SetActive(true);
        previousButton.interactable = true;

        if (selectedSkinIndex < bodySprites.Count-2)
        {
            ++selectedSkinIndex;
        }
        else if (selectedSkinIndex == bodySprites.Count-2)
        {
            ++selectedSkinIndex;
            //nextButton.gameObject.SetActive(false);
            nextButton.interactable = false;
        }

        changeSkin();
    }

    void changeSkin()
    {
        // Change Sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(headSprites[selectedSkinIndex]);
        Sprite bodySprite = Resources.Load<GameObject>(bodySprites[selectedSkinIndex]).GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < bodyList.Count; ++i)
        {
            bodyList[i].gameObject.GetComponent<SpriteRenderer>().sprite = bodySprite;
        }
    }

    void selectButtonEvent()
    {
        currentPlayerData.CurrentUsedSkin = selectedSkinIndex;
        print("Current Used Skin Index: " + currentPlayerData.CurrentUsedSkin.ToString());
    }

    void changeCoinText()
    {
        coinNumText.text = currentPlayerData.Coin.ToString();
    }
}
