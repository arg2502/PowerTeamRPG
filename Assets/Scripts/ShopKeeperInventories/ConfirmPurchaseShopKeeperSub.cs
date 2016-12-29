using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfirmPurchaseShopKeeperSub : SubMenu {

	NumItemsShopKeeperSubMenu parent;


	// Use this for initialization
	void Start () {
		parent = GameObject.FindObjectOfType<NumItemsShopKeeperSubMenu> ();
		contentArray = new List<string> { "Yes", "No" };

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

			// add the items
			for (int i = 0; i < parent.quantity; i++) {
				GameObject temp = (GameObject)Instantiate(parent.item.gameObject);
				GameControl.control.AddItem (temp);
			}

			// remove the gold
			GameControl.control.totalGold -= parent.totalPrice;
			parent.shopMenu.goldText.GetComponent<TextMesh> ().text = "Gold: " + GameControl.control.totalGold;

			// after purchase, close all submenus
			parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.receiptText;
			parent.ActivateMenu ();
			DisableSubMenu ();
			parent.shopMenu.ActivateMenu ();
			parent.DisableSubMenu ();
		}

		// no to purchase
		// return to numItems submenu
		else if (label == "No") {
			parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.buyingText;
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
			//if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) { SetStatChanges(GameControl.control.heroList[selectedIndex]); InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); }
			if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q)) 
			{
				parent.shopMenu.dBox.currentText = parent.shopMenu.shopKeeper.buyingText;
				parent.ActivateMenu();
			}
		}

		base.Update();
	}
}
