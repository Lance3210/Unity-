using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono控制器, 被封装的控制逻辑
/// </summary>
public class MonoEvent : MonoBehaviour
{
	private event UnityAction updateEvent;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		updateEvent?.Invoke();
	}

	/// <summary>
	/// 添加外部事件监听 使传入方法可用于 Update 祯更新方法
	/// </summary>
	/// <param name="function">需要执行的方法</param>
	public void AddUpdateListener(UnityAction function)
	{
		updateEvent += function;
	}

	/// <summary>
	/// 移除外部事件监听 
	/// </summary>
	/// <param name="function">需要取消执行的方法</param>
	public void RemoveUpdateListener(UnityAction function)
	{
		updateEvent -= function;
	}
}
