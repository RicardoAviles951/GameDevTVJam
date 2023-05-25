using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnCharacter : MonoBehaviour
{
    [Tooltip("Must match character name in Yarn dialogue scripts.")]
    public string characterName = "MyName";
    [Tooltip("When positioning message bubble in world space, YarnCharacterManager adds this additional offset to this gameObject's position. Taller characters should us taller offsets, etc.")]
    public Vector3 dialogueBubbleOffset = new Vector3(0f, 3f, 0f);
    [Tooltip("If true, then apply dialogueBubbleOffset relative to this transform's rotation and scale")]
    public bool offsetUsesRotation = false;
    // above bool is most likely not needed since game is 2D

    public Vector3 positionWithOffset
    {
        get {
            if (!offsetUsesRotation)
            {
                return transform.position + dialogueBubbleOffset;
            }
            else
            {
                return transform.position + transform.TransformPoint(dialogueBubbleOffset); //convert offset into local space
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (YarnDialogueBubbleDisplay.instance == null)
        {
            Debug.LogError("YarnCharacter can't find the YarnDialogueBubbleDisplay instance! Is the 3D Dialogue prefab and YarnDialogueBubbleDisplay in the scene?");
            return;
        }

        //YarnDialogueBubbleDisplay.instance.;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
