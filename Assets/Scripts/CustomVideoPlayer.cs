using UnityEngine;
using UnityEngine.Video;

public class CustomVideoPlayer : MonoBehaviour
{
    [SerializeField] private string videoFileName;

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayerComponent = GetComponent<VideoPlayer>();

        if (videoPlayerComponent)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayerComponent.url = videoPath;
            videoPlayerComponent.Play();
        }
    }
}