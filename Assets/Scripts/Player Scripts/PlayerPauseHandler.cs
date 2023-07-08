using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerPauseHandler : MonoBehaviour
{
    public InputActionReference pauseToggleOn, pauseToggleOff;
    private PlayerInput playerInput;
    public GameObject pauseMenu;
   
    private void Awake() {
        playerInput = (GameObject.FindWithTag("Player")).GetComponent<PlayerInput>();
    } 

    void OnEnable() {
        pauseToggleOn.action.performed += OnPauseTogglePressed; 
        pauseToggleOff.action.performed += OnPauseTogglePressed;
    }
    
    void OnDisable() {
        pauseToggleOn.action.performed -= OnPauseTogglePressed;
        pauseToggleOff.action.performed -= OnPauseTogglePressed; 
    }

    public void OnPauseTogglePressed(InputAction.CallbackContext ctx) {
        if (pauseMenu.activeSelf) {
            playerInput.SwitchCurrentActionMap("Player");
            Time.timeScale = 1; 
            Debug.Log("Pause Menu off"); 
            pauseMenu.SetActive(false); 
        } else {
            playerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0; 
            Debug.Log("Pause Menu on"); 
            pauseMenu.SetActive(true);
        }
    }
}
