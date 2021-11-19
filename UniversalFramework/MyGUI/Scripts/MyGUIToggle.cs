using UnityEngine;
using UnityEngine.Events;

public class MyGUIToggle : MyGUIControlBase {
	public bool isSeleted;
	public event UnityAction<bool> clickEvent;
	private bool oldSeleted;
	protected override void Style() {
		isSeleted = GUI.Toggle(pos.RectPos, isSeleted, content, style);
		if (oldSeleted != isSeleted) {
			oldSeleted = isSeleted;
			clickEvent?.Invoke(isSeleted);
		}
	}
	protected override void NoStyle() {
		isSeleted = GUI.Toggle(pos.RectPos, isSeleted, content);
		if (oldSeleted != isSeleted) {
			oldSeleted = isSeleted;
			clickEvent?.Invoke(isSeleted);
		}
	}

}
