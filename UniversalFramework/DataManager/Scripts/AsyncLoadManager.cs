using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// ��װ�첽���أ�������غ���Ҫִ�е��߼���ί�У������������Ϊ�����ͣ�ֻ�ܻ�ȡһ����Դ��
/// </summary>
public class AsyncLoadManager : SingletonManagerBase<AsyncLoadManager> {
	/// <summary>
	/// ������Դ�첽����
	/// </summary>
	/// <typeparam name="T">��Դ����</typeparam>
	/// <param name="site">��Դλ������</param>
	/// <param name="callBack">ʹ�ø���Դ�ķ���</param>
	public void AsyncLoad<T>(string site, UnityAction<T> callBack) where T : Object {
		ResourceRequest request = Resources.LoadAsync<T>(site);
		request.completed += (a) => {
			callBack((a as ResourceRequest).asset as T);//��װ��ת������һ��
		};
	}

	/// <summary>
	/// �����첽���ؼ���ɺ��߼�
	/// </summary>
	/// <param name="scene">��������</param>
	/// <param name="callBack">������ɺ���߼�</param>
	public void AsyncSceneLoad(string scene, UnityAction callBack) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
		operation.completed += (a) => {
			callBack();
		};
	}

	/// <summary>
	/// �����첽����
	/// </summary>
	/// <param name="scene"></param>
	public void AsyncSceneLoad(string scene) {
		SceneManager.LoadScene(scene);
	}
}