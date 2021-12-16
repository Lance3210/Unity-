using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器
/// 保证Lua解析器的唯一性
/// 注意：编辑阶段的Lua脚本的存放路径默认在Assets/Scripts/LuaScripts
/// </summary>
public class LuaManager : SingletonManagerBase<LuaManager>
{
    private LuaEnv env;
    public LuaTable Global => env.Global;
    //重定向路径列表（两条默认路径）
    private List<string> pathList = new List<string>(){
        Application.dataPath + "/Scripts/LuaScripts/",
        Application.dataPath + "/UniversalFramework/LuaTools/"
    };
    public List<string> PathList => pathList;

    /// <summary>
    /// Lua解析器初始化
    /// </summary>
    /// <param name="isLoadFromAB">是否从AB包中加载资源</param>
    /// <param name="isCreatePath">是否创建不存在的路径</param>
    /// <param name="loadPaths">新增重定向路径</param>
    public void Initialize(bool isLoadFromAB, bool isCreatePath = false, params string[] loadPaths)
    {
        if (env != null)
        {
            Debug.Log("LuaEnv has been initialized");
            return;
        }
        env = new LuaEnv();//初始化LuaEnv
        if (loadPaths != null)
        {
            LoadNewPathsToList(loadPaths);//新增加载路径列表
        }
        //是否从AB包中加载
        if (isLoadFromAB)
        {
            //最终打包后即可使用下面，从AB包中加载Lua脚本的规则
            env.AddLoader(CustomABLoader);
        }
        else
        {
            AddCustomLoadPaths(isCreatePath);//将所有路径添加至重定向
        }
    }

    /// <summary>
    /// 将所有路径添加至重定向
    /// </summary>
    /// <param name="isCreatePath">是否创建不存在的路径</param>
    private void AddCustomLoadPaths(bool isCreatePath)
    {
        foreach (var path in pathList)
        {
            env.AddLoader((ref string filePath) =>
            {
                if (!Directory.Exists(path))
                {
                    Debug.Log("Path does not exist");
                    if (isCreatePath)
                    {
                        Directory.CreateDirectory(path);//新建不存在的文件夹
                    }
                }
                string luaScriptPath = path + filePath + ".lua";//文件名
                if (File.Exists(luaScriptPath))
                {
                    return File.ReadAllBytes(luaScriptPath);
                }
                else
                    // 若有多条读取路径时误使用下面报错代码
                    // {
                    //     Debug.Log("Custom loading path failed，file name: " + filePath);
                    // }
                    return null;
            });
        }
    }

    /// <summary>
    /// 新增读取路径
    /// </summary>
    private void LoadNewPathsToList(params string[] loadPaths)
    {
        if (loadPaths != null)
        {
            for (int i = 0; i < loadPaths.Length; i++)
            {
                pathList.Add(loadPaths[i]);//加载新增路径
            }
        }
    }

    /// <summary>
    /// 加载AB包中的Lua脚本规则
    /// </summary>
    /// <param name="filePath">脚本名称</param>
    /// <returns>读取文件的字节数组</returns>
    private byte[] CustomABLoader(ref string filePath)
    {
        //利用AssetBundleManager加载Lua文件
        TextAsset luaText = AssetBundleManager.Instance.LoadRes<TextAsset>("lua", filePath + ".lua");
        if (luaText == null)
        {
            Debug.Log("Failed to load the script in AssetBundle, AB name: " + "lua" + "file name: " + filePath); ;
            return null;
        }
        return luaText.bytes;//返回byte数组
    }

    /// <summary>
    /// 执行Lua脚本
    /// </summary>
    /// <param name="name">脚本名称</param>
    public void DoLuaScript(string name)
    {
        DoString($"require('{name}')");
    }

    /// <summary>
    /// 执行Lua语句
    /// </summary>
    /// <param name="code">Lua代码</param>
    public void DoString(string code)
    {
        if (env == null)
        {
            Debug.Log("LuaEnv is null");
            return;
        }
        env.DoString(code);
    }

    /// <summary>
    /// 释放Lua垃圾
    /// </summary>
    public void Tick()
    {
        if (env == null)
        {
            Debug.Log("LuaEnv is null");
            return;
        }
        env.Tick();
    }

    /// <summary>
    /// 销毁解析器
    /// </summary>
    public void Dispose()
    {
        if (env == null)
        {
            Debug.Log("LuaEnv is null");
            return;
        }
        env.Dispose();
        env = null;
    }
}
