using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnPlayerFloorCheck : MonoBehaviour
{
    private static YarnPlayerFloorCheck instance;
    public static YarnPlayerFloorCheck Instance {get{return instance; } }
    
    public static bool fireFloor;
    public static bool waterFloor;
    public static bool airFloor;
    public static bool earthFloor;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }
    }
    // Start is called before the first frame update
    void Start()
    {
        fireFloor = false;
        waterFloor = false;
        airFloor = false;
        earthFloor = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
