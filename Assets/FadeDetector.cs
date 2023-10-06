using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDetector : MonoBehaviour
{
    private PlayerHudManager HudManager;
    private Coroutine currentFadeCoroutine; // Store the current fade coroutine

    private void Start()
    {
        HudManager = FindAnyObjectByType<PlayerHudManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            currentFadeCoroutine = StartCoroutine(HudManager.FadeIn(.25f, .25f));

            Debug.Log("Fading UI OUT..");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Stop the current fade coroutine if one is running
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            currentFadeCoroutine = StartCoroutine(HudManager.FadeIn(1, .25f));
            Debug.Log("Fading UI IN..");
        }
    }
    
}
