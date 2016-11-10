using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;

public class ResultGroupNode : BaseInput
{
    private Rect[] selfRectAll;
    private DataPool[] data_outAll;
    private DataPool[] data_inAll;

    public string resultContext = "";
    private int GroupID = 0;
    private int MaterialID = 0;

    void initValues()
    {
        selfRectAll = new Rect[1];
        data_outAll = new DataPool[0];
        data_inAll = new DataPool[1];
    }

    public ResultGroupNode() : base()
    {
        windowTitle = "Distance Group Node";
        windowRect = new Rect(10, 10, 130, 65);
        initValues();
    }

    public override void DrawWindow()
    {
        windowRect = GUI.Window(ID, windowRect, WindowsContent, windowTitle);

        Rect temp = new Rect(windowRect.x - 20, windowRect.y + windowRect.height / 3, 20, 20);
        GUI.DrawTexture(temp, NodeEditor.tex_interface1);
        selfRectAll[0] = new Rect(0, temp.y - windowRect.y, 20, 20);

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
        if (GroupID == 0)
        {
            GroupID = CustomFormat.GetGroupID;
        }
        EditorGUILayout.LabelField("Distance Group: " + GroupID);
        MaterialID = CustomFormat.MyLabelIntField("Material ID: ", MaterialID,new Rect(10, 40, 70, 17));
        if (data_inAll[0] && (data_inAll[0].nodeType == DataPool.NodeType.MorphNode || data_inAll[0].nodeType == DataPool.NodeType.ShapeNode))
        {
            string tmp = data_inAll[0].Shape_or_Morph_context;
            if (tmp != "")
                resultContext = "d=Min(d," + tmp + "," + MaterialID + ");\n";
        }
        //---------Node Window Context
        #region Something
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
        #endregion
    }

    public string GetRootMorphContext()
    {
        if (data_inAll[0] && data_inAll[0].rootMorph)
            return data_inAll[0].rootMorph.GetRootMorphContext();
        else
            return "";
    }

    #region Base Life Function
    public override BaseInput IsMouseAtWindow(Vector2 _mousePos)
    {
        return base.IsMouseAtWindow(_mousePos);
    }
    public override bool SetInput(DataPool _data)
    {
        if (_data && !(_data.nodeType == DataPool.NodeType.ShapeNode || _data.nodeType == DataPool.NodeType.MorphNode))
            return false;
        bool temp = false;
        for (int i = 0; i < selfRectAll.Length; i++)
        {
            if (selfRectAll[i].Contains(LocalMouse))
            {
                if (_data)
                {
                    _data.IsLink = false;
                    if (data_inAll[i] && _data != data_inAll[i])
                        data_inAll[i].IsLink = true;
                    data_inAll[i] = _data;
                }
                else
                {
                    if (data_inAll[i])
                        data_inAll[i].IsLink = true;
                    data_inAll[i] = _data;
                }
                temp = true;
                break;
            }
        }
        return temp;
    }
    public override bool Picked(out DataPool dataPool)
    {
        dataPool = null;
        bool temp = false;
        for (int i = 0; i < data_outAll.Length; i++)
        {
            if (data_outAll[i].otherRect.Contains(LocalMouse))
            {
                dataPool = data_outAll[i];
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
