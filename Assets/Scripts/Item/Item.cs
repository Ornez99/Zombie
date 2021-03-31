using UnityEngine;


public abstract class Item : ScriptableObject {

    [SerializeField]
    private string itemName = default;
    [SerializeField]
    private Sprite itemSprite = default;
    [SerializeField]
    private bool useable = default;
    [SerializeField]
    private bool equipable = default;

    public string ItemName { get => itemName; set => itemName = value; }
    public Sprite ItemSprite { get => itemSprite; set => itemSprite = value; }
    public bool Useable { get => useable; set => useable = value; }
    public bool Equipable { get => equipable; set => equipable = value; }
    public virtual bool IsEquipped { get => false;}

    public virtual void Use(Unit unit) {
        Debug.Log($"{itemName} does not override Use.");
    }

    public virtual void Equip(Unit unit) {
        Debug.Log($"{itemName} does not override Equip.");
    }

    public virtual void Drop(Unit unit) {
        Player.Instance.CreateItemOnGround(unit.transform.position, this);
        unit.Equipment.RemoveItem(this);
    }

}
