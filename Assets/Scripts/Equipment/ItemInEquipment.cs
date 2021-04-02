[System.Serializable]
public class ItemInEquipment
{
    private Unit itemOwner;
    private Item itemReference;
    private bool isEquipped;

    public Item ItemReference { get => itemReference; }
    public Unit ItemOwner { get => itemOwner; }
    public bool IsEquipped { get => isEquipped; set => isEquipped = value; }

    public ItemInEquipment(Unit itemOwner, Item itemReference)
    {
        this.itemOwner = itemOwner;
        this.itemReference = itemReference;
    }

    public void Drop(ItemInEquipment itemInEquipment)
    {
        Player.Instance.CreateItemOnGround(itemOwner.transform.position, itemReference);
        itemOwner.Equipment.RemoveItem(itemInEquipment);
        itemOwner.EquipmentUI.UpdateItemsUI(itemOwner.Equipment);
    }
}
