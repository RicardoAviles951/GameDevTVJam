using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    private bool goalReached = false;
    public AudioClip goalSound;
    public FadeController fadeController;
    public PlayerHudManager playerHudManager;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = GameObject.Find("Player");
        if(other.gameObject == player)
        {
            
            if(goalReached == false)
            {
                StartCoroutine(playerHudManager.FadeIn(0, .5f));
                fadeController.FadeOut();
                SoundManager.Instance.PlaySound(goalSound);
                Debug.Log("Goal Collider: Player Entered");
                goalReached = true;
                StartCoroutine(LoadNextLevel());
            }
            
        }
    }

    public IEnumerator LoadNextLevel()
    {
        Debug.Log("Loading next level...");
        yield return new WaitForSeconds(1); //Waits one second
        Scene currentScene = SceneManager.GetActiveScene();//Gets active scene
        int index = currentScene.buildIndex;//Gets scene index
        SceneManager.LoadScene(index + 1);//Loads next scene

    }
}
