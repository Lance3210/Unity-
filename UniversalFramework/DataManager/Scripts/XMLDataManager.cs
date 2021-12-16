using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
/// <summary>
/// XML数据管理类
/// </summary>
public class XMLDataManager : SingletonManagerBase<XMLDataManager>
{
	public void SaveData(object data, string fileName)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".xml";
		using (StreamWriter writer = new StreamWriter(path))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
			xmlSerializer.Serialize(writer, data);
		}
	}

	public void SaveData(object data, string fileName, bool isShowPath)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".xml";
		using (StreamWriter writer = new StreamWriter(path))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
			xmlSerializer.Serialize(writer, data);
		}
		if (isShowPath)
		{
			Debug.Log(path);
		}
	}

	public object LoadData(Type type, string fileName)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".xml";
		if (!File.Exists(path))
		{
			path = Application.streamingAssetsPath + "/" + fileName + ".xml";
			if (!File.Exists(path))
			{
				return Activator.CreateInstance(type);//无则返回默认值
			}
		}
		using (StreamReader reader = new StreamReader(path))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(reader);
		}
	}

	public object LoadData(Type type, string fileName, bool isShowPath)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".xml";
		if (isShowPath)
		{
			Debug.Log(path);
		}
		if (!File.Exists(path))
		{
			path = Application.streamingAssetsPath + "/" + fileName + ".xml";
			if (!File.Exists(path))
			{
				return Activator.CreateInstance(type);//无则返回默认值
			}
		}
		using (StreamReader reader = new StreamReader(path))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(reader);
		}
	}
}
