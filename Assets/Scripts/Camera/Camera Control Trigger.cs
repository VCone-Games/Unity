using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    [SerializeField] public bool swapCameras;
    [SerializeField] public bool verticalChange;
    [SerializeField] public bool panCameraOnContact;

    [SerializeField] public CinemachineVirtualCamera camera_On_Left_Top;
    [SerializeField] public CinemachineVirtualCamera camera_On_Right_Bottom;

    [SerializeField] public Vector2 panDirection;
    [SerializeField] public float panDistance = 3f;
    [SerializeField] public float panTime = 0.35f;

    private Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(panDistance, panTime, panDirection, false);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(panDistance, panTime, panDirection, true);
            }

            Vector2 exitDirection = (collision.transform.position - coll.bounds.center).normalized;

            if (swapCameras && !verticalChange && camera_On_Left_Top != null && camera_On_Right_Bottom != null)
            {
                CameraManager.Instance.SwapCameraHorizontal(camera_On_Left_Top, camera_On_Right_Bottom, exitDirection);
            }

            else if (swapCameras && verticalChange && camera_On_Left_Top != null && camera_On_Right_Bottom != null)
            {
                CameraManager.Instance.SwapCameraVertical(camera_On_Left_Top, camera_On_Right_Bottom, exitDirection);
            }
        }
    }
}
