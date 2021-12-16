using LitJson;
using System.IO;
using UnityEngine;

/// <summary>
/// ѡ��ʹ�õ�Json�洢��ʽ
/// </summary>
public enum E_JsonType
{
	JsonUtility,
	LitJson
}

/// <summary>
/// Json��д������
/// </summary>
public class JsonManager : SingletonManagerBase<JsonManager>
{
	/// <summary>
	/// �������л�ΪJson
	/// </summary>
	/// <param name="data">��������</param>
	/// <param name="path">�ļ�����</param>
	/// <param name="jsonType">���л���ʽ��Ĭ��ʹ��LitJson��</param>
	/// <returns>���ش洢·��</returns>
	public string Save(object data, string fileName, E_JsonType jsonType = E_JsonType.LitJson)
	{
		string path = Application.persistentDataPath + "/" + fileName + ".json";//�̶��洢·��
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
	/// Json�����л�Ϊ����
	/// </summary>
	/// <typeparam name="T">��������</typeparam>
	/// <param name="fileName">�ļ�����</param>
	/// <param name="jsonType">�����л���ʽ��Ĭ��ʹ��LitJson��</param>
	/// <returns></returns>
	public T Load<T>(string fileName, E_JsonType jsonType = E_JsonType.LitJson) where T : new()
	{	
		string path = Application.streamingAssetsPath + "/" + "fileName" + ".json";
		if (!File.Exists(path))//�ж�Ĭ�������ļ������Ƿ���Ŀ������
		{
			path = Application.persistentDataPath + "/" + fileName + ".json";
			if (!File.Exists(path))
			{
				return new T();//���򷵻�Ĭ�϶���
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
