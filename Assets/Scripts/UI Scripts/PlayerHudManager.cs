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
#endregion

public class PlayerHudManager : MonoBehaviour
{
    #region Variables and References List
    private VisualElement m_playerHudRoot; // The root of the player HUD UI
    private VisualElement m_HealthPizzaMonitor; // The health bar as depicted by pizza slices
    public List<Sprite> m_HealthPizzaSprites; // The list of pizza slices that make up the health bar
    public VisualElement m_CurrentWeaponContainer; // The text for the current weapon
    public Label m_WeaponLabel;
    private GameObject playerRef;
    private VisualElement m_controlsContainer; // The container showing all active controls for incase you want to set up a Hidden//Visual in the pause menu
    
    private int m_currentSpriteIndex; // The index of the current pizza slice sprite
    #endregion

    public void Start(){
        m_playerHudRoot = GetComponent<UIDocument>().rootVisualElement;
        m_HealthPizzaMonitor = m_playerHudRoot.Q<VisualElement>("PlayerHealth");
        m_controlsContainer = m_playerHudRoot.Q<VisualElement>("ControlsContainer");
        playerRef = GameObject.Find("Player");
        m_WeaponLabel = m_playerHudRoot.Q<Label>("WeaponName");
        m_WeaponLabel.text = playerRef.GetComponent<PlayerStateManager>().attackType.ToString().ToUpper();
        m_currentSpriteIndex = 0;
        //Debug.Log(m_WeaponLabel.ToString());
    }

    private void OnEnable() {
        PlayerHealth.damageTaken += PlayerHealthChanged;
    }

    private void OnDisable() {
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
}
