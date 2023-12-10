using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exclamation : MonoBehaviour
{
    public bool interrogation;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }
    public void Spawn()
    {
        myRenderer.enabled = true;
        myAnimator.SetTrigger("spawn");
    }

    public void Disappear()
    {
        myRenderer.enabled = false;
    }
}
