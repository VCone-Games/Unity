using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] public CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallYPanTime = 0.35f;
    public float fallSpeedDampingChangeThreshhold = -15f;

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    private CinemachineFramingTransposer framingTransposer;
    public CinemachineVirtualCamera currentCamera;

    private float normYPanAmount;

    private Vector2 startingTrackedObjectOffset;


    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    public CinemachineVirtualCamera CurrentCamera { get { return currentCamera; } }

    private void Awake()
    {

        if (Instance == null)
            Instance = this;

        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];

                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        normYPanAmount = framingTransposer.m_YDamping;

        normYPanAmount = framingTransposer.m_YDamping;

        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;

    }

    public void DisableCamera()
    {
        currentCamera.enabled = false;
    }

    public void EnableCamera()
    {
        currentCamera.enabled = true;
    }

    #region Lerp the Y Damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = framingTransposer.m_YDamping;

        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;

            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;
 

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    #endregion

    #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, Vector2 panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, Vector2 panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            endPos = panDirection.normalized;

            endPos *= panDistance;

            startingPos = startingTrackedObjectOffset;

            endPos += startingPos;

        }
        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }

    }


    #endregion

    #region Swap Cameras

    public void SwapCameraHorizontal(CinemachineVirtualCamera cameraFromLeft_Top, CinemachineVirtualCamera cameraFromRight_Bottom, Vector2 triggerExitDirection)
    {
        if (currentCamera == cameraFromLeft_Top && triggerExitDirection.x > 0f)
        {
            cameraFromRight_Bottom.enabled = true;

            cameraFromLeft_Top.enabled = false;

            currentCamera = cameraFromRight_Bottom;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if (currentCamera == cameraFromRight_Bottom && triggerExitDirection.x < 0f)
        {
            cameraFromLeft_Top.enabled = true;

            cameraFromRight_Bottom.enabled = false;

            currentCamera = cameraFromLeft_Top;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void SwapCameraVertical(CinemachineVirtualCamera cameraFromLeft_Top, CinemachineVirtualCamera cameraFromRight_Bottom, Vector2 triggerExitDirection)
    {
        if (currentCamera == cameraFromLeft_Top && triggerExitDirection.y < 0f)
        {
            cameraFromRight_Bottom.enabled = true;

            cameraFromLeft_Top.enabled = false;

            currentCamera = cameraFromRight_Bottom;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if (currentCamera == cameraFromRight_Bottom && triggerExitDirection.y > 0f)
        {
            cameraFromLeft_Top.enabled = true;

            cameraFromRight_Bottom.enabled = false;

            currentCamera = cameraFromLeft_Top;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #endregion
}
