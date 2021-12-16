using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_Platform
{
    PC,
    IOS,
    Android,
    Other
}

public enum E_Path
{
    streamingAssetsPath,
    persistentDataPath,
    dataPath
}

/// <summary>
/// AB包管理器
/// </summary>
public class AssetBundleManager : SingletonManagerMonoBase<AssetBundleManager>
{
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();//AB包集合
    private AssetBundle mainAB;//主包
    private AssetBundleManifest manifest;//配置文件
    public E_Platform platform;//平台
    public E_Path path;//AB包存放地址	
    private string Path
    {
        get
        {
            return path switch
            {
                E_Path.streamingAssetsPath => Application.streamingAssetsPath + "/" + platform + "/",
                E_Path.persistentDataPath => Application.persistentDataPath + "/" + platform + "/",
                E_Path.dataPath => Application.dataPath + "/" + platform + "/",
                _ => Application.streamingAssetsPath + "/" + platform + "/",
            };
        }
    }

    /// <summary>
    /// 同步加载AB包
    /// </summary>
    /// <param name="abName">AB包名称</param>
    public void LoadAB(string abName)
    {
        /*加载主包与依赖包*/
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(Path + platform);//加载主包
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        string[] strs = manifest.GetAllDependencies(abName);//加载配置文件		
        AssetBundle ab;
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(Path + strs[i]);
                abDic.Add(strs[i], ab);//加载所有有依赖关系的包
            }
        }
        /*加载目标包*/
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(Path + abName);
            abDic.Add(abName, ab);
        }
    }

    /// <summary>
    /// 同步加载单个资源
    /// </summary>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName)
    {
        /*加载AB包及其依赖包*/
        LoadAB(abName);
        /*加载资源*/
        return abDic[abName].LoadAsset(resName);
    }

    /// <summary>
    /// 同步加载单个指定类型的资源
    /// </summary>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <param name="type">资源类型</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        /*加载AB包及其依赖包*/
        LoadAB(abName);
        /*加载资源*/
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
        {
            return Instantiate(obj);//若是GameObject则直接实例化
        }
        return obj;
    }

    /// <summary>
    /// 同步加载单个泛型类型的资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        /*加载AB包及其依赖包*/
        LoadAB(abName);
        /*加载资源*/
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);//若是GameObject则直接实例化
        }
        return obj;
    }

    /// <summary>
    /// 同步卸载单个包
    /// </summary>
    public void Unload(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);//默认把实现物体一起清除
            abDic.Remove(abName);
        }
    }

    /// <summary>
    /// 同步清空所有已加载的AB包
    /// </summary>
    public void UnloadAll()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

    /// <summary>
    /// 异步加载单个资源（会先同步加载AB包）
    /// </summary>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <param name="callBack">回调函数</param>
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadResAsyncCoruntine(abName, resName, callBack));
    }

    /// <summary>
    /// 异步加载单个指定类型的资源（会先同步加载AB包）
    /// </summary>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <param name="type">资源类型</param>
    /// <param name="callBack">回调函数</param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadResAsyncCoruntine_Type(abName, resName, type, callBack));
    }

    /// <summary>
    /// 异步加载单个泛型资源（会先同步加载AB包）
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <param name="callBack">回调函数</param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<Object> callBack) where T : Object
    {
        StartCoroutine(LoadResAsyncCoruntine_Generics<T>(abName, resName, callBack));
    }

    //异步加载资源协程函数
    private IEnumerator LoadResAsyncCoruntine(string abName, string resName, UnityAction<Object> callBack)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        callBack(abr.asset);//异步加载结束后通过委托传递给外部
    }

    //指定类型异步加载资源协程函数
    private IEnumerator LoadResAsyncCoruntine_Type(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        callBack(abr.asset);//异步加载结束后通过委托传递给外部
    }

    //泛型异步加载资源协程函数
    private IEnumerator LoadResAsyncCoruntine_Generics<T>(string abName, string resName, UnityAction<Object> callBack) where T : Object
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        callBack(abr.asset as T);//异步加载结束后通过委托传递给外部
    }
}