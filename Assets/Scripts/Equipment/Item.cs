using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject {

    [SerializeField]
    private string itemName = "";
    [SerializeField]
    private Sprite itemSprite;

    public string ItemName { get => itemName; }
    public Sprite ItemSprite { get => itemSprite; }

}
