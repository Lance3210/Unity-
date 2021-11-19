using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 封装异步加载，传入加载后需要执行的逻辑的委托，如需参数，则为该类型（只能获取一个资源）
/// </summary>
public class AsyncLoadManager : SingletonManagerBase<AsyncLoadManager> {
	/// <summary>
	/// 单个资源异步加载
	/// </summary>
	/// <typeparam name="T">资源类型</typeparam>
	/// <param name="site">资源位置名称</param>
	/// <param name="callBack">使用该资源的方法</param>
	public void AsyncLoad<T>(string site, UnityAction<T> callBack) where T : Object {
		ResourceRequest request = Resources.LoadAsync<T>(site);
		request.completed += (a) => {
			callBack((a as ResourceRequest).asset as T);//封装了转类型这一步
		};
	}

	/// <summary>
	/// 场景异步加载及完成后逻辑
	/// </summary>
	/// <param name="scene">场景名称</param>
	/// <param name="callBack">加载完成后的逻辑</param>
	public void AsyncSceneLoad(string scene, UnityAction callBack) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
		operation.completed += (a) => {
			callBack();
		};
	}

	/// <summary>
	/// 场景异步加载
	/// </summary>
	/// <param name="scene"></param>
	public void AsyncSceneLoad(string scene) {
		SceneManager.LoadScene(scene);
	}
}