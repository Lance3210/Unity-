using UnityEngine;

public class MyGUITexture : MyGUIControlBase {
	[Header("Image Property")]
	public ScaleMode scaleMode = ScaleMode.StretchToFill;
	public Color color = Color.white;
	public bool alphaBlend = true;
	public float imageAspect;
	public float borderWidth;
	public float borderRadius;

	protected override void Style() {
		GUI.DrawTexture(pos.RectPos, content.image, scaleMode, alphaBlend, imageAspect, color, borderWidth, borderRadius);
	}
	protected override void NoStyle() {
		GUI.DrawTexture(pos.RectPos, content.image, scaleMode, alphaBlend, imageAspect, color, borderWidth, borderRadius);
	}
}
