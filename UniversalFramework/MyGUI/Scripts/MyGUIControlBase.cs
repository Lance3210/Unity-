using UnityEngine;

[System.Serializable]
public abstract class MyGUIControlBase : MonoBehaviour
{
    public MyGUIPos pos;
    public GUIContent content;
    public bool isOpenStyle;
    public GUIStyle style;

    public void DrawMyGUI()
    {
        if (isOpenStyle)
            Style();
        else
            NoStyle();
    }

    protected abstract void Style();
    protected abstract void NoStyle();
}
