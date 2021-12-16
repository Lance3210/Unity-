using UnityEngine;

[System.Serializable]
public class MyGUIPos
{
	public Vector2 offset;
	public float w, h;
	public AlignType screenAlign;
	public AlignType selfAlign;

	private Rect finalRect;
	private float selfX, selfY;
	private float screenX, screenY;
	public Rect RectPos {
		get {
			ResolutionAdaptation();
			return finalRect;
		}
	}
	private void AlignAtScreen(AlignType alignType)
	{
		switch (alignType)
		{
			case AlignType.TopLeft:
				screenX = 0;
				screenY = 0;
				break;
			case AlignType.TopCenter:
				screenX = Screen.width / 2;
				screenY = 0;
				break;
			case AlignType.TopRight:
				screenX = Screen.width;
				screenY = 0;
				break;
			case AlignType.MiddleLeft:
				screenX = 0;
				screenY = Screen.height / 2;
				break;
			case AlignType.MiddleCenter:
				screenX = Screen.width / 2;
				screenY = Screen.height / 2;
				break;
			case AlignType.MiddleRight:
				screenX = Screen.width;
				screenY = Screen.height / 2;
				break;
			case AlignType.BottomLeft:
				screenX = 0;
				screenY = Screen.height;
				break;
			case AlignType.BottomCenter:
				screenX = Screen.width / 2;
				screenY = Screen.height;
				break;
			case AlignType.BottomRight:
				screenX = Screen.width;
				screenY = Screen.height;
				break;
		}
	}
	private void AlignAtSelf(AlignType alignType)
	{
		switch (alignType)
		{
			case AlignType.TopLeft:
				selfX = 0;
				selfY = 0;
				break;
			case AlignType.TopCenter:
				selfX = w / 2;
				selfY = 0;
				break;
			case AlignType.TopRight:
				selfX = w;
				selfY = 0;
				break;
			case AlignType.MiddleLeft:
				selfX = 0;
				selfY = h / 2;
				break;
			case AlignType.MiddleCenter:
				selfX = w / 2;
				selfY = h / 2;
				break;
			case AlignType.MiddleRight:
				selfX = w;
				selfY = h / 2;
				break;
			case AlignType.BottomLeft:
				selfX = 0;
				selfY = h;
				break;
			case AlignType.BottomCenter:
				selfX = w / 2;
				selfY = h;
				break;
			case AlignType.BottomRight:
				selfX = w;
				selfY = h;
				break;
		}
	}
	private void ResolutionAdaptation()
	{
		AlignAtSelf(selfAlign);
		AlignAtScreen(screenAlign);
		finalRect.x = screenX - selfX + offset.x;
		finalRect.y = screenY - selfY + offset.y;
		finalRect.width = w;
		finalRect.height = h;
	}
}
public enum AlignType
{
	TopLeft,
	TopCenter,
	TopRight,
	MiddleLeft,
	MiddleCenter,
	MiddleRight,
	BottomLeft,
	BottomCenter,
	BottomRight
}
