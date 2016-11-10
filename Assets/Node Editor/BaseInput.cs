using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class BaseInput : ScriptableObject
{
    public int ID = 0;
    public bool isPicked = false;
    public string windowTitle = "None";
    public Rect windowRect;
    public Vector2 LocalMouse;
    public bool CanDragWindow = true;

    public BaseInput()
    {
        ID = GetInstanceID();
        windowTitle = "Base Window Node";
    }

    public virtual void DrawWindow() { }

    public virtual void WindowsContent(int _ID) { }

    public virtual BaseInput IsMouseAtWindow(Vector2 _mousePos)
    {
        if (!windowRect.Contains(_mousePos))
            return null;
        LocalMouse.x = _mousePos.x - windowRect.x;
        LocalMouse.y = _mousePos.y - windowRect.y;
        return this;
    }

    public virtual bool SetInput(DataPool _pos) { return false; }

    public virtual void DrawCurve() { }

    public virtual bool Picked(out DataPool CurvePos)
    {
        CurvePos = null;
        return false;
    }

    public virtual void Delected() { }
}
