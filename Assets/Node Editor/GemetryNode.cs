using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GemetryNode : BaseInput
{
    private Rect[] selfRectAll;
    private DataPool[] data_outAll;
    private DataPool[] data_inAll;

    private static Vector2 offset = new Vector2(20,0);

    void initValues()
    {
        selfRectAll = new Rect[3];
        data_outAll = new DataPool[3]{
            new DataPool(),
            new DataPool(),
            new DataPool()
        };
        data_inAll = new DataPool[3];
    }
    public GemetryNode() : base()
    {
        windowTitle = "Input Node";
        windowRect = new Rect(10,10,200,200);
        initValues();
    }
    public override void DrawWindow()
    {
        windowRect = GUI.Window(ID,windowRect,WindowsContent,windowTitle);

        GUI.DrawTexture(new Rect(windowRect.x - offset.x, windowRect.y + 18, 20, 20), NodeEditor.tex_interface1);
        GUI.DrawTexture(new Rect(windowRect.x - offset.x, windowRect.y + 55, 20, 20), NodeEditor.tex_interface1);
        GUI.DrawTexture(new Rect(windowRect.x - offset.x, windowRect.y + 90, 20, 20), NodeEditor.tex_interface1);

        GUI.DrawTexture(new Rect(windowRect.x + windowRect.width, windowRect.y + 38, 20, 20), NodeEditor.tex_interface1);
        GUI.DrawTexture(new Rect(windowRect.x + windowRect.width, windowRect.y + 73, 20, 20), NodeEditor.tex_interface1);
        GUI.DrawTexture(new Rect(windowRect.x + windowRect.width, windowRect.y + 110, 20, 20), NodeEditor.tex_interface1);
        
    }
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

    void GetRect(ref Rect _rect)
    {
        if (Event.current.type == EventType.Repaint)
            _rect = GUILayoutUtility.GetLastRect();
    }

    public override void WindowsContent(int _ID)
    {
        Event e = Event.current;
        //---------Node Window Context
        GUILayout.Label("In 1: ",GUILayout.Width(50));
        GetRect(ref selfRectAll[0]);
        GUILayout.Label("\t\t  Out 1 ->");
        GetRect(ref data_outAll[0].otherRect);

        GUILayout.Label("In 2: ",GUILayout.Width(50));
        GetRect(ref selfRectAll[1]);
        GUILayout.Label("\t\t  Out 2 ->");
        GetRect(ref data_outAll[1].otherRect);

        GUILayout.Label("In 3: ",GUILayout.Width(50));
        GetRect(ref selfRectAll[2]);
        GUILayout.Label("\t\t  Out 3 ->");
        GetRect(ref data_outAll[2].otherRect);
        //---------Node Window Context
        if (CanDragWindow)
        {
            GUI.DragWindow();
        }
        if (e.type == EventType.repaint)
        {
            //for really time calculate position of the interface
            DoInterfacePos();
            //for really time calculate alive of the interface
            DoInterfaceIsAlive();
        }

    }

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
                data_inAll[i].SetSelfPos(ref windowRect,ref selfRectAll[i]);
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
}
