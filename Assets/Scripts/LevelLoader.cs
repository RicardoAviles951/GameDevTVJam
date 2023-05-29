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
    public GameObject floorCheck;
    
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
        player = GameObject.FindGameObjectWithTag("Player");
        floorCheck = GameObject.FindGameObjectWithTag("FloorCheck");
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
                floorCheck.gameObject.transform.position = new Vector3(-5.3893528f, -10.5200005f, 0);
            }
            if(currentSceneName == "LevelPrototype")
            {
                SceneManager.LoadScene("KitchenHub");
                player.transform.position = new Vector3(4.88000011f, -3.6400001f, 0);
                this.gameObject.transform.position = new Vector3(-1.83000004f, -10.6599998f, 0);
                floorCheck.gameObject.transform.position = new Vector3(13.2202072f, 4.63999987f, 0);
            }
        }
    }
    
}
