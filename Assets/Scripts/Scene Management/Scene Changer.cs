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
        PlayerInfo.Instance.CanDoubleJump = true;
        PlayerInfo.Instance.CanDash = true;

        PlayerInfo.Instance.OnSceneChange(player.GetComponent<HealthPlayerManager>().CurrentHealth, scene.sceneName, scene.EnterPositions[EnterPoint]);

        Instance.StartCoroutine(FadeOutThenChangeScene(scene.sceneName));

        //PANTALLA DE CARGA

    }

    public void ChangeSceneByDeath()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerInfo.Instance.CheckpointSceneName);
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
        Debug.Log("cargando escena");
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Hook>().DisableHookInput();
        player.GetComponent<HorizontalMovement>().DisableMovementInput();
        player.GetComponent<Dash>().DisableDashInput();
        player.GetComponent<Jump>().DisableJumpInput();

        Instance.StartCoroutine(FadeInAndGainControl());
    }

    private IEnumerator FadeInAndGainControl()
    {
        yield return new WaitForSeconds(0.75f);
        Debug.Log("FADE IN");
        FadeInOut.instance.StartFadeIn();

        while (FadeInOut.instance.IsFadingIn)
        {
            yield return null;
        }

        Debug.Log("CONTROLAS");
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Hook>().EnableHookInput();
        player.GetComponent<HorizontalMovement>().EnableMovementInput();
        player.GetComponent<Dash>().EnableDashInput();
        player.GetComponent<Jump>().EnableJumpInput();
    }



}
