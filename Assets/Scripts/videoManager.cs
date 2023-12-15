using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoManager : MonoBehaviour
{
    [SerializeField] VideoPlayer video;

    private void Start()
    {
        video.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("dale al play auronplay");
            video.Play();
        }
 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("dale al pause menopause");
            video.Pause();
        }
       
    }
}
