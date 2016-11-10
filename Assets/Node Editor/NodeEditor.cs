using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

public class NodeEditor : EditorWindow {
    public static Texture tex_backGround;
    private static Texture2D tex_tmp_backGroud;
    public static Texture2D tex_interface1;
    private Event e;
    public enum NodeType
    {
        RenderNode,
        GemetryNode,
        ShapeNode,
        MorphNode,
        Result
    }
    public enum InsideType
    {
        Make_Link,
        Smart_Link,
        Remove
    }

    private BaseInput MouseAtThisWindow_1 = null;
    private BaseInput MouseAtThisWindow_2 = null;

    private Vector2 mousePos = Vector2.zero;
    private DataPool NodeData;

    private Rect rect_ReallyTimeView_Toggle;

    private bool IsReallyTimeView = false;
    private bool IsPickingNodeInterface = false;

    private GenericMenu mainMenu;
    private GenericMenu removeMenu;
    private GenericMenu linkMenu;

    private List<BaseInput> windowsNode;

    [MenuItem("NodeEditor/MyRayMarching")]
    static void ShowEditor()
    {
        EditorWindow.GetWindow(typeof(NodeEditor));
    }
    void initAll()
    {
        rect_ReallyTimeView_Toggle = new Rect(10, 10, 120, 30);
        windowsNode = new List<BaseInput>();
        tex_tmp_backGroud = Resources.Load<Texture2D>("image");
        tex_interface1 = Resources.Load<Texture2D>("interface1");

        mainMenu = new GenericMenu();
        mainMenu.AddItem(new GUIContent("Add Render Node"), false, AddWindow, NodeType.RenderNode);
        mainMenu.AddItem(new GUIContent("Add Gemetry Node"), false, AddWindow, NodeType.GemetryNode);
        mainMenu.AddSeparator("");
        mainMenu.AddItem(new GUIContent("Model System/Add Shape Node"), false, AddWindow, NodeType.ShapeNode);
        mainMenu.AddItem(new GUIContent("Model System/Add Morph Node"), false, AddWindow, NodeType.MorphNode);
        mainMenu.AddItem(new GUIContent("Model System/Add Distance Group Node"), false, AddWindow, NodeType.Result);

        removeMenu = new GenericMenu();
        removeMenu.AddItem(new GUIContent("Remove Node"), false, InsideMenu, InsideType.Remove);

        linkMenu = new GenericMenu();
        linkMenu.AddItem(new GUIContent("Make Link"), false, InsideMenu, InsideType.Make_Link);
        linkMenu.AddItem(new GUIContent("Smart Link"), false, InsideMenu, InsideType.Smart_Link);
    }
    void OnEnable()
    {
        initAll();
    }
    private void BackGround()
    {
        GUILayout.BeginHorizontal();
        if (tex_backGround == null)
            tex_backGround = tex_tmp_backGroud;
        GUI.DrawTexture(GUILayoutUtility.GetRect(position.width, position.height), tex_backGround, ScaleMode.StretchToFill, false);
        GUILayout.EndHorizontal();
    }
    private void Draw()
    {
        BeginWindows();
        for (int i = 0; i < windowsNode.Count; i++)
        {
            windowsNode[i].DrawWindow();
        }
        EndWindows();
    }

    void OnGUI()
    {
        BackGround();
        e = Event.current;
        mousePos = e.mousePosition;
        
        if (IsReallyTimeView = GUI.Toggle(rect_ReallyTimeView_Toggle, IsReallyTimeView, "Really Time Rend"))
            Repaint();

        if (GUI.RepeatButton(new Rect(position.width-200,20,150,30),"Update Code Now!") && !IsPickingNodeInterface)
        {
            StringBuilder modelContext = new StringBuilder();
            for (int i = 0; i < windowsNode.Count; i++)
            {
                ResultGroupNode result = windowsNode[i] as ResultGroupNode;
                if (result)
                    modelContext.Append(result.GetRootMorphContext());
            }
            for (int j = 0; j < windowsNode.Count; j++)
            {
                ResultGroupNode result = windowsNode[j] as ResultGroupNode;
                if (result)
                {
                    modelContext.Append(result.resultContext);
                    result.resultContext = "";
                }
            }
            AutoManager.CreateMyAsset(modelContext.ToString());
        }

        if (e.button == 1 && !IsPickingNodeInterface)
        {
            MouseAtThisWindow_1 = null;
            bool IsShowMainMenu = true;

            for (int i = 0; i < windowsNode.Count; i++)
            {
                BaseInput tmp_MouseAtWindow = windowsNode[i].IsMouseAtWindow(e.mousePosition - new Vector2(20,0));
                if (tmp_MouseAtWindow)
                {
                    MouseAtThisWindow_1 = tmp_MouseAtWindow;
                    IsPickingNodeInterface = false;
                    IsShowMainMenu = false;

                    if (tmp_MouseAtWindow.Picked(out NodeData))
                        linkMenu.ShowMenu(position);
                    else
                        removeMenu.ShowMenu(position);
                }
            }
            if (IsShowMainMenu)
                mainMenu.ShowMenu(position);
        }
        else if (IsPickingNodeInterface)
        {
            DrawCurve(NodeData.otherPos,mousePos);
            if (e.button == 0 && e.type == EventType.MouseDown)
            {
                if (!MouseAtThisWindow_1.IsMouseAtWindow(mousePos))
                    IsPickingNodeInterface = false;
                for (int i = 0; i < windowsNode.Count; i++)
                {
                    BaseInput tmp_MouseAtWindow = windowsNode[i].IsMouseAtWindow(e.mousePosition + new Vector2(20, 0));
                    if (tmp_MouseAtWindow)
                    {
                        MouseAtThisWindow_2 = tmp_MouseAtWindow;
                        if (MouseAtThisWindow_2 != MouseAtThisWindow_1)
                        {
                            NodeData.otherInputNode = MouseAtThisWindow_1;
                            IsPickingNodeInterface = !MouseAtThisWindow_2.SetInput(NodeData);
                        }
                    }
                }
            }
            Repaint();
        }
        else if (!IsPickingNodeInterface && e.type == EventType.MouseDown)
        {
            Repaint();
            for (int i = 0; i < windowsNode.Count; i++)
            {
                BaseInput tmp_MouseAtWindow = windowsNode[i].IsMouseAtWindow(e.mousePosition + new Vector2(20, 0));
                if (tmp_MouseAtWindow)
                    tmp_MouseAtWindow.SetInput(null);
            }
        }

        Draw();

        for (int i = 0; i < windowsNode.Count; i++)
            windowsNode[i].DrawCurve();

    }

    void AddWindow(object obj)
    {
        BaseInput temp = null;
        NodeType node = (NodeType)obj;
        switch (node)
        {
            case NodeType.RenderNode:
                temp = ScriptableObject.CreateInstance<RenderNode>();
                break;
            case NodeType.GemetryNode:
                temp = ScriptableObject.CreateInstance<GemetryNode>();
                break;
            case NodeType.ShapeNode:
                temp = ScriptableObject.CreateInstance<ShapeNode>();
                break;
            case NodeType.MorphNode:
                temp = ScriptableObject.CreateInstance<MorphNode>();
                break;
            case NodeType.Result:
                temp = ScriptableObject.CreateInstance<ResultGroupNode>();
                break;
        }
        temp.windowRect.position = mousePos;
        windowsNode.Add(temp);
    }

    void InsideMenu(object obj)
    {
        InsideType temp = (InsideType)obj;
        switch (temp)
        {
            case InsideType.Remove:
                MouseAtThisWindow_1.Delected();
                windowsNode.Remove(MouseAtThisWindow_1);
                MouseAtThisWindow_1 = null;
                break;
            case InsideType.Make_Link:
                IsPickingNodeInterface = true;
                break;
            case InsideType.Smart_Link:
                IsPickingNodeInterface = true;
                //------------
                break;
        }
    }

    public static void DrawCurve(Vector2 _start,Vector2 _end)
    {
        Vector2 tPos1 = new Vector2(_start.x + 70,_start.y);
        Vector2 tPos2 = new Vector2(_end.x - 70,_end.y);
        _start.x += 15;
        _end.x -= 20;
        Color glowCol = new Color(0.1f,0.7f,0.6f,0.5f);
        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(_start, _end, tPos1, tPos2, glowCol*3, null, i * 4 + 5);
        }
        Handles.DrawBezier(_start,_end,tPos1,tPos2,Color.red,null,7);
    }
}
