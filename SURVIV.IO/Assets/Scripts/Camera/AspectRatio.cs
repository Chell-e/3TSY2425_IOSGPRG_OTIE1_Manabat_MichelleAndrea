using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

public class AspectRatio : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;

    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        SetCameraAspect();
    }

    private void SetCameraAspect()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspectRatio;

        Rect rect = camera.rect;

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f; 
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight; 
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        camera.rect = rect;
    }
}
