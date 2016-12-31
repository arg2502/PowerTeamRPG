using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumItemsShopKeeperSubMenu : SubMenu {

	public ShopKeeperMenu shopMenu;
	public Item item; // item selected
	public int quantity; // number of items
	public int totalPrice; // final product
	public GameObject costText;
	public GameObject costTitle;
	ConfirmPurchaseShopKeeperSub sub;

	// Use this for initialization
	void Start () {
		//numOfRow = 3;
		quantity = 1;
		//totalPrice = quantity * item.price;
		contentArray = new List<string>{ "" };
		//buttonDescription = new List<string>{"How many?"};
		buttonArray = new GameObject[contentArray.Count];
		offsetPercent = 1.2f;
		buttonPrefab = "Prefabs/QuantityButtonPrefab";
		base.Start ();

		// link to parent
		shopMenu = GameObject.FindObjectOfType<ShopKeeperMenu> ();

		// create submenu
		GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/ConfirmPurchaseShopKeeperSub"));
		sub = go.GetComponent<ConfirmPurchaseShopKeeperSub> ();
		sub.parentPos = buttonArray [0].transform;


		// total cost of purchase
		costText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		costText.GetComponent<TextMesh>().GetComponent<Renderer> ().enabled = false;
		costText.GetComponent<TextMesh> ().text = "";
		costText.transform.position = new Vector2 (shopMenu.descriptionText.transform.position.x - (buttonArray [0].GetComponent<MyButton> ().width*1.3f), shopMenu.descriptionText.transform.position.y);

		costTitle = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		costTitle.GetComponent<TextMesh>().GetComponent<Renderer> ().enabled = false;
		costTitle.GetComponent<TextMesh> ().text = "Total Cost";
		costTitle.transform.position = new Vector2 (costText.transform.position.x, shopMenu.goldText.transform.position.y);
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

					// set appropriate button textures
					if (quantity == 1) {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
					} else {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverQuantityButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeQuantityButton", typeof(Sprite)) as Sprite;
					}
				} else {
					quantity = 1;

				}
			} else if (Input.GetKeyUp (KeyCode.D)) {
				// max = amount that can be bought with total gold
				if (GameControl.control.totalGold >= (quantity + 1) * item.price) {
					quantity++;

					// set appropriate button textures
					if (GameControl.control.totalGold < (quantity + 1) * item.price) {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledRightQuantityButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledRightQuantityButton", typeof(Sprite)) as Sprite;
					} else {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverQuantityButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeQuantityButton", typeof(Sprite)) as Sprite;
					}

				} else {
					shopMenu.dBox.currentText = shopMenu.shopKeeper.tooMuchText;
					//buttonDescription [0] = "You cannot afford more than that.";
					//print ("right 2");
				}
			}
			else if(Input.GetKeyUp(KeyCode.Space))
			{
				sub.EnableSubMenu ();
			}
		}
			
	}

	public void EnableSubMenu()
	{
		shopMenu.DeactivateMenu();
		quantity = 1;
		buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
		buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
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
		if (isVisible && isActive) {
			// update quantity - how many items are being bought
			totalPrice = quantity * item.price;
			contentArray [0] = quantity.ToString ();
			costText.GetComponent<TextMesh> ().text = totalPrice.ToString ();
			ChangeText ();
			if (Input.GetKeyUp (KeyCode.Backspace) || Input.GetKeyUp (KeyCode.Q)) {
				quantity = 1;
				shopMenu.dBox.isBuying = false;
				shopMenu.dBox.currentText = "";
				shopMenu.dBox.prevText = "";
				shopMenu.ActivateMenu ();
			}
			PressButton (KeyCode.A);
			PressButton (KeyCode.D);

		} 

		if (isVisible) {
			// turn on total cost text if off
			if (costText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == false
				&& costTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == false) {
				costText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
				costTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
			}
		}

		else {
			// turn off total cost text if on
			if (costText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == true
				&& costTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled == true) {
				costText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
				costTitle.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
			}
		}



		//update which position the submenu should appear in
		//use.parentPos = buttonArray[selectedIndex].transform;
		base.Update();
	}
}
