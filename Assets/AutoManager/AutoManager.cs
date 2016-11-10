using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;

public class AutoManager : Editor{
    private static readonly string createFoldPath = @"/RayMarching/";
    private static readonly string shaderName = @"SimpleModel.cginc";
	public static void CreateMyAsset(string _code)
    {
        if (_code == null)
            return;
        //-------------
        using (StreamWriter wr = new StreamWriter(Application.dataPath + createFoldPath + shaderName))
        {
            wr.Write(_code);
            wr.Flush();
            wr.Close();
        }
        //-------------
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

}
