using LitJson;
using System.IO;
using UnityEngine;

/// <summary>
/// 选择使用的Json存储方式
/// </summary>
public enum E_JsonType
{
	JsonUtility,
	LitJson
}

/// <summary>
/// Json读写管理器
/// </summary>
public class JsonManager : SingletonManagerBase<JsonManager>
{
	/// <summary>
	/// 对象序列化为Json
	/// </summary>
	/// <param name="data">对象数据</param>
	/// <param name="path">文件名称</param>
	/// <param name="jsonType">序列化方式（默认使用LitJson）</param>
	/// <returns>返回存储路径</returns>
	public string Save(object data, string fileName, E_JsonType jsonType = E_JsonType.LitJson)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".json";//固定存储路径
		string jsonStr = "";
		jsonStr = jsonType switch {
			E_JsonType.JsonUtility => JsonUtility.ToJson(data),
			E_JsonType.LitJson => JsonMapper.ToJson(data),
			_ => ""
		};
		File.WriteAllText(path, jsonStr);
		return path;
	}

	/// <summary>
	/// Json反序列化为对象
	/// </summary>
	/// <typeparam name="T">对象类型</typeparam>
	/// <param name="fileName">文件名称</param>
	/// <param name="jsonType">反序列化方式（默认使用LitJson）</param>
	/// <returns></returns>
	public T Load<T>(string fileName, E_JsonType jsonType = E_JsonType.LitJson) where T : new()
	{	
		string path = Application.streamingAssetsPath + "/" + "fileName" + ".json";
		if (!File.Exists(path))//判断默认数据文件夹中是否有目标数据
		{
			path = Application.persistentDataPath + "/" + fileName + ".json";
			if (!File.Exists(path))
			{
				return new T();//无则返回默认对象
			}
		}
		string jsonStr = File.ReadAllText(path);
		return jsonType switch {
			E_JsonType.JsonUtility => JsonUtility.FromJson<T>(jsonStr),
			E_JsonType.LitJson => JsonMapper.ToObject<T>(jsonStr),
			_ => new T()
		};
	}
}
