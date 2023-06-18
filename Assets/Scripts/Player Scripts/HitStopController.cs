using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    public float hitStopDuration = 0.1f; // Adjust this value as desired

    public IEnumerator HitStopCoroutine()
    {
        Time.timeScale = 0f; // Freeze the gameplay
        yield return new WaitForSecondsRealtime(hitStopDuration); // Wait for the hit stop duration
        Time.timeScale = 1f; // Resume the gameplay
    }
}
