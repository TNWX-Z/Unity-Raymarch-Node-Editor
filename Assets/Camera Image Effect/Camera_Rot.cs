using UnityEngine;
using System.Collections;

public class Camera_Rot : MonoBehaviour {

    [SerializeField]
    private float m_MoveSpeed = 10f;
    [SerializeField]
    private float m_RotSpeed = 180f;

    private float Hori_speed;
    private float Vert_speed;
    private float MouseX;
    private float MouseY;

    void Start ()
    {
    }

    private void MoveSelf()
    {
        Hori_speed = Input.GetAxis("Horizontal") * m_MoveSpeed * Time.deltaTime;
        Vert_speed = Input.GetAxis("Vertical") * m_MoveSpeed * Time.deltaTime;
        transform.Translate(Hori_speed * Vector3.right + Vert_speed * Vector3.forward, Space.Self);
    }
    private void RotSelf()
    {
        MouseX = Input.GetAxis("Mouse X") * m_RotSpeed * Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y") * m_RotSpeed * Time.deltaTime;
        transform.Rotate(0, MouseX, 0, Space.World);
        transform.Rotate(-MouseY, 0, 0, Space.Self);
    }
	
	void Update () {
        MoveSelf();
        RotSelf();
	}
}
