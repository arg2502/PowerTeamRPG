using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSlot : MonoBehaviour {

    public Image icon;

	public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
