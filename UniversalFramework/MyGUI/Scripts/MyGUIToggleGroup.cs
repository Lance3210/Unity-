using UnityEngine;

public class MyGUIToggleGroup : MonoBehaviour
{
	private MyGUIToggle[] toggles;
	private MyGUIToggle flag;
	private void Start()
	{
		toggles = GetComponentsInChildren<MyGUIToggle>();
		if (toggles.Length == 0) return;
		for (int i = 0; i < toggles.Length; i++)
		{
			MyGUIToggle toggle = toggles[i];//闭包
			toggle.clickEvent += (value) => {
				if (value)
				{
					for (int j = 0; j < toggles.Length; j++)
					{
						if (toggles[j] != toggle)
						{
							toggles[j].isSeleted = false;//其他为false
						}
					}
					flag = toggle;//记录上次
				}
				else if (toggle == flag)//保证单选
				{
					toggle.isSeleted = true;
				}
			};
		}
	}

	private void Update()
	{
		if (!Application.isPlaying) toggles = GetComponentsInChildren<MyGUIToggle>();
	}
}
