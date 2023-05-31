using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.SceneManagement;


public class YarnDialogueBubbleDisplay : DialogueViewBase
{
    public static YarnDialogueBubbleDisplay instance;
    public static YarnDialogueBubbleDisplay Instance { get { return instance;  } } // very minimal implementation of singleton manager (initialized lazily in Awake)
    public List<YarnCharacter> allCharacters = new List<YarnCharacter>();
    Camera worldCamera;

    [Tooltip("Display dialogue choices for this character, and display any no-name dialogue here too.")]
    public YarnCharacter playerCharacter;
    YarnCharacter speakerCharacter;

    public Canvas canvas;
    public CanvasScaler canvasScaler;

    [Tooltip("For best results, set teh rectTransform anchors to middle-center, and make sure the rect Transform's pivot Y is set to 0")]
    public RectTransform dialogueBubbleRect, optionsBubbleRect;

    [Tooltip("margin is 0-1.0 (0.1 means 10% of screen space)... -1 lets dialogue bubbles appear offscreen or get cutoff")]
    public float bubbleMargin = 0.1f;

    //<summary> The below line is to add a normal Line View so that Alverius can speak through that instead of a dialogue bubble
    public RectTransform normalLineView;
    public Vector3 offScreenPoint;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }
        
        worldCamera = Camera.main;
    }

    
    public void RegisterYarnCharacter(YarnCharacter newCharacter)
    {
        if(!YarnDialogueBubbleDisplay.instance.allCharacters.Contains(newCharacter))
        {
            allCharacters.Add(newCharacter);
        }
    }

    public void ForgetYarnCharacter(YarnCharacter deletedCharacter)
    {
        if(YarnDialogueBubbleDisplay.instance.allCharacters.Contains(deletedCharacter))
        {
            allCharacters.Remove(deletedCharacter);
        }
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        string characterName = dialogueLine.CharacterName;

        speakerCharacter = !string.IsNullOrEmpty(characterName) ? FindCharacter(characterName) : null;

        onDialogueLineFinished();
    }
    YarnCharacter FindCharacter (string searchName)
    {
        foreach(var character in allCharacters)
        {
            if(character.characterName == searchName)
            {
                return character;
            }
        }

        Debug.LogWarningFormat("YarnCharacterView couldn't find a YarnCharacter named {0}!", searchName);
        return null;
    }

    Vector2 WorldToAnchoredPosition(RectTransform bubble, Vector3 worldPos, float constrainToViewportMargin = -1f)
    {
        Camera canvasCamera = worldCamera;

        if(canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            canvasCamera = null;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bubble.parent.GetComponent<RectTransform>(),
            worldCamera.WorldToScreenPoint(worldPos),
            canvasCamera,
            out Vector2 screenPos
        );

        if(constrainToViewportMargin >= 0f)
        {
            bool useCanvasResolution = canvasScaler != null && canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize;
            Vector2 screenSize = Vector2.zero;
            screenSize.x = useCanvasResolution ? canvasScaler.referenceResolution.x : Screen.width;
            screenSize.y = useCanvasResolution ? canvasScaler.referenceResolution.y : Screen.height;

            var halfBubbleWidth = bubble.rect.width / 2;
            var halfBubbleHeight = bubble.rect.height / 2;

            var margin = screenSize.x < screenSize.y ? screenSize.x * constrainToViewportMargin : screenSize.y * constrainToViewportMargin;

            screenPos.x = Mathf.Clamp(
                screenPos.x,
                margin + halfBubbleWidth - bubble.anchorMin.x * screenSize.x,
                -(margin + halfBubbleWidth) - bubble.anchorMax.x * screenSize.x + screenSize.x
            );
        }
        return screenPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<YarnCharacter>();
        if(dialogueBubbleRect.gameObject.activeInHierarchy)  
            {
                if(speakerCharacter != null && speakerCharacter.characterName == "Alverius")
                    {
                        dialogueBubbleRect.anchoredPosition = WorldToAnchoredPosition(dialogueBubbleRect, offScreenPoint, bubbleMargin);
                        normalLineView.anchoredPosition = WorldToAnchoredPosition(normalLineView, speakerCharacter.positionWithOffset, bubbleMargin);
                    }
                else if(speakerCharacter != null && speakerCharacter.characterName != "Alverius") 
                    {
                        Debug.Log("BOX OVER CHARACTER");
                        normalLineView.anchoredPosition = WorldToAnchoredPosition(normalLineView, offScreenPoint, bubbleMargin);
                        dialogueBubbleRect.anchoredPosition = WorldToAnchoredPosition(dialogueBubbleRect, speakerCharacter.positionWithOffset, bubbleMargin);
                    }
                else if (speakerCharacter == null)
                    {
                        Debug.Log("BOX OVER PLAYER");
                        normalLineView.anchoredPosition = WorldToAnchoredPosition(normalLineView, offScreenPoint, bubbleMargin);
                        dialogueBubbleRect.anchoredPosition = WorldToAnchoredPosition(dialogueBubbleRect, playerCharacter.positionWithOffset, bubbleMargin);
                    }
            }

        if(optionsBubbleRect.gameObject.activeInHierarchy)
        {
            optionsBubbleRect.anchoredPosition = WorldToAnchoredPosition(optionsBubbleRect, playerCharacter.positionWithOffset, bubbleMargin);
        }

    }

    

}
