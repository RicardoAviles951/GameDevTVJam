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
    public static int PizzasDropped = 0;
    public static int CoinsCollected = 0;
    public static int startCoins = 0; 


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
        startCoins = CoinsCollected;
       // Debug.Log("Current Coins = " + CoinsCollected);
        //Debug.Log("Start Coins = " +  startCoins);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (pauseMenu.activeSelf)
            {
                SoundManager.Instance.ResumeMusic();
                Time.timeScale = 1.0f;
                pauseMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                SoundManager.Instance.PauseMusic();
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
                //Disable player input
            }
            //Debug.Log("PAUSE TOGGLED");
        }
    }

    

}
