using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 数组助手类，可以对数组进行一些改造和操作
/// 自定义条件查找（单个，多个）、排序（升，降）、最值（大，小）、筛选
/// </summary>
public static class ArrayHelper
{
    /// <summary>
    /// 单个查找，返回满足某种自定义条件的单个元素
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="condition">查找条件</param>
    /// <param name="array">要查找的数组</param>
    /// <returns></returns>
    public static T Find<T>(this T[] array, Func<T, bool> condition)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))//调用者指定条件
                return array[i];
        }
        return default;//无符合就返回泛型默认值
    }

    /// <summary>
    /// 多个查找，返回满足某种自定义条件的多个元素
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="condition">查找条件</param>
    /// <param name="array">要查找的数组</param>
    /// <returns></returns>
    public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
    {
        List<T> list = new List<T>();//集合存储满足条件的元素，由于个数不定，最好用动态
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
                list.Add(array[i]);//添加进集合
        }
        if (list.Count == 0)
            return default;
        return list.ToArray();//转换为数组
    }

    //接口CompareTo方法是系统自带的方法，可以在int,float等继承了该接口的类型后直接点出来使用，会返回一个int
    /// <summary>
    /// 返回最大值
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较条件的返回值类型</typeparam>
    /// <param name="condition">要比较的方法</param>
    /// <param name="array">要比较的数组</param>
    /// <returns></returns>
    public static T GetMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable//接口，该接口返回一个int
    {
        T max = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(max).CompareTo(condition(array[i])) < 0)//CompareTo方法比较条件，小于零就是true(A - B) < 0)
                max = array[i];
        }
        return max;
    }

    /// <summary>
    /// 返回最小值
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较条件的返回值类型</typeparam>
    /// <param name="condition">要比较的方法</param>
    /// <param name="array">要比较的数组</param>
    /// <returns></returns>
    public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable//接口
    {
        T min = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(min).CompareTo(condition(array[i])) > 0)//CompareTo方法比较条件，大于零就是false(A - B) > 0)
                min = array[i];
        }
        return min;
    }

    /// <summary>
    /// 返回降序排列数组
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较条件的返回值类型</typeparam>
    /// <param name="array">要比较的数组</param>
    /// <param name="condition">要比较的方法</param>
    public static T[] OrderDescending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                {
                    T temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
        return array;
    }

    /// <summary>
    /// 返回升序排列数组
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较条件的返回值</typeparam>
    /// <param name="array">要比较的数组</param>
    /// <param name="condition">要比较的方法</param>
    /// <returns></returns>
    public static T[] OrderAdditon<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                if (condition(array[j]).CompareTo(condition(array[j + 1])) > 0)
                {
                    T temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
        return array;
    }

    /// <summary>
    /// 筛选
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">筛选目标类型</typeparam>
    /// <param name="array">要筛选的数组</param>
    /// <param name="conditon">筛选条件</param>
    /// <returns></returns>
    public static Q[] Select<T, Q>(this T[] array, Func<T, Q> conditon)
    {
        Q[] result = new Q[array.Length];//存储满足条件的元素
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = conditon(array[i]);//筛选的条件
        }
        return result;
    }
}