using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumItemsShopKeeperSubMenu : SubMenu {

	ShopKeeperMenu shopMenu;
	public Item item; // item selected
	int quantity; // number of items

	// Use this for initialization
	void Start () {
		numOfRow = 1;
		contentArray = new List<string> { "0" };
		base.Start ();

		shopMenu = GameObject.FindObjectOfType<ShopKeeperMenu> ().GetComponent<ShopKeeperMenu> ();
		pm = shopMenu;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
