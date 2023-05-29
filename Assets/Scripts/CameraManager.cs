using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }

    private float[] cameraAnchors;
    private Camera _camera;

    [Range(0,2)]
    public int pos = 0;
    private Transform camTransform;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }

        _camera = Camera.main;
        camTransform = _camera.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraAnchors = new float[3];
        for(int i = 0; i < 3; i++)
        {
            float anchors = i*23.5f;
            
            cameraAnchors[i] = anchors;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets camera to proper position based on section
        camTransform.position = new Vector3(camTransform.position.x,cameraAnchors[pos],camTransform.position.z);
         
    }
}
