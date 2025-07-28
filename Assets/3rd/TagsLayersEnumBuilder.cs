#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class TagsLayersEnumBuilder : EditorWindow
{
    [MenuItem("Tools/Rebuild Tags And Layers Enums")]
    static void RebuildTagsAndLayersEnums()
    {
        var enumsPath = Application.dataPath + "/Scripts/Enums/";
        var enumsRelativePath = "Assets/Scripts/Enums/";

        if (!Directory.Exists(enumsPath))
        {
            Directory.CreateDirectory(enumsPath);
        }
        string tagFile = enumsPath + "Tags.cs";
        string layerFile = enumsPath + "Layers.cs";
        CreateFile(tagFile);
        CreateFile(layerFile);

        RebuildTagsFile(tagFile);
        RebuildLayersFile(layerFile);


        AssetDatabase.ImportAsset(enumsRelativePath + "Tags.cs", ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(enumsRelativePath + "Layers.cs", ImportAssetOptions.ForceUpdate);
    }

    static void CreateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            FileStream fs = File.Create(filePath);
            fs.Close();
        }
    }

    static void RebuildTagsFile(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("//This class is auto-generated, do not modify (TagsLayersEnumBuilder.cs)\n");
        sb.Append("public abstract class Tags {\n");

        var srcArr = UnityEditorInternal.InternalEditorUtility.tags;
        var tags = new String[srcArr.Length];
        Array.Copy(srcArr, tags, tags.Length);
        Array.Sort(tags, StringComparer.InvariantCultureIgnoreCase);

        for (int i = 0, n = tags.Length; i < n; ++i)
        {
            string tagName = tags[i];

            sb.Append("\tpublic const string " + CheckStartWithNumberic(tagName + " = \"" + tagName + "\";\n"));
        }

        sb.Append("}\n");

#if !UNITY_WEBPLAYER
        File.WriteAllText(filePath, sb.ToString());
#endif
    }

    static void RebuildLayersFile(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("//This class is auto-generated, do not modify (use Tools/TagsLayersEnumBuilder)\n");
        sb.Append("public abstract class Layers {\n");

        var layers = UnityEditorInternal.InternalEditorUtility.layers;

        for (int i = 0, n = layers.Length; i < n; ++i)
        {
            string layerName = layers[i];

            sb.Append("\tpublic const string " + GetVariableName(layerName) + " = \"" + layerName + "\";\n");
        }

        sb.Append("\n");

        for (int i = 0, n = layers.Length; i < n; ++i)
        {
            string layerName = layers[i];
            int layerNumber = LayerMask.NameToLayer(layerName);
            string layerMask = layerNumber == 0 ? "1" : ("1 << " + layerNumber);

            sb.Append("\tpublic const int " + GetVariableName(layerName) + "Mask" + " = " + layerMask + ";\n");
        }

        sb.Append("\n");

        for (int i = 0, n = layers.Length; i < n; ++i)
        {
            string layerName = layers[i];
            int layerNumber = LayerMask.NameToLayer(layerName);

            sb.Append("\tpublic const int " + GetVariableName(layerName) + "Number" + " = " + layerNumber + ";\n");
        }

        sb.Append("}\n");

#if !UNITY_WEBPLAYER
        File.WriteAllText(filePath, sb.ToString());
#endif
    }

    private static string GetVariableName(string str)
    {
        return CheckStartWithNumberic(str.Replace(" ", ""));
    }

    private static string CheckStartWithNumberic(string str)
    {
        if (str[0] >= '0' && str[0] <= '9')
        {
            return string.Format("_{0}", str);
        }
        else return str;
    }
}
#endif