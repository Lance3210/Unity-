using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理器
/// </summary>
public class ObjectPoolManager : SingletonManagerMonoBase<ObjectPoolManager>
{
	private Dictionary<string, Shelf> poolDic = new Dictionary<string, Shelf>();//Shelf封装存储架
	private GameObject poolObj;//层级中的对象池 Pool         

	/// <summary>
	/// 取出对象池中的对象
	/// </summary>
	/// <param name="prefab">目标对象</param>
	/// <returns>取出对象</returns>
	public GameObject GetObject(GameObject prefab)
	{
		GameObject obj;
		if (poolDic.ContainsKey(prefab.name) && poolDic[prefab.name].shelfQueue.Count > 0)
		{
			obj = poolDic[prefab.name].PopObj(ref prefab);//池中有 Key 且该存储架不为空则取出
		}
		else
		{
			obj = Instantiate(prefab);  //无则实例化指定对象
			obj.name = prefab.name;     //防止clone后缀且取出对象名与传入对象名相同可以方便回收
		}
		obj.SetActive(true);            //取出后就设置为激活
		return obj;
	}

	/// <summary>
	/// 把对象放入对象池
	/// </summary>
	/// <param name="targetObj">目标对象</param>
	public void PushObject(GameObject targetObj)
	{
		if (poolObj == null) poolObj = new GameObject("Pool");  //第一个时创建父对象装载
		targetObj.SetActive(false);                             //失活后放入
		if (poolDic.ContainsKey(targetObj.name))
		{
			poolDic[targetObj.name].PushObj(ref targetObj);//池中有 Key 则放回对应存储架
		}
		else
		{
			poolDic.Add(targetObj.name, new Shelf(targetObj, poolObj)); //无则新建
		}
	}

	/// <summary>
	/// 延迟指定时间后把对象放入对象池
	/// </summary>
	/// <param name="targetObj">目标对象</param>
	/// <param name="time">指定时间</param>
	public void PushObject(GameObject targetObj, float time)
	{
		StartCoroutine(Wait(targetObj, time));
	}
	private IEnumerator Wait(GameObject targetObj, float time)
	{
		yield return new WaitForSeconds(time);
		PushObject(targetObj);
	}

	/// <summary>
	/// 清空缓存池
	/// </summary>
	public void ClearPool()
	{
		poolDic.Clear();
		Destroy(poolObj);
	}

	/// <summary>
	/// 预先加载指定数量的对象到对象池
	/// </summary>
	/// <param name="prefab">目标对象</param>
	/// <param name="count">数量</param>
	public void PreLoad(GameObject prefab, int count)
	{
		GameObject obj;
		for (int i = 0; i < count; i++)
		{
			obj = Instantiate(prefab);
			obj.name = prefab.name;
			obj.SetActive(false);
			PushObject(obj);
		}
	}
}

/// <summary>
/// 封装对象池中的存储架数据类
/// </summary>
public class Shelf
{
	private GameObject eachFatherObj;       //层级中相同种类统一管理的父对象
	public Queue<GameObject> shelfQueue;    //储存架

	/// <summary>
	/// 新建存储架
	/// </summary>
	/// <param name="obj">需要新建的存储架管理的对象</param>
	/// <param name="poolObj">场景中的缓存池对象</param>
	public Shelf(GameObject obj, GameObject poolObj)
	{
		shelfQueue = new Queue<GameObject>();
		eachFatherObj = new GameObject(obj.name);
		eachFatherObj.transform.parent = poolObj.transform;
		PushObj(ref obj);
	}

	/// <summary>
	/// 取出存储架中对象
	/// </summary>
	/// <returns>取出的对象</returns>
	public GameObject PopObj(ref GameObject obj)
	{
		obj = shelfQueue.Dequeue();     //取出第一个并移除
		obj.transform.parent = null;    //置空父对象，在层级中单独显示
		return obj;
	}

	/// <summary>
	/// 把对象放入存储架
	/// </summary>
	/// <param name="obj">放入的对象</param>
	public void PushObj(ref GameObject obj)
	{
		shelfQueue.Enqueue(obj);                        //放回存储架
		obj.transform.parent = eachFatherObj.transform; //设置对象为层级中存储架的子对象
	}
}

