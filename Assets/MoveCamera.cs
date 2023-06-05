using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Camera _cam;
    private CameraManager camManager;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        camManager = _cam.GetComponent<CameraManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            //Get position of trigger box
            Vector2 pos = transform.position;
            //Position variable to collider position
            camManager.position = new Vector3(pos.x,pos.y, -10);
//            print(transform.position);
        }
    }
    
}
