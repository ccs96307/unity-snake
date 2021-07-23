using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Player Data
    GameData playerData = new GameData();

    // Button
    public Button previousButton;
    public Button nextButton;
    public Button selectedButton;
    public Button backMainMenuButton;

    // Color
    public int selectedSkinIndex;

    // Text
    public Text coinNumText;

    // Event
    public bool needToChangeColor;

    private void Awake()
    {
        // Load data
        SaveManager.LoadGame();
        playerData = SaveManager.dataList[0];

        // Button
        previousButton.onClick.AddListener(previousButtonEvent);
        nextButton.onClick.AddListener(nextButtonEvent);
        selectedButton.onClick.AddListener(selectButtonEvent);

        // Displayed current skin index
        selectedSkinIndex = playerData.CurrentUsedSkin;

        // Change coin text
        changeCoinText();

        // Event
        needToChangeColor = true;
    }

    void Start()
    {
        selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Used!";
    }

    void previousButtonEvent()
    {
        if (playerData.BuySkinDict[selectedSkinIndex] == false && playerData.Coin < 50)
        {
            selectedButton.interactable = false;
        }
        else
        {
            selectedButton.interactable = true;
        }

        nextButton.interactable = true;

        if (selectedSkinIndex == 2)
        {
            previousButton.interactable = false;
        }

        --selectedSkinIndex;
        needToChangeColor = true;

        // If bought or not bought
        if (playerData.BuySkinDict[selectedSkinIndex] == true)
        {
            selectedButton.interactable = true;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Use";
        }
        else if (selectedSkinIndex == playerData.CurrentUsedSkin)
        {
            selectedButton.interactable = false;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Used!";
        }
        else
        {
            selectedButton.interactable = true;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "$50 Unlock";
        }
    }

    void nextButtonEvent()
    {
        if (playerData.BuySkinDict[selectedSkinIndex] == false && playerData.Coin < 50)
        {
            selectedButton.interactable = false;
        }
        else
        {
            selectedButton.interactable = true;
        }

        previousButton.interactable = true;

        if (selectedSkinIndex == playerData.BuySkinDict.Count-2)
        {
            nextButton.interactable = false;
        }

        ++selectedSkinIndex;
        needToChangeColor = true;

        // If bought or not bought
        if (playerData.BuySkinDict[selectedSkinIndex] == true)
        {
            selectedButton.interactable = true;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Use";
        }
        else if (selectedSkinIndex == playerData.CurrentUsedSkin)
        {
            selectedButton.interactable = false;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Used!";
        }
        else
        {
            selectedButton.interactable = true;
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "$50 Unlock";
        }
    }

    void selectButtonEvent()
    {
        // If bought or not bought
        if (playerData.BuySkinDict[selectedSkinIndex] == true)
        {
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Used!";
            playerData.CurrentUsedSkin = selectedSkinIndex;
            SaveManager.SaveGame(playerData);
        }
        else
        {
            // Coin
            playerData.Coin -= 50;
            changeCoinText();

            // Buy event
            selectedButton.transform.GetChild(0).GetComponent<Text>().text = "Used!";
            playerData.CurrentUsedSkin = selectedSkinIndex;
            SaveManager.SaveGame(playerData);
        }
    }

    void changeCoinText()
    {
        coinNumText.text = playerData.Coin.ToString();
    }
}
