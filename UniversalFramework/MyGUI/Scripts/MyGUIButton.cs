using UnityEngine;
using UnityEngine.Events;

public class MyGUIButton : MyGUIControlBase
{
	public event UnityAction clickEvent;
	protected override void Style()
	{
		if (GUI.Button(pos.RectPos, content, style))
		{
			clickEvent?.Invoke();
		}
	}
	protected override void NoStyle()
	{
		if (GUI.Button(pos.RectPos, content))
		{
			clickEvent?.Invoke();
		}
	}
}
