using UnityEngine;

[ExecuteAlways]
public class MyGUIRoot : MonoBehaviour
{
	private MyGUIControlBase[] myGUIBases;
	void Start()
	{
		myGUIBases = this.GetComponentsInChildren<MyGUIControlBase>();
	}
	private void OnGUI()
	{
		if (!Application.isPlaying)//编辑模式下一直运行
			myGUIBases = this.GetComponentsInChildren<MyGUIControlBase>();
		for (int i = 0; i < myGUIBases.Length; i++)//顺便控制层级
		{
			myGUIBases[i].DrawMyGUI();
		}
	}
}
