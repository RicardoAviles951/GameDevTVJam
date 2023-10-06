using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    public float hitStopDuration = 10.0f; // Adjust this value as desired

    public IEnumerator HitStopCoroutine()
    {
        Debug.Log("TIMESTOP");
        Time.timeScale = 0f; // Freeze the gameplay
        yield return new WaitForSecondsRealtime(hitStopDuration); // Wait for the hit stop duration
        Debug.Log("TIME RESUME");
        Time.timeScale = 1f; // Resume the gameplay
    }
}
