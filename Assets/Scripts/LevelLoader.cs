using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader instance;
    public static LevelLoader Instance { get { return instance; } }
    private string currentSceneName;
    //private bool dialogueStop;
    public GameObject player;
    public GameObject floorCheck;
    public DialogueRunner drYarn;
    public InMemoryVariableStorage variableStorage;
    public FadeController fadeController;
    
    
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
       // dialogueStop = false;
        player = GameObject.FindGameObjectWithTag("Player");
        floorCheck = GameObject.FindGameObjectWithTag("FloorCheck");
        drYarn = GameObject.FindGameObjectWithTag("Dialogue System").GetComponent<DialogueRunner>();
        variableStorage = GameObject.FindGameObjectWithTag("Dialogue System").GetComponent<InMemoryVariableStorage>();
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
            fadeController.FadeOut();
            Debug.Log("Scene should load");
            if(currentSceneName == "KitchenHub")
            {
               // SceneManager.LoadScene("LevelPrototype");
                StartCoroutine(NextScene("LevelPrototype"));
                StopDialogue();
                player.transform.position = new Vector3(-13,-10.0030003f,0);
                this.gameObject.transform.position = new Vector3(-1.83000004f,-10.6599998f,0);
                floorCheck.gameObject.transform.position = new Vector3(-5.3893528f, -10.5200005f, 0);
            }
            if(currentSceneName == "LevelPrototype")
            {
               // SceneManager.LoadScene("Level2");
                StartCoroutine(NextScene("Level2"));
                StopDialogue();
                player.transform.position = new Vector3(4.88000011f, -3.6400001f, 0);
                this.gameObject.transform.position = new Vector3(-1.83000004f, -10.6599998f, 0);
                floorCheck.gameObject.transform.position = new Vector3(13.2202072f, 4.63999987f, 0);
            }
        }
    }

    private void StopDialogue()
    {
        Debug.Log("This has run");
        if(drYarn != null && drYarn.IsDialogueRunning)
        {
            bool yes = true;
            variableStorage.SetValue("$yes", yes);
        }
    }

    private IEnumerator NextScene(string scene)
    {
        Debug.Log("Loading next scene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
    
}
