using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSlot : MonoBehaviour {

    //internal Item item;
	internal InventoryItem item;
    internal ScriptableItem s_item;
    public Image icon;
    public Text quantity;

//	public void SetItem(Item _item)
//    {
//        item = _item;
//        icon.sprite = item.sprite;
//        quantity.text = "X " + (item.quantity - item.uses);
//    }
//
//    public void UpdateQuantity()
//    {
//        quantity.text = "X " + (item.quantity - item.uses);
//    }

	public void SetItem(InventoryItem _item)
	{
		item = _item;
		icon.sprite = ItemDatabase.GetItemSprite(item.name);
		quantity.text = "X " + (item.quantity - item.uses);
	}
    
	public void UpdateQuantity()
	{
		quantity.text = "X " + (item.quantity - item.uses);
	}

    public void SetItem(ScriptableItem _s_item)
    {
        s_item = _s_item;
        icon.sprite = ItemDatabase.GetItemSprite(s_item.name);

        // we don't need quantity if we're here -- Shopkeeper
        quantity.text = "";
    }
}
