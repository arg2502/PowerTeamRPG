using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfirmPurchaseShopKeeperSub : SubMenu {

	NumItemsShopKeeperSubMenu parent;


	// Use this for initialization
	void Start () {
		parent = GameObject.FindObjectOfType<NumItemsShopKeeperSubMenu> ();
		contentArray = new List<string> { "Yes", "No" };
		buttonPrefab = "Prefabs/SmallButtonPrefab";
		offsetPercent = 1.5f;
		base.Start ();
	
	}
	public void EnableSubMenu()
	{
		parent.DeactivateMenu();

		parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.confirmationText;

		base.EnableSubMenu();
	}

	// deal with the button pressed
	public override void ButtonAction(string label)
	{
		// yes to purchase
		// add the items
		// remove the gold
		// return to main buying window
		if (label == "Yes") {

			// buying
			if (!GameControl.control.isSellMenu) {
				// add the items
				for (int i = 0; i < parent.quantity; i++) {
					GameObject temp = (GameObject)Instantiate (parent.item.gameObject);
                    temp.name = "ParentItem";
					GameControl.control.AddItem (temp);
				}

				// remove the gold
				GameControl.control.totalGold -= parent.totalPrice;
				parent.shopMenu.goldText.GetComponent<TextMesh> ().text = "Gold: " + GameControl.control.totalGold;
			} else {
				// selling
				// remove items
				if (parent.shopMenu.whichInventory == "Consumables") {
					if (parent.item.quantity - parent.quantity > 0) {
						//parent.item.quantity -= parent.quantity; // this may not affect the actual item, just the menu's version. WE'll see
						GameControl.control.consumables[parent.sellerParent.inventoryPos].GetComponent<Item>().quantity -= parent.quantity;
					} else {
						// remove item
						GameControl.control.consumables.RemoveAt(parent.sellerParent.inventoryPos);
					}
				}
				else if (parent.shopMenu.whichInventory == "Weapons") {
					if (parent.item.quantity - parent.quantity > 0) {
						//parent.item.quantity -= parent.quantity; // this may not affect the actual item, just the menu's version. WE'll see
						GameControl.control.weapons[parent.sellerParent.inventoryPos].GetComponent<Item>().quantity -= parent.quantity;
					} else {
						// remove item
						GameControl.control.weapons.RemoveAt(parent.sellerParent.inventoryPos);
					}
				}
				else if (parent.shopMenu.whichInventory == "Armor") {
					if (parent.item.quantity - parent.quantity > 0) {
						//parent.item.quantity -= parent.quantity; // this may not affect the actual item, just the menu's version. WE'll see
						GameControl.control.equipment[parent.sellerParent.inventoryPos].GetComponent<Item>().quantity -= parent.quantity;
					} else {
						// remove item
						GameControl.control.equipment.RemoveAt(parent.sellerParent.inventoryPos);
					}
				}
				else if (parent.shopMenu.whichInventory == "Key Items") {
					if (parent.item.quantity - parent.quantity > 0) {
						//parent.item.quantity -= parent.quantity; // this may not affect the actual item, just the menu's version. WE'll see
						GameControl.control.reusables[parent.sellerParent.inventoryPos].GetComponent<Item>().quantity -= parent.quantity;
					} else {
						// remove item
						GameControl.control.reusables.RemoveAt(parent.sellerParent.inventoryPos);
					}
				}


				// add gold
				GameControl.control.totalGold += parent.totalPrice;
				parent.shopMenu.goldText.GetComponent<TextMesh> ().text = "Gold: " + GameControl.control.totalGold;
			}

			// after purchase, close all submenus
			parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.receiptText;
			parent.ActivateMenu ();
			DisableSubMenu ();

			if (!GameControl.control.isSellMenu) {
				parent.shopMenu.ActivateMenu ();
				parent.DisableSubMenu ();
			} else {
				parent.sellerParent.ActivateMenu ();
				parent.DisableSubMenu ();
				parent.sellerParent.parent.ActivateMenu ();
				parent.sellerParent.DisableSubMenu ();
			}
		}

		// no to purchase
		// return to numItems submenu
		else if (label == "No") {
			parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.howMuchText;
			parent.ActivateMenu ();
			DisableSubMenu ();
		}

	}

	// Update is called once per frame
	void Update () {
		if (isVisible && frameDelay > 0) 
		{
			//UpdateStatChanges();
			//parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
			//if (Input.GetKeyUp(GameControl.control.upKey) || Input.GetKeyUp(GameControl.control.downKey)) { SetStatChanges(GameControl.control.heroList[selectedIndex]); InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); }
			if (Input.GetKeyUp(GameControl.control.backKey) || Input.GetKeyUp(GameControl.control.pauseKey)) 
			{
				parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.howMuchText;
				parent.ActivateMenu();
			}
		}

		base.Update();
	}
}
