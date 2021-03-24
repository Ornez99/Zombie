using UnityEngine;

[CreateAssetMenu(fileName = "ColorToColor", menuName = "ScriptableObjects/ColorToColor", order = 1)]
public class ColorToColor : ScriptableObject {

    public Color32 InputColor;
    public Color32 OutputColor;

}