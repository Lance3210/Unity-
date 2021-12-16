using UnityEngine;
using UnityEngine.Events;

public class MyGUISlider : MyGUIControlBase
{
	[Header("Slider Property")]
	public float minValue;
	public float maxValue = 1;
	public float nowValue = 0;
	private float oldValue = 0;
	public SliderType type = SliderType.Horizontal;
	public GUIStyle thumbStyle;
	public event UnityAction<float> dragEvent;
	protected override void Style()
	{
		switch (type)
		{
			case SliderType.Horizontal:
				nowValue = GUI.HorizontalSlider(pos.RectPos, nowValue, minValue, maxValue, style, thumbStyle);
				break;
			case SliderType.Vertical:
				nowValue = GUI.VerticalSlider(pos.RectPos, nowValue, minValue, maxValue, style, thumbStyle);
				break;
		}
		if (oldValue != nowValue)
		{
			oldValue = nowValue;
			dragEvent?.Invoke(nowValue);
		}
	}
	protected override void NoStyle()
	{
		switch (type)
		{
			case SliderType.Horizontal:
				nowValue = GUI.HorizontalSlider(pos.RectPos, nowValue, minValue, maxValue);
				break;
			case SliderType.Vertical:
				nowValue = GUI.VerticalSlider(pos.RectPos, nowValue, minValue, maxValue);
				break;
		}
		if (oldValue != nowValue)
		{
			oldValue = nowValue;
			dragEvent?.Invoke(nowValue);
		}
	}
}
public enum SliderType
{
	Horizontal,
	Vertical
}