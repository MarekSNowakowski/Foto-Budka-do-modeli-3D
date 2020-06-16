 
using System.IO;
using UnityEngine;
 
public class CameraCapture : MonoBehaviour
{
    private Camera cam;
    private int i=1;
 
    void Start () {
        cam = GetComponent<Camera>();
    }
 
    public void Capture()
    {
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        cam.Render();
 
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
 
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
 
        File.WriteAllBytes("Assets/Out/photo" + i, bytes);
        i++;
    }
}