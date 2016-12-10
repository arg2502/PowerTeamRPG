using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopKeeperMenu : Menu {

	ShopKeeper shopKeeper; // access to the shopkeeper
	public DialogueBoxShopKeeper dBox; // to set dialogue box's flavor text
	public GameObject descriptionText;
	NumItemsShopKeeperSubMenu subMenu;
	// Use this for initialization
	void Start () {
		contentArray = new List<string>();
		numOfRow = 6;
		shopKeeper = GetComponent<ShopKeeper> (); // both ShopKeeper & ShopKeeperMenu should be attached to the same game object
		dBox = GameObject.FindObjectOfType<DialogueBoxShopKeeper>();

		// set the content array to the list of item names
		for (int i = 0; i < shopKeeper.inventory.Count; i++) { 
			contentArray.Add(shopKeeper.inventory[i].GetComponent<Item>().name); 
		}

		base.Start();




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

		GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/NumItemsShopKeeperSubMenu"));
		subMenu = go.GetComponent<NumItemsShopKeeperSubMenu> ();
		subMenu.parentPos = buttonArray [0].transform;

		//Create the description text object
		descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		if (selectedIndex + scrollIndex < shopKeeper.inventory.Count) {
			descriptionText.GetComponent<TextMesh> ().text = FormatText (shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().description);
		}
		descriptionText.transform.position = new Vector2(camera.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

		// set selected button
		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

		// Create the appropriate sub menu
		//GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/ConsumableItemSubMenu"));
		//consumeSub = temp.GetComponent<ConsumableItemSubMenu>();
		//consumeSub.parentPos = buttonArray[selectedIndex].transform;
		//consumeSub.itemName = contentArray[selectedIndex + scrollIndex];

		//call change text method to correctly size text and avoid a certain bug
		ChangeText();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		// update the description text
		if (selectedIndex + scrollIndex < shopKeeper.inventory.Count) {
			descriptionText.GetComponent<TextMesh> ().text = FormatText (shopKeeper.inventory [selectedIndex + scrollIndex].GetComponent<Item> ().description);
			dBox.listPosition = selectedIndex + scrollIndex;
		}// + "\n\nQuantity: " + (itemList[selectedIndex + scrollIndex].quantity - itemList[selectedIndex + scrollIndex].uses); }

		PressButton(KeyCode.Space);

		if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Backspace)) { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
	
		// set the submenus position to the button you are on - no need to make more than one submenu...right?
		subMenu.parentPos = buttonArray [selectedIndex + scrollIndex].transform;


	}
	public override void ButtonAction (string label)
	{
		// set the submenu's item equal to the item of the button you selected
		// then activate the submenu
		subMenu.item = shopKeeper.inventory[selectedIndex + scrollIndex].GetComponent<Item>();
		subMenu.EnableSubMenu ();
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
}
