using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    private string currentSceneName;
    private static CharacterManager instance;
    public static CharacterManager Instance { get { return instance; } }
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "KitchenHub")
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if(currentSceneName != "KitchenHub")
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
