#region Programming Credits
/* The code for this Player HUD manager was 
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
using System.Security.Cryptography;
#endregion

public class PlayerHudManager : MonoBehaviour
{
    #region Variables and References List
    private VisualElement m_playerHudRoot; // The root of the player HUD UI
    private VisualElement m_HealthPizzaMonitor; // The health bar as depicted by pizza slices
    public List<Sprite> m_HealthPizzaSprites; // The list of pizza slices that make up the health bar
    public VisualElement m_CurrentWeaponContainer; // The text for the current weapon
    public VisualElement m_EntireHud;
    public Label m_WeaponLabel;
    public Label m_CoinLabel;
    private GameObject playerRef;
    private GameObject gameManagerRef;
    private VisualElement m_controlsContainer; // The container showing all active controls for incase you want to set up a Hidden//Visual in the pause menu

    public float fadeDuration = 1.0f; // Adjust the duration as needed
    private float startOpacity = 0.0f;
    private float currentTime = 0.0f;

    private int m_currentSpriteIndex; // The index of the current pizza slice sprite
    #endregion

    public void Start(){
        m_playerHudRoot = GetComponent<UIDocument>().rootVisualElement;
        m_EntireHud = m_playerHudRoot.Q<VisualElement>("PlayerHud");
        m_HealthPizzaMonitor = m_playerHudRoot.Q<VisualElement>("PlayerHealth");
        m_controlsContainer = m_playerHudRoot.Q<VisualElement>("ControlsContainer");
        playerRef = GameObject.Find("Player");
        gameManagerRef = GameObject.Find("GameManager");
        m_WeaponLabel = m_playerHudRoot.Q<Label>("WeaponName");
        m_CoinLabel = m_playerHudRoot.Q<Label>("coinsAmount");
        m_WeaponLabel.text = playerRef.GetComponent<PlayerStateManager>().attackType.ToString().ToUpper();
        m_CoinLabel.text = GameStateController.CoinsCollected.ToString();
        m_currentSpriteIndex = 0;


        m_EntireHud.style.opacity = startOpacity; //Sets the opacity of the HUD to 0 upon start
        StartCoroutine(FadeIn(1,1f)); //Fades in the HUD over 1 second

        GameObject coin = GameObject.Find("Coin");
        if(coin != null) 
        {
            coin.GetComponent<CoinManager>().coinChanged.AddListener(CoinsChanged);
        }
    }

    private void OnEnable()
    {
        PlayerHealth.damageTaken += PlayerHealthChanged;
    }

    private void OnDisable() 
    {
        PlayerHealth.damageTaken -= PlayerHealthChanged;
    }


    /// <summary>
    /// Called when the player takes damage or heals.
    /// Used to update the health bar sprite. 
    /// </summary>
    /// <param name="changeAmount"> The inverse amount of health to change by. 
    /// Positive numbers are damage, negative numbers are healing, as Max Health is 0 and 'end of pizza' is the length of the list.
    /// </param>
    public void PlayerHealthChanged(int changeAmount) {
        if (m_currentSpriteIndex == m_HealthPizzaSprites.Count-1) return; // If the player is already dead, don't do anything)
        m_HealthPizzaMonitor.style.backgroundImage = new StyleBackground(m_HealthPizzaSprites[m_currentSpriteIndex+changeAmount]);
        m_currentSpriteIndex += changeAmount; // Update the current sprite index
    }

    public void CoinsChanged()
    {
        m_CoinLabel.text = GameStateController.CoinsCollected.ToString();
        Debug.Log("Got a coin!");
    }


    //This function changes the opacity value over time.
    public IEnumerator FadeIn(float target, float fadetime)
    {
        currentTime = 0; //Starts our timer at zero
        float currentOpacity = m_EntireHud.style.opacity.value; //cash our current opacity 

        while (currentTime < fadetime) //Runs code until target time is reached
        {
            float t = currentTime / fadetime;
            m_EntireHud.style.opacity = Mathf.Lerp(currentOpacity, target, t);

            currentTime += Time.deltaTime;
            //Debug.Log("Opacity "+ m_EntireHud.style.opacity.value);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        m_EntireHud.style.opacity = target;
        //Debug.Log("Opacity " + m_EntireHud.style.opacity.value);
    }
}
