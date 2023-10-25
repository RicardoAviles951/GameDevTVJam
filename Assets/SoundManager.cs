using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //Singleton Pattern
    public static SoundManager Instance;
    public AudioSource musicSource, effectsSource;
    public AudioClip MorningPizza, PizzaRun;
    [Range(0f, 1f)] public float maxVolume = .50f;

    private Dictionary<string, AudioClip> sceneMusic = new Dictionary<string, AudioClip>();

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

        sceneMusic.Add("KitchenHub", MorningPizza);
        sceneMusic.Add("LevelPrototype", PizzaRun);
        sceneMusic.Add("Level2", PizzaRun);
        sceneMusic.Add("EndScene", MorningPizza);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (musicSource != null && sceneMusic.ContainsKey(sceneName))
        {
            musicSource.clip = sceneMusic[sceneName];
            FadeIn(musicSource.clip);
        }
    }

    private void Start()
    {
        
        
    }

    public void FadeIn(AudioClip music)
    {
        if (musicSource != null)
        {
            musicSource.clip = music;
            musicSource.volume = 0;
            musicSource.Play();
            StartCoroutine(Delay());
        }
        //musicSource.clip = music;
        
        
       
    }
    public void PlaySound(AudioClip clip, float pitch = 1.0f)
    {
        effectsSource.clip = clip;
        effectsSource.pitch = pitch;
        effectsSource.PlayOneShot(effectsSource.clip);

    }

    private IEnumerator Delay()
    {
        while (musicSource.volume < maxVolume)
        {
            musicSource.volume += .25f*Time.deltaTime;
            yield return null;
           // Debug.Log("Music volume " + musicSource.volume);
        }

        musicSource.volume = maxVolume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    public void ResumeMusic()
    {
        musicSource.Play();
    }

}
