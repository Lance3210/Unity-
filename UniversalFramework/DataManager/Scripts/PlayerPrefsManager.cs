using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 数据存储管理类
/// </summary>
public class PlayerPrefsManager : SingletonManagerBase<PlayerPrefsManager>
{
	/// <summary>
	/// 保存对象数据
	/// </summary>
	/// <param name="obj">保存对象</param>
	/// <param name="keyName">对象名</param>
	public void Save(object obj, string keyName)
	{
		Type type = obj?.GetType();//获取对象类型，空则跳过，存在空引用字段（或对象）自动忽略
		FieldInfo[] allFieldInfos = type?.GetFields();//获取所有字段，空则跳过
		string saveKeyName;
		for (int i = 0; i < allFieldInfos?.Length; i++)//空则不执行
		{
			saveKeyName = keyName + "_" + allFieldInfos[i].FieldType.Name + "_" + allFieldInfos[i].Name;//自定义键规则
			SaveValue(allFieldInfos[i].GetValue(obj), saveKeyName);//插入得到的数据和自定义键名
		}
		PlayerPrefs.Save();//保存到硬盘
	}

	/// <summary>
	/// //封装数据保存方法
	/// </summary>
	/// <param name="value">字段值</param>
	/// <param name="saveKeyName">键名</param>
	private void SaveValue(object value, string saveKeyName)
	{
		Type fieldType = value?.GetType();//获取字段类型用于分类判断

		//Int32
		if (fieldType == typeof(int))
		{
			PlayerPrefs.SetInt(saveKeyName, (int)value);
		}
		//Single
		else if (fieldType == typeof(float))
		{
			PlayerPrefs.SetFloat(saveKeyName, (float)value);
		}
		//Double
		else if (fieldType == typeof(double))
		{
			PlayerPrefs.SetFloat(saveKeyName, (float)(double)value);//降精度
		}
		//String
		else if (fieldType == typeof(string))
		{
			PlayerPrefs.SetString(saveKeyName, value.ToString());
		}
		//Boolean
		else if (fieldType == typeof(bool))
		{
			PlayerPrefs.SetInt(saveKeyName, (bool)value ? 1 : 0);//转类型存储
		}
		//List<> And Array
		else if (typeof(IList).IsAssignableFrom(fieldType))//用父类接口判断是否是List或是Array（数组）类型
		{
			IList list = value as IList;//父类装子类
			PlayerPrefs.SetInt(saveKeyName + "_count", list.Count);//记录长度，加载要用
			for (int i = 0; i < list.Count; i++)
			{
				SaveValue(list[i], saveKeyName + "_" + i);
			}
		}
		//Dictionary<>
		else if (typeof(IDictionary).IsAssignableFrom(fieldType))
		{
			IDictionary dictionary = value as IDictionary;
			PlayerPrefs.SetInt(saveKeyName + "_count", dictionary.Count);
			int index = 0;
			foreach (var key in dictionary.Keys)//遍历所有键来存储键和值
			{
				SaveValue(key, saveKeyName + "_key_" + index);
				SaveValue(dictionary[key], saveKeyName + "_value_" + index);
				++index;
			}
		}
		//CustomClass
		else
		{
			Save(value, saveKeyName);//递归获取
		}
	}

	/// <summary>
	/// 读取对象数据
	/// </summary>
	/// <param name="type">对象类型</param>
	/// <param name="keyName">对象名</param>
	/// <returns></returns>
	public object Load(Type type, string keyName)
	{
		object data = Activator.CreateInstance(type);//创建一个对象
		FieldInfo[] allFieldInfos = type.GetFields();//填充所有字段
		string saveKeyName;
		for (int i = 0; i < allFieldInfos.Length; i++)
		{
			saveKeyName = keyName + "_" + allFieldInfos[i].FieldType.Name + "_" + allFieldInfos[i].Name;
			allFieldInfos[i].SetValue(data, LoadValue(allFieldInfos[i].FieldType, saveKeyName));//为新对象赋值
		}
		return data;//返回新对象
	}

	/// <summary>
	/// 封装数据读取方法
	/// </summary>
	/// <param name="fieldType">字段类型</param>
	/// <param name="saveKeyName">键名</param>
	/// <returns>返回字段值</returns>
	private object LoadValue(Type fieldType, string saveKeyName)
	{
		//Int32
		if (fieldType == typeof(int))
		{
			return PlayerPrefs.GetInt(saveKeyName, 0);
		}
		//Single
		else if (fieldType == typeof(float))
		{
			return PlayerPrefs.GetFloat(saveKeyName, 0);
		}
		//Double
		else if (fieldType == typeof(double))
		{
			return (double)PlayerPrefs.GetFloat(saveKeyName, 0);
		}
		//String
		else if (fieldType == typeof(string))
		{
			return PlayerPrefs.GetString(saveKeyName, "");
		}
		//Boolean
		else if (fieldType == typeof(bool))
		{
			return PlayerPrefs.GetInt(saveKeyName, 0) == 1 ? true : false;
		}
		//Array And List<>
		else if (typeof(IList).IsAssignableFrom(fieldType))//Array和List都继承了IList接口
		{
			int count = PlayerPrefs.GetInt(saveKeyName + "_count", 0);//获取长度
			if (fieldType.IsArray)//处理数组
			{
				Array array = Activator.CreateInstance(fieldType, count) as Array;//创建对象
				for (int i = 0; i < count; i++)
				{
					array.SetValue(LoadValue(fieldType.GetElementType(), saveKeyName + "_" + i), i);//填充对象
				}
				return array;//返回新对象
			}
			else//处理List<>
			{
				IList list = Activator.CreateInstance(fieldType, count) as IList;//创建对象
				for (int i = 0; i < count; i++)
				{
					list.Add(LoadValue(fieldType.GetGenericArguments()[0], saveKeyName + "_" + i));//获取所有泛型的Type；填充对象
				}
				return list;//返回新对象
			}

		}
		//Dictionary<>
		else if (typeof(IDictionary).IsAssignableFrom(fieldType))
		{
			IDictionary dictionary = Activator.CreateInstance(fieldType) as IDictionary;//创建新对象
			int count = PlayerPrefs.GetInt(saveKeyName + "_count", 0);//获取长度
			Type[] keyAndValueTypes = fieldType.GetGenericArguments();//获取键和值的泛型类型
			for (int i = 0; i < count; i++)
			{
				dictionary.Add(LoadValue(keyAndValueTypes[0], saveKeyName + "_key_" + i),
					LoadValue(keyAndValueTypes[1], saveKeyName + "_value_" + i));
			}
			return dictionary;//返回新对象
		}
		//CustomClass
		else
		{
			return Load(fieldType, saveKeyName);//递归赋值
		}
	}

	/// <summary>
	/// 清除所有数据
	/// </summary>
	public void DeleteAllData()
	{
		PlayerPrefs.DeleteAll();
	}
}
