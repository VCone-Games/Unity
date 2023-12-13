using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cameraToChange;
    [SerializeField] private CinemachineVirtualCamera defaultCamera;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraManager.Instance.SwapCameras(cameraToChange);
            Debug.Log("CAMBIANDO DE CAMARA");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraManager.Instance.SwapCameras(defaultCamera);
            Debug.Log("VOLVIENDO A LA CAMARA ORIGINAL");
        }

    }
}
