using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class YarnFloorCheck : MonoBehaviour
{
    private static YarnFloorCheck instance;
    public static YarnFloorCheck Instance { get { return instance; } }
    public GameObject player;
    public bool usedOnce;
    public DialogueRunner drYarn;
    public string nodeName;
    private string currentSceneName;
    List<string> fireFloorDialogue = new List<string> {"TowerIntro", "FF1", "FF2", "FF3"};
    List<string> waterFloorDialogue = new List<string> {"WFEntry", "WF1", "WF2", "WF3"};
    List<string> airFloorDialogue = new List<string> {"AFEntry", "AF1", "AF2", "AF3"};
    List<string> earthFloorDialogue = new List<string> {"EFEntry", "EF1", "EF2", "EF3"};


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
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            if (!usedOnce && currentSceneName == "PrototypeLevel")
            {
                if(YarnPlayerFloorCheck.fireFloor == false)
                {
                    nodeName = fireFloorDialogue[0];
                    drYarn.StartDialogue(nodeName);
                }
                else if(YarnPlayerFloorCheck.fireFloor == true)
                {
                    int randomDialogue = Random.Range(1, 3);
                    nodeName = fireFloorDialogue[randomDialogue];
                    drYarn.StartDialogue(nodeName);
                }
                YarnPlayerFloorCheck.fireFloor = true;
            }
            else if (!usedOnce && this.gameObject.tag == "WaterFloor")
            {
                if (YarnPlayerFloorCheck.waterFloor == false)
                {
                    nodeName = waterFloorDialogue[0];
                    drYarn.StartDialogue(nodeName);
                }
                else if (YarnPlayerFloorCheck.waterFloor == true)
                {
                    int randomDialogue = Random.Range(1, 3);
                    nodeName = waterFloorDialogue[randomDialogue];
                    drYarn.StartDialogue(nodeName);
                }
                YarnPlayerFloorCheck.waterFloor = true;
            }
           /* else if (!usedOnce && this.gameObject.tag == "AirFloor")
            {
                if (player.GetComponent<YarnPlayerFloorCheck>().airFloor == false)
                {
                    nodeName = airFloorDialogue[0];
                    drYarn.StartDialogue(nodeName);
                }
                else if (player.GetComponent<YarnPlayerFloorCheck>().airFloor == true)
                {
                    int randomDialogue = Random.Range(1, 3);
                    nodeName = airFloorDialogue[randomDialogue];
                    drYarn.StartDialogue(nodeName);
                }
                player.GetComponent<YarnPlayerFloorCheck>().airFloor = true;
            }
            else if (!usedOnce && this.gameObject.tag == "EarthFloor")
            {
                if (player.GetComponent<YarnPlayerFloorCheck>().earthFloor == false)
                {
                    nodeName = earthFloorDialogue[0];
                    drYarn.StartDialogue(nodeName);
                }
                else if (player.GetComponent<YarnPlayerFloorCheck>().earthFloor == true)
                {
                    int randomDialogue = Random.Range(1, 3);
                    nodeName = earthFloorDialogue[randomDialogue];
                    drYarn.StartDialogue(nodeName);
                }
                player.GetComponent<YarnPlayerFloorCheck>().earthFloor = true;
            }*/
        }
    }
}
