using UnityEngine;

[CreateAssetMenu(fileName = "ColorToBuildingType", menuName = "ScriptableObjects/ColorToBuildingType", order = 1)]
public class ColorToBuildingType : ScriptableObject {

    public Color32 Color;
    public BuildingType BuildingType;

}
