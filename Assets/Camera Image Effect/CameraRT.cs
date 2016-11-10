using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraRT : MonoBehaviour {

    private Camera mainCam;
    private RenderTexture rt_Camera;
    public Material mat;

	void Start () {
        mainCam = GetComponent<Camera>();
        rt_Camera = new RenderTexture(mainCam.pixelWidth,mainCam.pixelHeight,16);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest);
        Graphics.Blit(src,rt_Camera,mat,-1);
        NodeEditor.tex_backGround = rt_Camera;
    }
}
