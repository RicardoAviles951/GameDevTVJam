#region Programming Credits
/* The code for this Pause Menu manager was 
* written by Meagan Couture. 
* Please contact meagancouture@comcast.net or 
* CoralineDark on Discord if you have any questions.
*/
#endregion
#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#endregion

public class PauseMenuController : MonoBehaviour
{
    #region Variables and References List
    private VisualElement m_pauseRoot; // The root of the pause menu UI
    private Button m_playBTN; // Reference to the play button
    private Button m_quitBTN; // Reference to the quit button
    private Button m_returnBTN; // Reference to the return to title button
    
    private PlayerInput playerInput;
    #endregion

    private void OnEnable()
    {
        //GameObject player = GameObject.FindWithTag("Player"); 
        //playerInput = player.GetComponent<PlayerInput>(); 
        
        m_pauseRoot = GetComponent<UIDocument>().rootVisualElement;
        m_playBTN = m_pauseRoot.Q<Button>("playBTN");
        m_quitBTN = m_pauseRoot.Q<Button>("quitBTN");
        m_returnBTN = m_pauseRoot.Q<Button>("returnBTN");

        m_playBTN.clicked += PlayPressed; 
        m_quitBTN.clicked += QuitPressed; 
        m_returnBTN.clicked += ReturnPressed;
       
    }

    private void OnDisable()
    {
        m_playBTN.clicked -= PlayPressed; 
        m_quitBTN.clicked -= QuitPressed; 
        m_returnBTN.clicked -= ReturnPressed; 
    }

    /// <summary>
    /// Called when the player presses the play button
    /// </summary>
    private void PlayPressed()
    {
        //playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1; 
        Debug.Log("Pause Menu Play Pressed");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the player presses the quit button
    /// </summary>
    private void QuitPressed()
    {
        Debug.Log("Pause Menu Quit Pressed");
        //Application.Quit(); 
    }

    /// <summary>
    /// Called when the player presses the return button
    /// </summary>
    private void ReturnPressed()
    {
        Debug.Log("Pause Menu Return Pressed");
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync("KitchenHub");
    }
}
