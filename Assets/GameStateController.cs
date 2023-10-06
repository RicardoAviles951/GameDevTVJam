using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameStateController : MonoBehaviour
{
    private GameObject pauseMenu;
    private InputSystem PauseControls;
    private InputAction pauseAction;
    public static bool isPaused = false;
    public static int CoinsCollected = 0;


    private void Awake()
    {
        PauseControls = new InputSystem();
        pauseMenu = GameObject.Find("PauseMenu");
    }

    private void OnEnable()
    {
        PauseControls.UI.Enable();
        pauseAction = PauseControls.UI.PauseToggle;
        pauseAction.performed += TogglePause;
    }
    private void OnDisable()
    {
        PauseControls.UI.Disable();
        pauseAction.performed -= TogglePause;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 1.0f;
                pauseMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
                //Disable player input
            }
            Debug.Log("PAUSE TOGGLED");
        }
    }

    

}
