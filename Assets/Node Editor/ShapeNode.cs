using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;

public class ShapeNode : BaseInput
{
    private Rect[] selfRectAll;
    private DataPool[] data_outAll;
    private DataPool[] data_inAll;

    private string shapeContext = "";
    private Transform selfTransform;
    private Vector3 positionXYZ = Vector3.zero;
    private Vector3 rotationXYZ = Vector3.zero;
    private Vector3 sizeXYZ = Vector3.one;

    private ShapeType shapeType = ShapeType.圆球;
    public enum ShapeType
    {
        圆球,
        正方体,
        长方体,
        圆柱体,
        胶囊体,
        椎体,
        甜甜圈,
        //Prism,
        Line3D,
        平面,
        贝塞尔3D,
    }

    void initValues()
    {
        selfRectAll = new Rect[1];
        data_outAll = new []{
            new DataPool(),
        };
        data_inAll = new DataPool[1];
    }

    public ShapeNode() : base()
    {
        windowTitle = "Shape Node";
        windowRect = new Rect(10,10,120,120);
        initValues();
        data_outAll[0].nodeType = DataPool.NodeType.ShapeNode;
    }

    public override void DrawWindow()
    {
        windowRect = GUI.Window(ID, windowRect, WindowsContent, windowTitle);
        Rect temp = new Rect(windowRect.x + windowRect.width, windowRect.y + windowRect.height / 2 - 10, 20, 20);
        GUI.DrawTexture(temp, NodeEditor.tex_interface1);
        data_outAll[0].otherRect = new Rect(windowRect.width - 20, windowRect.height / 2 - 10, 20, 20);
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
        shapeType = (ShapeType)EditorGUI.EnumPopup(new Rect(10, 20, 90, 25), shapeType, CustomFormat.enumStyle);
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Object:");
        selfTransform = (Transform)EditorGUILayout.ObjectField(selfTransform,typeof(Transform),true);
        if (selfTransform)
        {
            positionXYZ = selfTransform.transform.position;
            rotationXYZ = selfTransform.transform.eulerAngles;
            sizeXYZ = selfTransform.transform.localScale;
        }
        else
        {
            rotationXYZ = Vector3.zero;
            positionXYZ = Vector3.one;
            sizeXYZ = Vector3.one;
        }
        SelectFunction(shapeType);
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

    private void SelectFunction(ShapeType _shapeType)
    {
        switch (_shapeType)
        {
            case ShapeType.圆球:
                shapeContext = CustomFormat.ShapeContextFormat("Sphere(p{0},{1})", positionXYZ, rotationXYZ, sizeXYZ.x);
                break;
            case ShapeType.正方体:
                shapeContext = CustomFormat.ShapeContextFormat("Cube(p{0},{1})", positionXYZ, rotationXYZ, sizeXYZ.x);
                break;
            case ShapeType.长方体:
                shapeContext = "";
                break;
            case ShapeType.圆柱体:
                shapeContext = "";
                break;
            case ShapeType.胶囊体:
                shapeContext = "";
                break;
            case ShapeType.椎体:
                shapeContext = "";
                break;
            case ShapeType.甜甜圈:
                shapeContext = "";
                break;
            case ShapeType.Line3D:
                shapeContext = "";
                break;
            case ShapeType.平面:
                shapeContext = "";
                break;
            case ShapeType.贝塞尔3D:
                shapeContext = "";
                break;
            default:
                break;
        }
        data_outAll[0].Shape_or_Morph_context = shapeContext;
    }


    #region Base Life Function
    public override BaseInput IsMouseAtWindow(Vector2 _mousePos)
    {
        return base.IsMouseAtWindow(_mousePos);
    }
    public override bool SetInput(DataPool _data)
    {
        if (_data && _data.nodeType != DataPool.NodeType.ShapeNode)
            return false;
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
