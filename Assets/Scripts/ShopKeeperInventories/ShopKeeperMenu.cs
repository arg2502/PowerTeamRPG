

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopKeeperMenu : Menu {

	public ShopKeeper shopKeeper; // access to the shopkeeper
	public DialogueBoxShopKeeper dBox; // to set dialogue box's flavor text
	public GameObject descriptionText;
	public GameObject descriptionTitle;
	public GameObject goldText;
	public NumItemsShopKeeperSubMenu subMenu;
	public SellerSubMenu sellerSub;
	public string whichInventory; // tell SellerSubMenu which list to show

	// Use this for initialization
	void Start () {
		contentArray = new List<string>();
		//numOfRow = 6;
		shopKeeper = GetComponent<ShopKeeper> (); // both ShopKeeper & ShopKeeperMenu should be attached to the same game object
		dBox = GameObject.FindObjectOfType<DialogueBoxShopKeeper>();

		// set the content array 
		// Buy Menu - isSellMenu = false
		// list of item names
		if (!GameControl.control.isSellMenu) {
			numOfRow = 6;
			for (int i = 0; i < shopKeeper.inventory.Count; i++) { 
				contentArray.Add (shopKeeper.inventory [i].GetComponent<Item> ().name); 
			}
			shopKeeper.howMuchText = shopKeeper.howMuchBuying;
			shopKeeper.tooMuchText = shopKeeper.tooMuchBuying;
			shopKeeper.confirmationText = shopKeeper.confirmationBuying;
			shopKeeper.receiptText = shopKeeper.receiptBuying;

		}
		// Sell Menu - isSellMenu = true
		// list of inventory
		else{
			numOfRow = 4;
			contentArray.Add ("Consumable Items");
			contentArray.Add ("Weapons");
			contentArray.Add ("Armor");
			contentArray.Add ("Key Items");

			shopKeeper.howMuchText = shopKeeper.howMuchSelling;
			shopKeeper.tooMuchText = shopKeeper.tooMuchSelling;
			shopKeeper.confirmationText = shopKeeper.confirmationSelling;
			shopKeeper.receiptText = shopKeeper.receiptSelling;

		}
		base.Start();

		//isActive = true;

		// create the buttons
		for (int i = 0; i < numOfRow; i++)
		{
			// create a button
			buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
			MyButton b = buttonArray[i].GetComponent<MyButton>();
			buttonArray[i].transform.position = new Vector2(camera.transform.position.x - 600, camera.transform.position.y + (250 + b.height) + (i * -(b.height + b.height / 2)));

			// assign text
			b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
			b.labelMesh = b.textObject.GetComponent<TextMesh>();
			// if there are not as many items as there are buttons
			if (i >= contentArray.Count)
			{
				buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = "";
				buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled;
			}
			else
			{
				b.labelMesh.text = contentArray[i];
			}
			b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);



		}

		// sub menus
		GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/NumItemsShopKeeperSubMenu"));
		subMenu = go.GetComponent<NumItemsShopKeeperSubMenu> ();
		subMenu.parentPos = buttonArray [0].transform;



		//Create the description text object
		descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));

		descriptionText.transform.position = new Vector2(camera.transform.position.x + 200, buttonArray[0].transform.position.y + 15);



		// state how much gold player has
		goldText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		goldText.GetComponent<TextMesh> ().text = "Gold: " + GameControl.control.totalGold;
		goldText.transform.position = new Vector2 (buttonArray [0].transform.position.x - buttonArray[0].GetComponent<MyButton>().width/2, buttonArray [0].transform.position.y + buttonArray [0].GetComponent<MyButton> ().height*2);

		descriptionTitle = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		descriptionTitle.GetComponent<TextMesh> ().text = "Item Description";
		descriptionTitle.transform.position = new Vector2 (descriptionText.transform.position.x, goldText.transform.position.y);



		if (!GameControl.control.isSellMenu) {
			if (selectedIndex + scrollIndex < shopKeeper.inventory.Count) {
				descriptionText.GetComponent<TextMesh> ().text = 
					FormatText ("Price: " + shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().price +
						"\n\n" + shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			}
		} else {
			descriptionText.GetComponent<TextMesh> ().GetComponent<Renderer> ().enabled = false;
			descriptionTitle.GetComponent<TextMesh> ().GetComponent<Renderer> ().enabled = false;
		}

		// set correct button states (and menu to isActive)
		ActivateMenu ();

		//call change text method to correctly size text and avoid a certain bug
		ChangeText();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			base.Update ();

			// update the description text
			if (!GameControl.control.isSellMenu) {
				if (selectedIndex + scrollIndex < shopKeeper.inventory.Count) {
					descriptionText.GetComponent<TextMesh> ().text = 
					FormatText ("Price: " + shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().price +
					"\n\n" + shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().description);
					dBox.listPosition = selectedIndex + scrollIndex;
				}
			} else {
				descriptionText.GetComponent<TextMesh> ().GetComponent<Renderer> ().enabled = false;
				descriptionTitle.GetComponent<TextMesh> ().GetComponent<Renderer> ().enabled = false;
				if(!dBox.isBuying) dBox.currentText = shopKeeper.sellingText;
			}
			PressButton (GameControl.control.selectKey);

			if (Input.GetKeyUp (GameControl.control.pauseKey) || Input.GetKeyUp (GameControl.control.backKey)) {
				UnityEngine.SceneManagement.SceneManager.LoadScene (GameControl.control.currentScene);
			}
	
			// set the submenus position to the button you are on - no need to make more than one submenu...right?
			if (!GameControl.control.isSellMenu) {
				subMenu.parentPos = buttonArray [selectedIndex + scrollIndex].transform;
			} else {
				//sellerSub.parentPos = buttonArray [selectedIndex + scrollIndex].transform;
			}

			// set dialogue isBuying to false upon moving
			if (Input.GetKeyUp (GameControl.control.upKey) || Input.GetKeyUp (GameControl.control.downKey)) {
				dBox.isBuying = false;
			}

		}

	}
	public override void ButtonAction (string label)
	{
		// set the submenu's item equal to the item of the button you selected
		// then activate the submenu
		if (!GameControl.control.isSellMenu) {
			dBox.isBuying = true;
			dBox.currentText = shopKeeper.howMuchText;
			dBox.prevPosition = -1; // so it start's different - triggers a change
			subMenu.item = shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ();
			subMenu.EnableSubMenu ();
		} else {
			whichInventory = buttonArray [selectedIndex].GetComponent<MyButton> ().labelMesh.text;

			// create seller sub menu now
			GameObject sellerGo = (GameObject)Instantiate (Resources.Load ("Prefabs/SellerSubMenuPrefab"));
			sellerSub = sellerGo.GetComponent<SellerSubMenu> ();
			//sellerSub.parent = this;
			sellerSub.parentPos = buttonArray [selectedIndex + scrollIndex].transform;

			//sellerSub.EnableSubMenu ();
		}
	}
	public string FormatText(string str)
	{
		string formattedString = null;
		int desiredLength = 40;
		string[] wordArray = str.Split(' ');
		int lineLength = 0;
		foreach (string s in wordArray)
		{
			//if the current line plus the length of the next word and SPACE is greater than the desired line length
			if (s.Length + 1 + lineLength > desiredLength)
			{
				//go to new line
				formattedString += "\n" + s + " ";
				//starting a new line
				lineLength = s.Length;
			}
			else
			{
				formattedString += s + " ";
				lineLength += s.Length + 1;
			}
		}
		return formattedString;
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
		isActive = true;

		if (!GameControl.control.isSellMenu) {
			// see which items are available to buy - code here based on how battle menu deactivates the skills and spells
			for (int i = 0; i < buttonArray.Length; i++) {
				for (int j = 0; j < shopKeeper.inventory.Count; j++) {
				

					if (buttonArray [i].GetComponent<MyButton> ().textObject.GetComponent<TextMesh> ().text == shopKeeper.inventory [j].GetComponent<Item> ().name) {
						// set to normal when coming back from sub menu first
						buttonArray [i].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.normal;

						if (GameControl.control.totalGold < shopKeeper.inventory [j].GetComponent<Item> ().price) {
							buttonArray [i].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactive;
						}
					}
				}
			}
		} else {

			// set back to normal first
			for (int i = 0; i < buttonArray.Length; i++) {
				buttonArray [i].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.normal;
			}

			// check if the player still has that type of item
			if (GameControl.control.consumables.Count <= 0) {
				buttonArray [0].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactive;
			} if (GameControl.control.weapons.Count <= 0) {
				buttonArray [1].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactive;
			} if (GameControl.control.equipment.Count <= 0) {
				buttonArray [2].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactive;
			} if (GameControl.control.reusables.Count <= 0) {
				buttonArray [3].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactive;
			} 
		}

		// set selected button
		if (buttonArray [selectedIndex].GetComponent<MyButton> ().state == MyButton.MyButtonTextureState.normal) {
			buttonArray [selectedIndex].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.hover;
		} else if (buttonArray[selectedIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive) {
			buttonArray [selectedIndex].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.inactiveHover;			
		}
	}



}
