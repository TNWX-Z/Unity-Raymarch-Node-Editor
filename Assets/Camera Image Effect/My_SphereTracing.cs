
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class My_SphereTracing : MonoBehaviour {

    [SerializeField]
    private Material my_mat;
    private Camera cam;
    private Mesh my_mesh;

    private CommandBuffer g_before_command;

    void Awake()
    {
    }

    private void CreatMesh()
    {
        cam = this.gameObject.GetComponent <Camera> ();
        if (my_mesh == null)
        {
            my_mesh = new Mesh();
            my_mesh.vertices = new[] {
                new Vector3(-1,-1),new Vector3(1,-1),
                Vector3.one,new Vector3(-1,1)
            };
            my_mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        }
    }

    void Start () {
        CreatMesh();
        if(my_mat == null)
            my_mat = new Material(Shader.Find("My/SphereTracing"));
    }

    void OnPreCull()
    {
        if (g_before_command == null)
        {
            //int MyGeometryBuffer = Shader.PropertyToID("MyG_Buffer");
            g_before_command = new CommandBuffer();
            g_before_command.name = "My Raymarching";
            g_before_command.DrawMesh(my_mesh, Matrix4x4.identity, my_mat, 0, 0);
            cam.AddCommandBuffer(CameraEvent.BeforeGBuffer, g_before_command);
        }
    }

    private void RelaseAllBuffer()
    {
        if (cam != null)
        {
            if (g_before_command != null)
            {
                cam.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, g_before_command);
                g_before_command.Release();
                g_before_command = null;
            }
        }
    }

    void OnDisable()
    {
        RelaseAllBuffer();
    }

}
