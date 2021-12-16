using UnityEngine;

public class MyGUILabel : MyGUIControlBase
{
	protected override void Style()
	{
		GUI.Label(pos.RectPos, content, style);
	}
	protected override void NoStyle()
	{
		GUI.Label(pos.RectPos, content);
	}
}
