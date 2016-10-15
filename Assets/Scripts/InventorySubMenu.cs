using UnityEngine;
using System.Collections;

public class InventorySubMenu : SubMenu {

	// Use this for initialization
	void Start () {
        base.Start();
	}

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Consumable Items":
                break;
            case "Weapons":
                break;
            case "Armor":
                break;
            case "Key Items":
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
