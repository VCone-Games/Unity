using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager: MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip[] audioClips;

    public int actualClip;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        musicSource.clip = audioClips[actualClip];
        musicSource.Play();
    }

    public void ChangeMusic(int clipId)
    {
        actualClip = clipId;
        musicSource.clip = audioClips[actualClip];
        musicSource.Play();
    }

    public void ChangeFootstepsClip(int stepsId)
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerSoundManager>().SetGroundMaterial(stepsId);
    }
}
