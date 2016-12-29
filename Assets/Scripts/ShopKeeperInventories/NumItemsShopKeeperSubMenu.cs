﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumItemsShopKeeperSubMenu : SubMenu {

	ShopKeeperMenu shopMenu;
	public Item item; // item selected
	int quantity; // number of items
	int totalPrice; // final product

	// Use this for initialization
	void Start () {
		numOfRow = 3;
		quantity = 1;
		//totalPrice = quantity * item.price;
		contentArray = new List<string>{ quantity.ToString() };
		//buttonDescription = new List<string>{"How many?"};

		base.Start ();

		shopMenu = GameObject.FindObjectOfType<ShopKeeperMenu> ();
	}
	
	// deal with the button pressed
	public override void ButtonAction(string label)
	{
		if (isActive && isVisible) {
			if (Input.GetKeyUp (KeyCode.A)) {
				// make sure quantity does not go below 1
				if (quantity > 1) {
					quantity--;
					shopMenu.dBox.currentText = shopMenu.shopKeeper.buyingText;
					//if(buttonDescription[0] != "How many?") buttonDescription [0] = "How many?";
					//print ("left");
				}
			} else if (Input.GetKeyUp (KeyCode.D)) {
				// max = amount that can be bought with total gold
				if (GameControl.control.totalGold >= (quantity + 1) * item.price) {
					quantity++;
					//print ("right");
				} else {
					shopMenu.dBox.currentText = shopMenu.shopKeeper.tooMuchText;
					//buttonDescription [0] = "You cannot afford more than that.";
					//print ("right 2");
				}
			}
		}
			
	}

	public void EnableSubMenu()
	{
		shopMenu.DeactivateMenu();
		base.EnableSubMenu();
	}

	public void DeactivateMenu()
	{
		isActive = false;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
		}
	}

	public void ActivateMenu()
	{
		frameDelay = 0.0f;
		isActive = true;
		shopMenu.descriptionText.GetComponent<Renderer>().enabled = true;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if (isVisible && frameDelay > 0)
		if (isVisible && isActive)
		{
			// update quantity - how many items are being bought
			totalPrice = quantity * item.price;
			contentArray[0] = quantity.ToString() + " x " + item.price.ToString() + " = " + totalPrice.ToString();
			ChangeText ();
			if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q))
			{
				quantity = 1;
				shopMenu.dBox.isBuying = false;
				shopMenu.dBox.currentText = "";
				shopMenu.dBox.prevText = "";
				shopMenu.ActivateMenu();
			}
		}

		PressButton (KeyCode.A);
		PressButton (KeyCode.D);

		//update which position the submenu should appear in
		//use.parentPos = buttonArray[selectedIndex].transform;
		base.Update();
	}
}
