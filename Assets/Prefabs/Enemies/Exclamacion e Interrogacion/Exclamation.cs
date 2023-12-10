using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exclamation : MonoBehaviour
{
    private Animator myAnimator;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    public void Spawn()
    {
        myAnimator.SetTrigger("spawn");
    }
}
