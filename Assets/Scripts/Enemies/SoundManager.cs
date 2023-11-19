using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource atackSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource dieSource;

    [Header("Sonidos")]
    [SerializeField] private AudioClip atack;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;

    private void Start()
    {

    }
    public void PlayAtack()
    {
        atackSource.clip=  atack;
        atackSource.volume = 1.0f;
        atackSource.Play();
    }

    public void PlayDie()
    {
        dieSource.clip= die;
        dieSource.volume = 1.0f;
        dieSource.Play();
    }
    public void PlayHit()
    {
        hitSource.clip=   hit;
        hitSource.volume = 1.0f;
        hitSource.Play();
    }
}

