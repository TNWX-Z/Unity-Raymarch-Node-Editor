using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;

public class MorphNode : BaseInput
{
    private Rect[] selfRectAll;
    private DataPool[] data_outAll;
    private DataPool[] data_inAll;

    public string ModelContext = "";
    internal int morphID = 0;
    private Transform selfTransform;
    private Vector3 positionXYZ = Vector3.zero;
    private Vector3 rotationXYZ = Vector3.zero;
    private Vector3 sizeXYZ = Vector3.one;

    private MorphType morphType = MorphType.融合;
    public enum MorphType
    {
        融合,
        相交,
        相差,
    }

    void initValues()
    {
        selfRectAll = new Rect[2];
        data_outAll = new[]{
            new DataPool(),
        };
        data_inAll = new DataPool[2];
    }

    public MorphNode() : base()
    {
        windowTitle = "Morph Node";
        windowRect = new Rect(10, 10, 120,150);
        initValues();
        data_outAll[0].nodeType = DataPool.NodeType.MorphNode;
        data_outAll[0].rootMorph = this;
    }

    public override void DrawWindow()
    {
        windowRect = GUI.Window(ID, windowRect, WindowsContent, windowTitle);

        Rect temp = new Rect(windowRect.x + windowRect.width, windowRect.y + windowRect.height / 2 - 10, 20, 20);
        GUI.DrawTexture(temp, NodeEditor.tex_interface1);
        data_outAll[0].otherRect = new Rect(windowRect.width - 20, windowRect.height / 2 - 10, 20, 20);

        temp = new Rect(windowRect.x-20,windowRect.y + windowRect.height/4,20,20);
        GUI.DrawTexture(temp, NodeEditor.tex_interface1);
        selfRectAll[0] = new Rect(0,temp.y - windowRect.y,20,20);

        temp = new Rect(windowRect.x - 20, windowRect.y + windowRect.height / 2, 20, 20);
        GUI.DrawTexture(temp, NodeEditor.tex_interface1);
        selfRectAll[1] = new Rect(0, temp.y - windowRect.y, 20, 20);

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
        morphType = (MorphType)EditorGUI.EnumPopup(new Rect(4, 20, 110, 25), morphType, CustomFormat.enumStyle);
        if (morphID == 0)
        {
            morphID = CustomFormat.GetMorphID;
         //   data_outAll[0].Shape_or_Morph_context = morphID;
        }
        if ((data_inAll[0] && data_inAll[0].Shape_or_Morph_context != "") || (data_inAll[1] && data_inAll[1].Shape_or_Morph_context != ""))
            data_outAll[0].Shape_or_Morph_context = "d"+morphID;
        else
            data_outAll[0].Shape_or_Morph_context = "";
        EditorGUI.LabelField(new Rect(windowRect.width-30, windowRect.height/2-8, windowRect.width, 20), "d"+morphID);
        SelectFunction(morphType);
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

    private void SelectFunction(MorphType _morphType)
    {
        switch (_morphType)
        {
            case MorphType.融合:
                ModelContext = CustomFormat.MorphContextFormat(this, "min", data_inAll);
                break;
            case MorphType.相交:
                ModelContext = CustomFormat.MorphContextFormat(this, "max", data_inAll);
                break;
            case MorphType.相差:
                ModelContext = CustomFormat.MorphContextFormat(this, "Subtract", data_inAll);
                break;
        }
    }

    public string GetRootMorphContext()
    {
        StringBuilder temp = new StringBuilder();
        foreach (DataPool it in data_inAll)
        {
            if (it && it.rootMorph)
            {
                temp.Append(it.rootMorph.GetRootMorphContext());
            }
        }
        temp.Append(ModelContext);
        return temp.ToString();
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
                if (_data )
                {
                    _data.IsLink = false;
                    if (data_inAll[i] && _data != data_inAll[i])
                        data_inAll[i].IsLink = true;
                    data_inAll[i] = _data;
                }else
                {
                    data_inAll[i].IsLink = true;
                    data_inAll[i] = _data;
                }
                temp = true;
                break;
            }
        }
        return temp;
    }
    public override bool Picked(out DataPool CurvePos)
    {
        CurvePos = null;
        if (!data_outAll[0].IsLink)
            return false;
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
