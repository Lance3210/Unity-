using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LuaPacking : Editor
{
    //拷贝Lua文件到新文件夹
    private static string luaTxtPath = Application.dataPath + "/ABRes/LuaTxt/";

    [MenuItem("XLua/Lua Packing/Copy .lua to .txt")]
    public static void CopyLuaToTxt()
    {
        //拷贝前清空旧路径下的lua文件
        ClearLuaTxt();
        //找到所有Lua文件
        foreach (var path in LuaManager.Instance.PathList)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            //得到过滤后的所有Lua文件路径
            string[] paths = Directory.GetFiles(path, "*.lua");
            if (!Directory.Exists(luaTxtPath))
            {
                Directory.CreateDirectory(luaTxtPath);
            }
            //拷贝到新路径
            string fileName;
            for (int i = 0; i < paths.Length; i++)
            {
                //截取Lua脚本名称，注意不包括'/'
                fileName = luaTxtPath + paths[i].Substring(paths[i].LastIndexOf("/") + 1) + ".txt";
                File.Copy(paths[i], fileName);
            }
        }
        AssetDatabase.Refresh();
        //打包成AB包
        PackageIntoAssetBundle();
    }

    [MenuItem("XLua/Lua Packing/Clear \"ABRes\"LuaTxt\"")]
    public static void ClearLuaTxt()
    {
        if (Directory.Exists(luaTxtPath))
        {
            string[] oldFiles = Directory.GetFiles(luaTxtPath);
            for (int i = 0; i < oldFiles.Length; i++)
            {
                File.Delete(oldFiles[i]);
            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("XLua/Lua Packing/Package .lua into AB")]
    public static void PackageIntoAssetBundle()
    {
        AssetDatabase.Refresh();
        string[] paths = Directory.GetFiles(luaTxtPath, "*.txt");
        foreach (var file in paths)
        {
            //通过AssetImporter类得到资源并改写其AB包名称
            //注意GetAtPath有固定规则，path必须是Assets/.../...（故需要截取到Assets）
            AssetImporter importer = AssetImporter.GetAtPath(file.Substring(file.IndexOf("Assets")));
            if (importer != null)
            {
                importer.assetBundleName = "lua";
            }
        }
    }
}
