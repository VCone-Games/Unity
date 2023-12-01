using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager: MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip[] EnviromentalAudioClips;
    [Tooltip("0 - FIRST PHASE, 1 - FIRST PHASE LOOP, 2 - INTERLUDE," +
        "3 - SECOND PHASE, 4 SECOND PHASE LOOP")]
    [SerializeField] private AudioClip[] AzafranAudioClips;
    private bool phirstPhaseAzafran = true;
    public bool PhirstPhaseAzafran { get { return phirstPhaseAzafran; } set { phirstPhaseAzafran = value; } }

    public int actualClip;

    private void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}

		if (SceneManager.GetActiveScene().name == "Azafran")
		{
			Debug.Log("Estoy en azafran, MUSICA.");
			musicSource.loop = false;
			musicSource.clip = AzafranAudioClips[0];
			musicSource.Play();
			return;
		}
		
        musicSource.clip = EnviromentalAudioClips[actualClip];
        musicSource.Play();
    }

    public void ChangeMusic(int clipId)
    {
        if (SceneManager.GetActiveScene().name == "Azafran")
        {
            Debug.Log("Estoy en azafran, MUSICA.");
			musicSource.loop = false;
			musicSource.clip = AzafranAudioClips[0];
			musicSource.Play();
		} else
        {
            musicSource.loop = true;
			actualClip = clipId;
			musicSource.clip = EnviromentalAudioClips[actualClip];
			musicSource.Play();
		}
    }

	public void Update()
	{
		if (SceneManager.GetActiveScene().name == "Azafran")
        {
            if(phirstPhaseAzafran && !musicSource.isPlaying)
            {
                Debug.Log("LOOP PRIMERA CANCION...");
                musicSource.clip = AzafranAudioClips[1];
				musicSource.Play();
			} else if (!phirstPhaseAzafran && !musicSource.isPlaying)
            {
                Debug.Log("LOOP SEGUNDA CANCION...");
                musicSource.clip = AzafranAudioClips[4];
				musicSource.Play();
			}
        }
	}

    public void PlayAzafranInterlude()
    {
        Debug.Log("INTERLUDE AZAFRAN");
        musicSource.clip = AzafranAudioClips[2];
        musicSource.Play();
    }

    public void PlayAzafranSecondPhase()
    {
        Debug.Log("SECOND PHASE AZAFRAN MUSIC");
        musicSource.clip = AzafranAudioClips[3];
		musicSource.Play();
	}
	public void ChangeFootstepsClip(int stepsId)
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerSoundManager>().SetGroundMaterial(stepsId);
    }
}
