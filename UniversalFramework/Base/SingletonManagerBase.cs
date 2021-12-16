/// <summary>
/// 单例模式管理器基类
/// </summary>
/// <typeparam name="T">继承该类的子类</typeparam>
public abstract class SingletonManagerBase<T> where T : SingletonManagerBase<T>, new()
{
	private static T instance;
	public static T Instance {
		get {
			if (instance == null)
			{
				instance = new T();
			}
			return instance;
		}
	}

}
