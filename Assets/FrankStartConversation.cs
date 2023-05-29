using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class FrankStartConversation : MonoBehaviour
{
    private static FrankStartConversation instance;
    public static FrankStartConversation Instance { get { return instance; } }
    public GameObject player;
    public bool usedOnce;
    public bool randomConvo;
    public DialogueRunner drYarn;
    public string nodeName;
    List<string> introDialogue = new List<string> {"FrankIntro"};
    List<string> randomDialogue = new List<string> {"FrankTalk1", "FrankTalk2", "FrankTalk3", "FrankTalk4"};
    private string currentSceneName;
    private bool sceneCheck;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }

        player = GameObject.FindGameObjectWithTag("Player");
        drYarn = GameObject.FindGameObjectWithTag("Dialogue System").GetComponent<DialogueRunner>();
    }
    // Start is called before the first frame update
    void Start()
    {
        usedOnce = false;
        randomConvo = false;
        sceneCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "KitchenHub" && sceneCheck == true)
        {
            randomConvo = false;
            sceneCheck = false;
        }
        else if (sceneCheck && currentSceneName != "KitchenHub")
        {
            Debug.Log("is this running?");
            sceneCheck = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentSceneName == "KitchenHub"){
            if(collision.gameObject == player)
            {
                if(!usedOnce)
                {
                    nodeName = introDialogue[0];
                    drYarn.StartDialogue(nodeName);
                    usedOnce = true;
                    BlockRemover.removeBlocker = true;
                }
                else if (usedOnce && !randomConvo)
                {
                    int randomNumber = Random.Range(1,3);
                    nodeName = randomDialogue[randomNumber]; 
                    drYarn.StartDialogue(nodeName);
                    randomConvo = true;
                }
            }
        }
    }
}
