using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSlip : MonoBehaviour
{
    private PlayerStateManager player;
    // Start is called before the first frame update

    void Start()
    {
        GameObject _player = GameObject.Find("Player");
        if(_player != null)
        {
            player = _player.GetComponent<PlayerStateManager>();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            player.isSlipping = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            player.isSlipping = false;
        }
    }
}
