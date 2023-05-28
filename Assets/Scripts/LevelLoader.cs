using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && this.gameObject.tag == "Level Loader 1")
        {
            Debug.Log("Scene should load");
            SceneManager.LoadScene("SamTestScene2");
        }
        else if(collision.gameObject.tag == "Player" && this.gameObject.tag == "Level Loader 2")
        {
            SceneManager.LoadScene("SamTestScene");
        }
    }
}
