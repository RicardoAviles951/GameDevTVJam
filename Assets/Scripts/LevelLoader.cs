using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader instance;
    public static LevelLoader Instance { get { return instance; } }
    private string currentSceneName;
    public GameObject player;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }
    }
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("LevelSwitch");
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Scene should load");
            if(currentSceneName == "KitchenHub")
            {
                SceneManager.LoadScene("LevelPrototype");
                player.transform.position = new Vector3(-13,-10.0030003f,0);
                this.gameObject.transform.position = new Vector3(-1.83000004f,-10.6599998f,0);
            }
        }
    }

   /* private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }*/
    
}
