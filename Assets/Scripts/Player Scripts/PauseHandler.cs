using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    public InputActionReference pauseToggle;
    public GameObject pauseMenu; //The UI object that hold the menu
   
    private void Awake() {

    } 

    void OnEnable() {
        pauseToggle.action.performed += OnPauseTogglePressed; 
       
    }
    
    void OnDisable() {
        pauseToggle.action.performed -= OnPauseTogglePressed;
    }

    public void OnPauseTogglePressed(InputAction.CallbackContext context) {
        


        //if (pauseMenu.activeSelf) {
        //    playerInput.SwitchCurrentActionMap("Player");
        //    Time.timeScale = 1; 
        //    Debug.Log("Pause Menu off"); 
        //    pauseMenu.SetActive(false); 
        //} else {
        //    playerInput.SwitchCurrentActionMap("UI");
        //    Time.timeScale = 0; 
        //    Debug.Log("Pause Menu on"); 
        //    pauseMenu.SetActive(true);
        //}
    }
}
