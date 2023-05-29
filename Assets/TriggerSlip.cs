using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSlip : MonoBehaviour
{
    public PlayerStateManager player;
    // Start is called before the first frame update

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
