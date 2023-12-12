using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public int zoneId;
    public int sceneId;
    private bool dead;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        DontDestroyOnLoad(gameObject);
    }


    public void ChangeSceneByMoving(SceneObject scene, int EnterPoint)
    {
        Time.timeScale = 1;
        GameObject player = GameObject.FindWithTag("Player");
        HealthPlayerManager healthPlayer = player.GetComponent<HealthPlayerManager>();

        PlayerInfo.Instance.OnSceneChange(healthPlayer.CurrentHealth, healthPlayer.MaxHealth,
            scene.sceneName, scene.EnterPositions[EnterPoint], player.GetComponent<PlayerSoundManager>().currentMaterialId);

        zoneId = scene.zone;
        sceneId = SceneManager.GetSceneByName(scene.sceneName).buildIndex;

        Instance.StartCoroutine(FadeOutThenChangeScene(scene.sceneName));

        //PANTALLA DE CARGA

    }

    public void ChangeSceneByDeath()
    {
        Time.timeScale = 1;
        dead = true;
        Instance.StartCoroutine(FadeOutThenChangeScene(PlayerInfo.Instance.CheckpointSceneName));
    }

    private IEnumerator FadeOutThenChangeScene(string sceneName)
    {
        FadeInOut.instance.StartFadeOut();

        while (FadeInOut.instance.IsFadingOut)
        {
            yield return null;
        }


        SceneManager.LoadScene(sceneName);
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (dead)
        {
            MusicManager.Instance.ChangeMusic(PlayerInfo.Instance.sceneMusicId);
        }
        else if (zoneId == 0 || MusicManager.Instance.actualClip != zoneId)
        {
            MusicManager.Instance.ChangeMusic(zoneId);
        }



        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        player.GetComponent<Jump>().DisableBonusAirTime();
        player.GetComponent<Hook>().DisableHookInput();
        player.GetComponent<HorizontalMovement>().DisableMovementInput();
        player.GetComponent<Dash>().DisableDashInput();
        player.GetComponent<Jump>().DisableJumpInput();
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        Instance.StartCoroutine(FadeInAndGainControl());
    }

    private IEnumerator FadeInAndGainControl()
    {
        yield return new WaitForSeconds(0.75f);

        FadeInOut.instance.StartFadeIn();

        while (FadeInOut.instance.IsFadingIn)
        {
            yield return null;
        }


        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Hook>().EnableHookInput();
        player.GetComponent<HorizontalMovement>().EnableMovementInput();
        player.GetComponent<Dash>().EnableDashInput();
        player.GetComponent<Jump>().EnableJumpInput();
    }



}
