using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CampoDeVisionCamara : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        var localCamara = GetComponent<Camera>();
        localCamara.stereoTargetEye = StereoTargetEyeMask.None;
    }    
}
