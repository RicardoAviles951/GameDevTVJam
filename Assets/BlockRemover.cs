using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRemover : MonoBehaviour
{
    public static bool removeBlocker;
    // Start is called before the first frame update
    void Start()
    {
        removeBlocker = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(removeBlocker)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
