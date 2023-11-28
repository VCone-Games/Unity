using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Color fadeColor;

    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn {  get; private set; }   

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        fadeColor.a = 0f;
    }


    private void Update()
    {
        if(IsFadingOut)
        {
            if(fadeImage.color.a < 1)
            {
                fadeColor.a += Time.deltaTime * fadeSpeed;
                fadeImage.color = fadeColor;
            }
            else
            {
                fadeColor.a = 1 ;
                fadeImage.color = fadeColor;
                IsFadingOut = false;
            }
        }

        if(IsFadingIn)
        {
            if (fadeImage.color.a > 0)
            {
                fadeColor.a -= Time.deltaTime * fadeSpeed;
                fadeImage.color = fadeColor;
            }
            else
            {
                fadeColor.a = 0;
                fadeImage.color = fadeColor;
                IsFadingIn = false;
            }
        }

    }

    public void StartFadeOut()
    {
        IsFadingOut = true;
        fadeImage.color = fadeColor;
    }

    public void StartFadeIn()
    {
        fadeColor.a = 1;
        IsFadingIn = true;
        fadeImage.color = fadeColor;
    }

}
