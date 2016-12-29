using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumItemsShopKeeperSubMenu : SubMenu {

	public ShopKeeperMenu shopMenu;
	public Item item; // item selected
	public int quantity; // number of items
	public int totalPrice; // final product
	ConfirmPurchaseShopKeeperSub sub;

	// Use this for initialization
	void Start () {
		//numOfRow = 3;
		quantity = 1;
		//totalPrice = quantity * item.price;
		contentArray = new List<string>{ "" };
		//buttonDescription = new List<string>{"How many?"};
		buttonArray = new GameObject[contentArray.Count];
		offsetPercent = 0.95f;
		// basically base.Start(), but slightly different for the different button prefab
		for (int i = 0; i < contentArray.Count; i++) {
			buttonArray[i] = (GameObject)Instantiate(Resources.Load ("Prefabs/LevelUpButtonPrefab"));
			MyButton b = buttonArray [i].GetComponent<MyButton> ();
			//buttonArray [i].transform.position = new Vector2 (parentPos.position.x + b.width, parentPos.position.y + (i * -(b.height + b.height / 2)));

			// assign text
			b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
			b.labelMesh = b.textObject.GetComponent<TextMesh>();
			b.labelMesh.text = contentArray[i];
			b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
		}
		// set selected button
		//print(contentArray.Count);
		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

		//call change text method to correctly size text and avoid a certain bug
		ChangeText();

		//Change the description text
		if (pm != null) { 
			pm.descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]); 
		}

		DisableSubMenu(); // hide the menu until the player opens it
		isVisible = false;

		// link to parent
		shopMenu = GameObject.FindObjectOfType<ShopKeeperMenu> ();

		// create submenu
		GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/ConfirmPurchaseShopKeeperSub"));
		sub = go.GetComponent<ConfirmPurchaseShopKeeperSub> ();
		sub.parentPos = buttonArray [0].transform;
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
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/lvlUpMenu/hoverDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/lvlUpMenu/activeDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
					} else {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/lvlUpMenu/hoverlvlUpButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/lvlUpMenu/activelvlUpButton", typeof(Sprite)) as Sprite;
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
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/lvlUpMenu/hoverDisabledRightlvlUpButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/lvlUpMenu/activeDisabledRightlvlUpButton", typeof(Sprite)) as Sprite;
					} else {
						buttonArray [0].GetComponent<MyButton> ().hoverTexture = Resources.Load ("Sprites/lvlUpMenu/hoverlvlUpButton", typeof(Sprite)) as Sprite;
						buttonArray [0].GetComponent<MyButton> ().activeTexture = Resources.Load ("Sprites/lvlUpMenu/activelvlUpButton", typeof(Sprite)) as Sprite;
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
