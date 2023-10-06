using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image ScreenCover;
    public Animator FadeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        FadeAnimator = ScreenCover.GetComponent<Animator>();
        Color c = ScreenCover.color;
        c.a = 1.0f;
        ScreenCover.color = c;
        
    }

    public void FadeOut()
    {
        FadeAnimator.SetTrigger("FadeOut");
    }
}
