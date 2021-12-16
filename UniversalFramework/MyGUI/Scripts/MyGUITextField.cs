using UnityEngine;
using UnityEngine.Events;

public class MyGUITextField : MyGUIControlBase
{
	public event UnityAction<string> textEvent;
	private string oldString;
	protected override void Style()
	{
		content.text = GUI.TextField(pos.RectPos, content.text, style);
		if (oldString != content.text)
		{
			oldString = content.text;
			textEvent?.Invoke(oldString);
		}
	}
	protected override void NoStyle()
	{
		content.text = GUI.TextField(pos.RectPos, content.text);
		if (oldString != content.text)
		{
			oldString = content.text;
			textEvent?.Invoke(oldString);
		}
	}
}
