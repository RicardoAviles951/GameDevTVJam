using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }
    private Camera _camera;
    private Transform camTransform;
    private string currentSceneName;
    public Vector3 position;
    [Header("Camera Shake Parameters")]
    public float shakeDuration = 0f;
    public float shakeMagnitude = 0.7f;
    public float dampingSpeed = 1.0f;
    public bool isShaking = false;


    private void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }

        //Set new camera position
        position = new Vector3(0,0,-10);
        //Cache camera
        _camera = Camera.main;
        //Cache camera transform
        camTransform = _camera.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        camTransform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets camera to proper position based on section
        camTransform.position = position;

        currentSceneName = SceneManager.GetActiveScene().name;

        if(currentSceneName == "KitchenHub")
        {
            _camera.gameObject.GetComponent<PixelPerfectCamera>().assetsPPU = 32;
        } 
        else if (currentSceneName != "KitchenHub")
        {
            _camera.gameObject.GetComponent<PixelPerfectCamera>().assetsPPU = 16;
        }

        if(shakeDuration > 0f)
        {
            Vector3 localPos = camTransform.localPosition;
            Debug.Log("Camera shake!!");
            isShaking = true;
            camTransform.localPosition = localPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            isShaking = false;
            shakeDuration = 0;
        }

    }

    public void CameraShake(float duration, float magnitude)
    {
        Debug.Log("Camera Shake called...");
        shakeDuration = duration;
        shakeMagnitude = magnitude;

    }

   
}
