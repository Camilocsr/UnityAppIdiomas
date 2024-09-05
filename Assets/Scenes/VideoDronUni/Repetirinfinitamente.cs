using UnityEngine;
using UnityEngine.Video;

public class Repetirinfinitamente : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    void Start()
    {
        
        if (videoPlayer == null)
        {
            Debug.LogError("No se ha asignado un VideoPlayer. Asigna uno desde el Inspector.");
            return;
        }

        videoPlayer.loopPointReached += LoopVideo;
    }

    void LoopVideo(VideoPlayer vp)
    {
        videoPlayer.Play();
    }
}