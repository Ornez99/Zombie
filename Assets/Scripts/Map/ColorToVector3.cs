using UnityEngine;

[CreateAssetMenu(fileName = "ColorToVector3", menuName = "ScriptableObjects/ColorToVector3", order = 1)]
public class ColorToVector3 : ScriptableObject {

    public Color32 Color;
    public Vector3 Vector3;

}