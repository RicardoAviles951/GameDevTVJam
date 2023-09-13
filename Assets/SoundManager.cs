using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Singleton Pattern
    public static SoundManager Instance;
    public AudioSource musicSource, effectsSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        

    }

    public void PlaySound(AudioClip clip, float pitch = 1.0f)
    {
        effectsSource.clip = clip;
        effectsSource.pitch = pitch;
        effectsSource.PlayOneShot(effectsSource.clip);

    }

}
