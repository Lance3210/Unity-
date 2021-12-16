using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子对象工具类
/// </summary>
public static class ChildrenHelper
{
	/// <summary>
	/// 按名字长度升序为所有子对象排序
	/// </summary>
	/// <param name="father">父对象</param>
	public static void SortChildrenByNameLength(this Transform father)
	{
		for (int i = 0; i < father.childCount; i++)
		{
			for (int j = 0; j < father.childCount - i - 1; j++)
			{
				if (father.GetChild(j).name.Length > father.GetChild(j + 1).name.Length)
					father.GetChild(j).SetSiblingIndex(j + 1);
			}
		}
	}

	/// <summary>
	/// 按名字长度升序为所有子对象排序（Sort()实现）
	/// </summary>
	/// <param name="father">父对象</param>
	public static void SortChildrenByNameLengthList(this Transform father)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < father.childCount; i++)
		{
			list.Add(father.GetChild(i));
		}
		list.Sort((a, b) => {
			if (a.name.Length < b.name.Length) return -1;
			return 1;
		});
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetSiblingIndex(i);
		}
	}

	/// <summary>
	/// 在子对象中查找同名目标对象
	/// </summary>
	/// <param name="father">父对象</param>
	/// <param name="name">目标子对象名称</param>
	/// <returns>目标子对象</returns>
	public static Transform FindGameObjectInAllChildren(this Transform father, string name)
	{
		if (father.childCount == 0) return null;
		Transform child = father.Find(name);
		if (child != null) return child;
		for (int i = 0; i < father.childCount; i++)
		{
			child = father.GetChild(i).FindGameObjectInAllChildren(name);
		}
		return child;
	}

	/// <summary>
	/// 在子对象中查找所有同名目标对象
	/// </summary>
	/// <param name="transform">父对象</param>
	/// <param name="name">目标子对象名称</param>
	/// <returns>目标子对象数组</returns>
	public static Transform[] FindGameObjectsInAllChildren(this Transform father, string name)
	{
		if (father.childCount == 0) return null;
		List<Transform> children = new List<Transform>();
		for (int i = 0; i < father.childCount; i++)
		{
			if (father.GetChild(i).childCount > 0)
				children.AddRange(father.GetChild(i).FindGameObjectsInAllChildren(name));
			if (father.GetChild(i).name == name)
				children.Add(father.GetChild(i));
		}
		return children.ToArray();
	}

	/// <summary>
	/// 返回子对象总个数
	/// </summary>
	/// <param name="transform">父对象</param>
	/// <returns>子对象个数</returns>
	public static int GetAllChildrenCount(this Transform father)
	{
		int count = father.childCount;
		if (count == 0) return 0;
		for (int i = 0; i < father.childCount; i++)
		{
			count += father.GetChild(i).GetAllChildrenCount();
		}
		return count;
	}

	/// <summary>
	/// 返回所有子对象
	/// </summary>
	/// <param name="father">父对象</param>
	/// <returns>子对象数组</returns>
	public static Transform[] GetAllChildren(this Transform father)
	{
		if (father.childCount == 0) return null;
		List<Transform> children = new List<Transform>();
		for (int i = 0; i < father.childCount; i++)
		{
			if (father.GetChild(i).childCount > 0)
				children.AddRange(father.GetChild(i).GetAllChildren());
			children.Add(father.GetChild(i));
		}
		return children.ToArray();
	}
}
