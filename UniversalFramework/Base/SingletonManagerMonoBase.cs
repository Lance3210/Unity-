using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的单例模式管理器基类
/// </summary>
/// <typeparam name="T">继承该类的子类</typeparam>
[DisallowMultipleComponent]
public abstract class SingletonManagerMonoBase<T> : MonoBehaviour where T : SingletonManagerMonoBase<T>
{
	private static T instance;
	[Tooltip("你可以控制该脚本在切换场景时是否销毁（非根物体无效）")]
	public bool isDestoryOnLoad = false;
	public static T Instance {
		get {
			if (instance == null)
			{
				GameObject obj = new GameObject();
				instance = obj.AddComponent<T>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			name = typeof(T).Name;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if (!isDestoryOnLoad && transform.parent == null)
		{
			DontDestroyOnLoad(gameObject);//DontDestroyOnLoad要求物体必须为根物体
		}
	}
}
