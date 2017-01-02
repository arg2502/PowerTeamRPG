using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SellerSubMenu : SubMenu {

	//ShopKeeper shopKeep;
	public ShopKeeperMenu parent;
	NumItemsShopKeeperSubMenu child;
	public int sellingPrice;
	public int inventoryPos; // to keep track of where player is inside inventory when confirming transaction

	// Use this for initialization
	void Start () {
		//shopKeep = GetComponent<ShopKeeper> ();
		parent = GameObject.FindObjectOfType<ShopKeeperMenu> ();
		child = parent.subMenu;
		contentArray = new List<string> ();
		sellingPrice = 0;
		// set up content array based on shopkeeper menu choice
		if (parent.whichInventory == "Consumable Items") {
			for (int i = 0; i < GameControl.control.consumables.Count; i++) {
				contentArray.Add (GameControl.control.consumables [i].GetComponent<Item>().name);
			}
		} else if (parent.whichInventory == "Weapons") {
			for (int i = 0; i < GameControl.control.weapons.Count; i++) {
				contentArray.Add (GameControl.control.weapons [i].GetComponent<Item>().name);
			}
		} else if (parent.whichInventory == "Armor") {
			for (int i = 0; i < GameControl.control.equipment.Count; i++) {
				contentArray.Add (GameControl.control.equipment [i].GetComponent<Item>().name);
			}
		} else {
			for (int i = 0; i < GameControl.control.reusables.Count; i++) {
				contentArray.Add (GameControl.control.reusables [i].GetComponent<Item>().name);
			}
		}

		base.Start ();
		EnableSubMenu ();
	
	}

	public void EnableSubMenu()
	{
		parent.DeactivateMenu ();
		base.EnableSubMenu ();
	}
	public void ActivateMenu()
	{
		frameDelay = 0.0f;
		isActive = true;

		for (int i = 0; i < buttonArray.Length; i++)
		{
			if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
		}
	}
	public override void ButtonAction (string label)
	{
		parent.dBox.isBuying = true;
		parent.dBox.currentText = parent.shopKeeper.howMuchText;
		parent.dBox.prevPosition = -1; // so it start's different - triggers a change
		child.parentPos = buttonArray [selectedIndex].transform;

		// determine which inventory for the item
		if (parent.whichInventory == "Consumable Items") {
			child.item = GameControl.control.consumables [selectedIndex + scrollIndex].GetComponent<Item> ();
		}
		else if (parent.whichInventory == "Weapons") {
			child.item = GameControl.control.weapons [selectedIndex + scrollIndex].GetComponent<Item> ();
		}
		else if (parent.whichInventory == "Armor") {
			child.item = GameControl.control.equipment [selectedIndex + scrollIndex].GetComponent<Item> ();
		}
		else if (parent.whichInventory == "Key Items") {
			child.item = GameControl.control.reusables [selectedIndex + scrollIndex].GetComponent<Item> ();
		}
		inventoryPos = selectedIndex + scrollIndex;
		child.EnableSubMenu ();
	}

	// Update is called once per frame
	void Update () {
		if (isActive && frameDelay > 0) {
			if (Input.GetKeyUp (KeyCode.Q) || Input.GetKeyUp (KeyCode.Backspace)) {
				parent.ActivateMenu ();
			}
			ChangeText ();
			parent.dBox.currentText = parent.shopKeeper.sellingText;
		}
		child.parentPos = buttonArray [selectedIndex].transform;


		if (isVisible) {
			// determine selling price
			if (parent.whichInventory == "Consumable Items") {
				//print (GameControl.control.consumables [selectedIndex + scrollIndex].GetComponent<Item> ().price);
				//print(parent.shopKeeper.consumablesPerc);
				sellingPrice = (int)(GameControl.control.consumables [selectedIndex + scrollIndex].GetComponent<Item> ().price * parent.shopKeeper.consumablesPerc);
				parent.descriptionText.GetComponent<TextMesh> ().text = FormatText("Price: " + sellingPrice + "\n" + "Quantity: " 
					+ GameControl.control.consumables [selectedIndex + scrollIndex].GetComponent<Item> ().quantity 
					+"\n\n" + GameControl.control.consumables [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			}
			else if (parent.whichInventory == "Weapons") {
				sellingPrice = (int)(GameControl.control.weapons [selectedIndex + scrollIndex].GetComponent<Item> ().price * parent.shopKeeper.weaponsPerc);
				parent.descriptionText.GetComponent<TextMesh> ().text = FormatText("Price: " + sellingPrice + "\n" + "Quantity: " 
					+ GameControl.control.weapons [selectedIndex + scrollIndex].GetComponent<Item> ().quantity 
					+"\n\n" + GameControl.control.weapons [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			}
			else if (parent.whichInventory == "Armor") {
				sellingPrice = (int)(GameControl.control.equipment [selectedIndex + scrollIndex].GetComponent<Item> ().price * parent.shopKeeper.equipPerc);
				parent.descriptionText.GetComponent<TextMesh> ().text = FormatText("Price: " + sellingPrice + "\n" + "Quantity: " 
					+ GameControl.control.equipment [selectedIndex + scrollIndex].GetComponent<Item> ().quantity 
					+"\n\n" + GameControl.control.equipment [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			}
			else if (parent.whichInventory == "Key Items") {
				sellingPrice = (int)(GameControl.control.reusables [selectedIndex + scrollIndex].GetComponent<Item> ().price * parent.shopKeeper.reusePerc);
				parent.descriptionText.GetComponent<TextMesh> ().text = FormatText("Price: " + sellingPrice + "\n" + "Quantity: " 
					+ GameControl.control.reusables [selectedIndex + scrollIndex].GetComponent<Item> ().quantity 
					+"\n\n" + GameControl.control.reusables [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			}

			// turn on total cost text if off
			if (parent.descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == false
				&& parent.descriptionTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == false) {
				parent.descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
				parent.descriptionTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;

			}
		}

		else {
			// turn off total cost text if on
			if (parent.descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == true
				&& parent.descriptionTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == true) {
				parent.descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
				parent.descriptionTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
			}
		}
		base.Update ();

	
	}
}
