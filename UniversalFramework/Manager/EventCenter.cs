using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter : SingletonManagerBase<EventCenter>
{
	private static Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

	/// <summary>
	/// 添加事件监听
	/// </summary>
	/// <param name="name">事件名称</param>
	/// <param name="function">要执行的委托</param>
	public void AddEventListener(string name, UnityAction<object> function)
	{
		if (eventDic.ContainsKey(name))
		{
			eventDic[name] += function;
		}
		else
		{
			eventDic.Add(name, function);
		}
	}

	/// <summary>
	/// 触发事件
	/// </summary>
	/// <param name="name">事件名称</param>
	/// <param name="info">委托参数</param>
	public void EventTrigger(string name, object info)
	{
		if (eventDic.ContainsKey(name))
			eventDic[name](info);
	}

	/// <summary>
	/// 移除事件监听
	/// </summary>
	/// <param name="name">事件名称</param>
	/// <param name="function">要销毁的委托</param>
	public void RemoveEventListener(string name, UnityAction<object> function)
	{
		if (eventDic.ContainsKey(name))
			eventDic[name] -= function;
	}

	/// <summary>
	/// 清空事件中心 适合用于场景切换
	/// </summary>
	public void Clear()
	{
		eventDic.Clear();
	}
}
