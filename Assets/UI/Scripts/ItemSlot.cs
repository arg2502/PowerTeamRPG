using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSlot : MonoBehaviour {

    public Image icon;
    public Text quantity;

	public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetQuantity(int number)
    {
        quantity.text = "X " + number.ToString();
    }
}
