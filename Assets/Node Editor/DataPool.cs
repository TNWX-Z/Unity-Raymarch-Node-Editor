using UnityEngine;
using System.Collections;

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System;
using System.Text;

public class DataPool
{
    private bool isAlive = true;
    public BaseInput otherInputNode;
    public Rect otherWindowsPos;
    public Rect otherRect;
    private Vector2 _otherPos = Vector2.zero;
    public Vector2 otherPos
    {
        get
        {
            _otherPos.x = otherWindowsPos.x + otherRect.x + otherRect.width + 5;
            _otherPos.y = otherWindowsPos.y + otherRect.y + otherRect.height / 2;
            return _otherPos;
        }
    }
    public Vector2 selfPos = Vector2.zero;

    public bool IsLink = true;

    public NodeType nodeType = NodeType.None;
    public enum NodeType
    {
        GemetryNode,
        RenderNode,
        MorphNode,
        ShapeNode,
        None
    }

    public MorphNode rootMorph;

    public String Shape_or_Morph_context;

    #region Self Function
    public void SetSelfPos(ref Rect _selfWindowsPos,ref Rect _selfNodeInterfacePos)
    {
        selfPos.x = _selfWindowsPos.x + _selfNodeInterfacePos.x;
        selfPos.y = _selfWindowsPos.y + _selfNodeInterfacePos.y + _selfNodeInterfacePos.height / 2;
    }
    public void Dead()
    {
        isAlive = false;
    }
    public static implicit operator bool(DataPool me)
    {
        return me != null;
    }
    public static bool operator -(DataPool self)
    {
        return self != null && !self.isAlive;
    }
    #endregion
}
