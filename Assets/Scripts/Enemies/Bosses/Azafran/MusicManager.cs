using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Sonidos")]
    [SerializeField] private AudioClip first;
    [SerializeField] private AudioClip interlude;
    [SerializeField] private AudioClip second;

    public void PlayFirst()
    {
        musicSource.clip=  first;
        musicSource.Play();
    }

    public void PlayInterlude()
    {
        musicSource.clip= interlude;
        musicSource.PlayOneShot(interlude);
        musicSource.clip =   second;
    }
    public void PlaySecond()
    { 
        musicSource.Play();
    }
}
