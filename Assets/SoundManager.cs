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
        sceneMusic.Add("Level2", MorningPizza);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneMusic.ContainsKey(sceneName))
        {
            //musicSource.clip = sceneMusic[sceneName];
            //FadeIn(musicSource.clip);
        }
    }

    private void Start()
    {
        
        
    }

    public void FadeIn(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.volume = 0;
        musicSource.Play();
        StartCoroutine(Delay());
        
       
    }
    public void PlaySound(AudioClip clip, float pitch = 1.0f)
    {
        effectsSource.clip = clip;
        effectsSource.pitch = pitch;
        effectsSource.PlayOneShot(effectsSource.clip);

    }

    private IEnumerator Delay()
    {
        while (musicSource.volume < 1)
        {
            musicSource.volume += .25f*Time.deltaTime;
            yield return null;
           // Debug.Log("Music volume " + musicSource.volume);
        }

        musicSource.volume = 1;
    }

}
