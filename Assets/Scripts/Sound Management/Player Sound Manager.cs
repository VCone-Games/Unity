using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private AudioSource landingSource;

    [Header("Footsteps Sounds: 0-Metal, 1-Snow, 2-Grass, 3-Stone")]
    [SerializeField] public int materialInicial;
    [SerializeField] private AudioClip[] footSteps;

    [Header("Footsteps Volumes: 0-Metal, 1-Snow, 2-Grass, 3-Stone")]
    [SerializeField] private float[] footStepsVOLUME;

    [Header("Footsteps Control Variables")]
    [SerializeField] private int currentMaterialId;
    [SerializeField] private bool footstepsPlaying;

    [Header("Landing Sounds: 0-Metal, 1-Snow, 2-Grass, 3-Stone")]
    [SerializeField] private AudioClip[] landingSounds;

    [Header("Landing Volumes: 0-Metal, 1-Snow, 2-Grass, 3-Stone")]
    [SerializeField] private float[] landingVOLUME;

    private void Start()
    {
        //Debug.Log(materialInicial);
        SetGroundMaterial(materialInicial);
        //Debug.Log(currentMaterialId);
    }
    public void PlayFootsteps()
    {
        if (footstepsPlaying) return;
        footstepsPlaying = true;
        footstepsSource.volume = footStepsVOLUME[currentMaterialId];
        footstepsSource.Play();
    }

    public void StoptFootsteps()
    {
        if (!footstepsPlaying) return;
        footstepsPlaying = false;
        footstepsSource.volume = 0;
        footstepsSource.Stop();
    }

    public void SetGroundMaterial(int material)
    {
        //Debug.Log("CAMBIAO A :" + material);
        footstepsSource.clip = footSteps[material];
        landingSource.clip = landingSounds[material];
        currentMaterialId = material;
    }

    public void PlayLanding()
    {
        landingSource.volume = landingVOLUME[currentMaterialId];
        landingSource.Play();
    }
}
