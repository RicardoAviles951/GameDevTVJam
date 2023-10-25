using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndScreenManager : MonoBehaviour
{
    private VisualElement endScreenRoot;

    public Button restart_button;
    public Button exit_button;
    private int currentSpriteIndex = 0;
    public List<Sprite> sprites;
    public VisualElement coinRoot;
    public Label Tips;
    public Label Pizzas;
    private float timer = 0;
    [SerializeField] float desiredFrameRate = 6f;
    float frameRate = 0;
    public AudioClip hoversound;
    public AudioClip clicksound;
    public FadeController fader;
    private float currentTime;

    private void Start()
    {
        endScreenRoot.style.opacity = 0;
        StartCoroutine(FadeIn(1f, 1f));
        coinRoot      = endScreenRoot.Q<VisualElement>("Sprite");
        Tips          = endScreenRoot.Q<Label>("CoinCount");
        Tips.text     = GameStateController.CoinsCollected.ToString();
        Pizzas        = endScreenRoot.Q<Label>("Lbl_droppedcounter");
        Pizzas.text = GameStateController.PizzasDropped.ToString();
        frameRate = desiredFrameRate / 60;
        coinRoot.style.backgroundImage = new StyleBackground(sprites[currentSpriteIndex]);

        
        fader.FadeOut();

    }

    private void Update()
    {
        frameRate = desiredFrameRate / 60;
        if(timer < frameRate)
        {
            //Debug.Log("Waiting...");
        }
        else
        {
            if(currentSpriteIndex >= sprites.Count-1)
            {
                currentSpriteIndex = 0;
            }
            else
            {
                currentSpriteIndex += 1;
            }
            
            coinRoot.style.backgroundImage = new StyleBackground(sprites[currentSpriteIndex]);
            //Debug.Log("CurrentIndex..." + currentSpriteIndex);
            timer = 0;
        }
        timer += 1 * Time.deltaTime;
    }

    private void OnEnable()
    {

        endScreenRoot = GetComponent<UIDocument>().rootVisualElement;
        restart_button = endScreenRoot.Q<Button>("restart-button");
        exit_button = endScreenRoot.Q<Button>("exit-button");

        restart_button.clicked += RestartClicked;
        exit_button.clicked += ExitClicked;

        // Add a hover event handler for the restart button
        restart_button.RegisterCallback<PointerEnterEvent>(OnRestartButtonEnter);
        exit_button.RegisterCallback<PointerEnterEvent>(OnExitButtonEnter);
    }

    private void OnDisable()
    {
        restart_button.clicked -= RestartClicked;
        exit_button.clicked -= ExitClicked;
    }
    void RestartClicked()
    {
        Debug.Log("Restarting!");
        SoundManager.Instance.PlaySound(clicksound);
        StartCoroutine(FadeIn(0, 1));
        GameStateController.CoinsCollected = 0;
        fader.FadeOut();
        GameStateController.isPaused = false;
        StartCoroutine(LoadNextLevel());

    }

    void ExitClicked()
    {
        Debug.Log("Exiting!");
        Debug.Log("Ending application...");
        SoundManager.Instance.StopMusic();
        Application.Quit(); 
    }

    void OnRestartButtonEnter(PointerEnterEvent e)
    {
        Debug.Log("Entered restart button");
        SoundManager.Instance.PlaySound(hoversound);
    }

    void OnExitButtonEnter(PointerEnterEvent e)
    {
        Debug.Log("Entered exit button");
        SoundManager.Instance.PlaySound(hoversound);
    }


    public IEnumerator FadeIn(float target, float fadetime)
    {
        currentTime = 0; //Starts our timer at zero
        float currentOpacity = endScreenRoot.style.opacity.value; //cash our current opacity 

        while (currentTime < fadetime) //Runs code until target time is reached
        {
            float t = currentTime / fadetime;
            endScreenRoot.style.opacity = Mathf.Lerp(currentOpacity, target, t);

            currentTime += Time.deltaTime;
            //Debug.Log("Opacity "+ m_EntireHud.style.opacity.value);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        endScreenRoot.style.opacity = target;
        //Debug.Log("Opacity " + m_EntireHud.style.opacity.value);
    }


    public IEnumerator LoadNextLevel()
    {
        Debug.Log("Loading next level...");
        yield return new WaitForSeconds(1); //Waits one second
        SceneManager.LoadScene(0);//Loads next scene
    }
}
