using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Pause UI
    public Button PauseButton;
    public Button RestartButton;
    public Button BackMenuButton;
    public GameObject PauseWindow;
    private bool isPause;

    void Start()
    {
        isPause = false;
        PauseButton.onClick.AddListener(PauseGame);
        RestartButton.onClick.AddListener(ReloadScene);
        BackMenuButton.onClick.AddListener(BackMainMenu);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPause = true;
        Time.timeScale = 1;
    }

    void PauseGame()
    {
        isPause = !isPause;

        if (isPause == true)
        {
            PauseButton.image.sprite = Resources.Load<Sprite>("Sprites/resume");
            PauseWindow.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseButton.image.sprite = Resources.Load<Sprite>("Sprites/pause");
            PauseWindow.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void BackMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}


