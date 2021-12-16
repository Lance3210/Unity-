using System;
using System.Collections.Generic;

/// <summary>
/// 数组助手类，自定义条件查找（单个，多个）、排序（升，降）、最值（大，小）、筛选
/// </summary>
public static class ArrayHelper
{
	/// <summary>
	/// 单个查找，返回满足某种自定义条件的单个元素
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="condition">查找条件</param>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T Find<T>(this T[] array, Func<T, bool> condition)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (condition(array[i]))
			{
				return array[i];//调用者指定条件
			}
		}
		return default;//无符合就返回默认值
	}

	/// <summary>
	/// 多个查找，返回满足某种自定义条件的多个元素
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="condition">查找条件</param>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
	{
		List<T> list = new List<T>();
		for (int i = 0; i < array.Length; i++)
		{
			if (condition(array[i]))
			{
				list.Add(array[i]);//添加进集合
			}
		}
		if (list.Count == 0)
		{
			return default;
		}
		return list.ToArray();//转换为数组
	}

	/// <summary>
	/// 返回最大值
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T GetMax<T>(this T[] array)
	{
		T max = array[0];
		for (int i = 0; i < array.Length; i++)
		{
			if (CompareByElement(max, array[i]) < 0)
			{
				max = array[i];
			}
		}
		return max;
	}

	/// <summary>
	/// 返回最大值
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <typeparam name="Q">比较条件的返回值类型</typeparam>
	/// <param name="condition">比较条件</param>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T GetMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
	{
		T max = array[0];
		for (int i = 0; i < array.Length; i++)
		{
			if (condition(max).CompareTo(condition(array[i])) < 0)
			{
				max = array[i];
			}
		}
		return max;
	}

	/// <summary>
	/// 返回最小值
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T GetMin<T>(this T[] array)
	{
		T min = array[0];
		for (int i = 0; i < array.Length; i++)
		{
			if (CompareByElement(min, array[i]) > 0)
			{
				min = array[i];
			}
		}
		return min;
	}

	/// <summary>
	/// 返回最小值
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <typeparam name="Q">比较条件的返回值类型</typeparam>
	/// <param name="condition">比较条件</param>
	/// <param name="array">数组</param>
	/// <returns></returns>
	public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
	{
		T min = array[0];
		for (int i = 0; i < array.Length; i++)
		{
			if (condition(min).CompareTo(condition(array[i])) > 0)
			{
				min = array[i];
			}
		}
		return min;
	}

	/// <summary>
	/// 筛选满足条件的元素，不满足的置为default
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <typeparam name="Q">筛选目标类型</typeparam>
	/// <param name="array">要筛选的数组</param>
	/// <param name="conditon">筛选条件</param>
	/// <returns></returns>
	public static Q[] Select<T, Q>(this T[] array, Func<T, Q> conditon)
	{
		Q[] result = new Q[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			result[i] = conditon(array[i]);//筛选的条件
		}
		return result;
	}

	/// <summary>
	/// 升序排序，默认快速排序
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	public static void AscendingSorting<T>(this T[] array)
	{
		QuickSorting(array, 1);
	}

	/// <summary>
	/// 降序排序，默认快速排序
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	public static void DecendingSorting<T>(this T[] array)
	{
		QuickSorting(array, -1);
	}

	/// <summary>
	/// 根据索引比较元素大小
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	/// <param name="index1"></param>
	/// <param name="index2"></param>
	/// <returns></returns>
	public static int CompareByInde<T>(T[] array, int index1, int index2)
	{
		return ((IComparable)array[index1]).CompareTo(array[index2]);
	}

	/// <summary>
	/// 根据索引交换元素
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="array">数组</param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	private static void SwapByIndex<T>(T[] array, int a, int b)
	{
		T temp = array[a];
		array[a] = array[b];
		array[b] = temp;
	}

	/// <summary>
	/// 比较元素大小
	/// </summary>
	/// <typeparam name="T">元素类型</typeparam>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="flag">取正反</param>
	/// <returns></returns>
	private static int CompareByElement<T>(T a, T b, int flag = 1)
	{
		return flag * ((IComparable)a).CompareTo(b);
	}

	#region 快速排序

	private static void QuickSorting<T>(T[] array, int flag)
	{
		QuickBase(array, 0, array.Length - 1, flag);
	}

	private static void QuickBase<T>(T[] array, int begin, int end, int flag)
	{
		if (end - begin < 2)
		{
			return;
		}
		int pivot = GetPivotIndex(array, begin, end, flag);
		QuickBase(array, begin, pivot, flag);
		QuickBase(array, pivot + 1, end, flag);
	}

	private static Random random = new Random();

	private static int GetPivotIndex<T>(T[] array, int begin, int end, int flag)
	{
		SwapByIndex(array, begin, random.Next(begin, end));//随机元素作为头部轴点
		T pivot = array[begin];//备份轴点
		while (begin < end)
		{
			while (begin < end)
			{
				if (CompareByElement(pivot, array[end], flag) < 0)
				{
					--end;//轴点小于右边
				}
				else
				{
					array[begin++] = array[end];//将end放至begin
					break;
				}
			}
			while (begin < end)
			{
				if (CompareByElement(pivot, array[begin], flag) > 0)
				{
					++begin;//轴点大于左边
				}
				else
				{
					array[end--] = array[begin];//将begin放至end
					break;
				}
			}
		}
		array[begin] = pivot;//插入轴点
		return begin;
	}
	#endregion
}