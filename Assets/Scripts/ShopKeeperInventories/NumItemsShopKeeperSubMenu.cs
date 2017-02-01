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
	public SellerSubMenu sellerParent;


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
		sellerParent = GameObject.FindObjectOfType<SellerSubMenu>();

		// create submenu
		GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/ConfirmPurchaseShopKeeperSub"));
		sub = go.GetComponent<ConfirmPurchaseShopKeeperSub> ();
		sub.parentPos = buttonArray [0].transform;


		// total cost of purchase
		costText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		costText.GetComponent<TextMesh>().GetComponent<Renderer> ().enabled = false;
		costText.GetComponent<TextMesh> ().text = "";
		costText.transform.position = new Vector2 (shopMenu.descriptionText.transform.position.x - (buttonArray [0].GetComponent<MyButton> ().width*1.3f), shopMenu.goldText.transform.position.y);

		costTitle = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		costTitle.GetComponent<TextMesh>().GetComponent<Renderer> ().enabled = false;
		costTitle.GetComponent<TextMesh> ().text = "Total: ";
		costTitle.transform.position = new Vector2 (shopMenu.descriptionTitle.transform.position.x - (buttonArray [0].GetComponent<MyButton> ().width*2.0f), shopMenu.goldText.transform.position.y);
	}
	
	// deal with the button pressed
	public override void ButtonAction(string label)
	{
		if (isActive && isVisible) {
			if (Input.GetKeyUp (GameControl.control.leftKey)) {
				// make sure quantity does not go below 1
				if (quantity > 1) {
					quantity--;
					shopMenu.dBox.currentText = shopMenu.shopKeeper.howMuchText;

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
			} else if (Input.GetKeyUp (GameControl.control.rightKey)) {
				if (!GameControl.control.isSellMenu) {
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
				} else {
					if (item.quantity >= (quantity + 1)) {
						quantity++;

						if (item.quantity < (quantity + 1)) {
							buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledRightQuantityButton", typeof(Sprite)) as Sprite;
							buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledRightQuantityButton", typeof(Sprite)) as Sprite;

						} else {
							buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverQuantityButton", typeof(Sprite)) as Sprite;
							buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeQuantityButton", typeof(Sprite)) as Sprite;

						}
					} else {
						shopMenu.dBox.currentText = shopMenu.shopKeeper.tooMuchText;
					}
				}
			}
			else if(Input.GetKeyUp(GameControl.control.selectKey))
			{
				sub.EnableSubMenu ();
			}
		}
			
	}

	public void EnableSubMenu()
	{
		quantity = 1;

		if (!GameControl.control.isSellMenu) {
			shopMenu.DeactivateMenu ();

			// set button to both disabled if quantity can only be 1
			if (GameControl.control.totalGold < (quantity + 1) * item.price) {
				buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledBothQuantityButton", typeof(Sprite)) as Sprite;
				buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledBothQuantityButton", typeof(Sprite)) as Sprite;
			} else {
				buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
				buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
			}

		} else {
			sellerParent = GameObject.FindObjectOfType<SellerSubMenu>();
			sellerParent.DeactivateMenu ();

			// set button to both disabled if quantity can only be 1
			if (item.quantity < (quantity + 1)) {
				buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledBothQuantityButton", typeof(Sprite)) as Sprite;
				buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledBothQuantityButton", typeof(Sprite)) as Sprite;
			} else {
				buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/hoverDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
				buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/shopkeeperQuantityButtons/activeDisabledLeftQuantityButton", typeof(Sprite)) as Sprite;
			}
		}



		// set appropriate text
		if (!GameControl.control.isSellMenu) {
			totalPrice = quantity * item.price;
		} else {
			totalPrice = quantity * sellerParent.sellingPrice;
		}
		costText.GetComponent<TextMesh> ().text = totalPrice.ToString ();
		contentArray [0] = quantity.ToString ();
		ChangeText ();

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
		if (isVisible && isActive && frameDelay > 0) {

			// update quantity - how many items are being bought
			if (!GameControl.control.isSellMenu) {
				totalPrice = quantity * item.price;
			} else {
				totalPrice = quantity * sellerParent.sellingPrice;
			}
			costText.GetComponent<TextMesh> ().text = totalPrice.ToString ();
			contentArray [0] = quantity.ToString ();
			ChangeText ();

			if (Input.GetKeyUp (GameControl.control.backKey) || Input.GetKeyUp (GameControl.control.pauseKey)) {
				quantity = 1;
				shopMenu.dBox.isBuying = false;
				shopMenu.dBox.currentText = "";
				shopMenu.dBox.prevText = "";

				if (!GameControl.control.isSellMenu) {
					shopMenu.ActivateMenu ();
				} else {
					sellerParent.ActivateMenu ();
				}
			}
			PressButton (GameControl.control.leftKey);
			PressButton (GameControl.control.rightKey);


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
