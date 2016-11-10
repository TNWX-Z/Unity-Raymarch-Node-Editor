using UnityEngine;
using System.Collections;
using UnityEditor;

public class RenderNode : BaseInput
{
    private Rect[] selfRectAll;
    private DataPool[] data_outAll;
    private DataPool[] data_inAll;

    void initValues()
    {
        selfRectAll = new Rect[0];
        data_outAll = new DataPool[0];
        data_inAll = new DataPool[0];
    }
    public RenderNode() : base()
    {
        windowTitle = "Render Node";
        windowRect = new Rect(20, 20, 100, 100);
        initValues();
    }
    public override void DrawWindow()
    {
        windowRect = GUI.Window(ID, windowRect, WindowsContent, windowTitle);
    }

    #region Do really time calculate
    void DoInterfacePos()
    {
        for (int i = 0; i < data_outAll.Length; i++)
        {
            data_outAll[i].otherWindowsPos = windowRect;
        }
    }
    void DoInterfaceIsAlive()
    {
        for (int i = 0; i < data_inAll.Length; i++)
        {
            if (-data_inAll[i])
            {
                data_inAll[i] = null;
            }
        }
    }
    #endregion

    void GetRect(ref Rect _rect)
    {
        if (Event.current.type == EventType.Repaint)
            _rect = GUILayoutUtility.GetLastRect();
    }
    public override void WindowsContent(int _ID)
    {
        Event e = Event.current;
        //---------Node Window Context
        EditorGUI.LabelField(new Rect(10,30,50,15),"Render");

        //---------Node Window Context

        #region Something
        if (CanDragWindow)
        {
            GUI.DragWindow();
        }
        if (e.type == EventType.Repaint)
        {
            //for really time calculate position of the interface
            DoInterfacePos();
            //for really time calculate alive of the interface
            DoInterfaceIsAlive();
        }
        #endregion
    }

    #region Base Life Function
    public override BaseInput IsMouseAtWindow(Vector2 _mousePos)
    {
        return base.IsMouseAtWindow(_mousePos);
    }

    public override bool SetInput(DataPool _data)
    {
        bool temp = false;
        for (int i = 0; i < selfRectAll.Length; i++)
        {
            if (selfRectAll[i].Contains(LocalMouse))
            {
                data_inAll[i] = _data;
                temp = true;
                break;
            }
        }
        return temp;
    }

    public override bool Picked(out DataPool CurvePos)
    {
        CurvePos = null;
        bool temp = false;
        for (int i = 0; i < data_outAll.Length; i++)
        {
            if (data_outAll[i].otherRect.Contains(LocalMouse))
            {
                CurvePos = data_outAll[i];
                temp = true;
                break;
            }
        }
        return temp;
    }

    public override void DrawCurve()
    {
        for (int i = 0; i < data_inAll.Length; i++)
        {
            if (data_inAll[i])
            {
                data_inAll[i].SetSelfPos(ref windowRect, ref selfRectAll[i]);
                NodeEditor.DrawCurve(data_inAll[i].otherPos, data_inAll[i].selfPos);
            }
        }
    }

    public override void Delected()
    {
        for (int i = 0; i < data_outAll.Length; i++)
        {
            data_outAll[i].Dead();
            data_outAll[i] = null;
        }
        data_outAll = null;

        for (int i = 0; i < data_inAll.Length; i++)
        {
            data_inAll[i] = null;
        }
        data_inAll = null;
    }
    #endregion
}
