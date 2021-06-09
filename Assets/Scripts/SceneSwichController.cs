using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneSwichController : MonoBehaviour
{
    public void StartButtonClick()
    {
        SceneManager.LoadScene(0);
    }

}
