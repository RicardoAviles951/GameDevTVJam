using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
   public void LevelRestart()
    {
        //Gets current level index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Coins before restart " + GameStateController.CoinsCollected);
        GameStateController.CoinsCollected = GameStateController.startCoins;
        //Loads current scene again. 
        SceneManager.LoadScene(currentSceneIndex);
    }
}
