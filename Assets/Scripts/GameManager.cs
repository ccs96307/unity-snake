using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void SpeedUpButtonClicked()
    {
        Player.moveSpeed *= 1.5f;
    } 
    
}
