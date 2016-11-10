using UnityEngine;
using System.Collections;
using System.Text;
using UnityEditor;

public static class CustomFormat{

    private static int morphID = 0;
    public static int GetMorphID
    {
        get {
            return ++morphID;
        }
    }
    private static int groupID = 0;
    public static int GetGroupID
    {
        get
        {
            return ++groupID;
        }
    }

    public static GUIStyle enumStyle = new GUIStyle(GUI.skin.box) {
        stretchHeight = true,stretchWidth = true,
        fontSize = 13,fontStyle = FontStyle.Bold,
    };

    public static GUIStyle windowStyle = new GUIStyle(GUI.skin.window)
    {
        fontSize = 18,
        fontStyle = FontStyle.Bold,
    };

    public static void ShowMenu(this GenericMenu menu,Rect region)
    {
        if (region.Contains(Event.current.mousePosition + region.position))
            menu.ShowAsContext();
    }

    public static string vec3ToString(this Vector3 vec3)
    {
        return "float3("+ vec3.x+","+vec3.y+","+vec3.z+")";
    }

    public static int MyLabelIntField(string context,int value,Rect region,int count = 2)
    {
        EditorGUI.LabelField(new Rect(region.x, region.y, context.Length*6, region.height), context);
        return EditorGUI.IntField(new Rect(region.x + context.Length * 6, region.y, count * 15, region.height), value);
    }

    public static string ShapeContextFormat(string context, Vector3 _position,Vector3 _angle, float r)
    {
        StringBuilder result = new StringBuilder();
        r = Mathf.Abs(r);

        if (_position == Vector3.zero)
        {
            result.AppendFormat(context, "", r);
            return result.ToString();
        }
        result.AppendFormat(context, "-"+_position.vec3ToString(), r);
        return result.ToString();
    }
    public static string ShapeContextFormat(string context, Vector3 _position, Vector3 _angle, Vector3 _scale)
    {
        StringBuilder result = new StringBuilder();
        _scale.x = Mathf.Abs(_scale.x);
        _scale.y = Mathf.Abs(_scale.y);
        _scale.z = Mathf.Abs(_scale.z);

        if (_position == Vector3.zero)
        {
            result.AppendFormat(context, "", _scale);
            return result.ToString();
        }
        result.AppendFormat(context, "-"+_position.vec3ToString(), _scale);
        return result.ToString();
    }

    public static string MorphContextFormat(MorphNode me,string _format,params DataPool[] dataPools)
    {
        StringBuilder result = new StringBuilder();
        if (dataPools[0] && dataPools[1])
            if (dataPools[0].Shape_or_Morph_context != "" && dataPools[1].Shape_or_Morph_context != "")
                result.AppendFormat("float {0}={1}({2},{3});\n", "d" + me.morphID, _format, dataPools[0].Shape_or_Morph_context, dataPools[1].Shape_or_Morph_context);
            else if (dataPools[0].Shape_or_Morph_context != "" && dataPools[1].Shape_or_Morph_context == "")
                result.Append(MorphContextFormat(me, _format, dataPools[0], null));
            else if (dataPools[0].Shape_or_Morph_context == "" && dataPools[1].Shape_or_Morph_context != "")
                result.Append(MorphContextFormat(me, _format, null, dataPools[1]));
            else { }
        else
            foreach (DataPool it in dataPools)
                if (it && it.Shape_or_Morph_context != "")
                    result.AppendFormat("float {0}={1};\n", "d" + me.morphID, it.Shape_or_Morph_context);
        return result.ToString();
    }
    public static string MorphContextFormat(MorphNode me, string _format, float lerp,params DataPool[] dataPools)
    {
        StringBuilder result = new StringBuilder();
        if (dataPools[0] && dataPools[1])
            if (dataPools[0].Shape_or_Morph_context != "" && dataPools[1].Shape_or_Morph_context != "")
                result.AppendFormat("float {0}={1}({2},{3});\n", "d" + me.morphID, _format, dataPools[0].Shape_or_Morph_context, dataPools[1].Shape_or_Morph_context);
            else if (dataPools[0].Shape_or_Morph_context != "" && dataPools[1].Shape_or_Morph_context == "")
                result.Append(MorphContextFormat(me, _format, dataPools[0], null));
            else if (dataPools[0].Shape_or_Morph_context == "" && dataPools[1].Shape_or_Morph_context != "")
                result.Append(MorphContextFormat(me, _format, null, dataPools[1]));
            else { }
        else
            foreach (DataPool it in dataPools)
                if (it && it.Shape_or_Morph_context != "")
                    result.AppendFormat("float {0}={1};\n", "d" + me.morphID, it.Shape_or_Morph_context);
        return result.ToString();
    }
}
