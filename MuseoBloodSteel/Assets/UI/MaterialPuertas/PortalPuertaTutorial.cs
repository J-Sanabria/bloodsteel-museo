using UnityEngine;

[ExecuteAlways]
public class CameraToRenderTexture : MonoBehaviour
{
    public Camera portalCamera;
    public RenderTexture targetTexture;

    void Update()
    {
        if (portalCamera && targetTexture)
            portalCamera.targetTexture = targetTexture;
    }
}
 