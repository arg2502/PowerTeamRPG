﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryMenu : Menu {

    public List<Item> itemList;
    public GameObject descriptionText;
    ConsumableItemSubMenu consumeSub; // a universal sub menu for all consumable items
    public bool isActive = true; // a sub menu is not present

	// Use this for initialization
	void Start () {
        contentArray = new List<string>();
        numOfRow = 7;

        // set the correct list of items
        if (GameControl.control.whichInventory == "consumables")
        {
            foreach (GameObject go in GameControl.control.consumables) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ConsumableItem>()); } }
        }
        else if (GameControl.control.whichInventory == "reusables")
        {
            foreach (GameObject go in GameControl.control.reusables) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ReusableItem>()); } }
        }
        else if (GameControl.control.whichInventory == "weapons")
        {
            foreach (GameObject go in GameControl.control.weapons) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<WeaponItem>()); } }
        }
        else if (GameControl.control.whichInventory == "armor")
        {
            foreach (GameObject go in GameControl.control.equipment) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ArmorItem>()); } }
        }

        // set the content array to the list of item names
        for (int i = 0; i < itemList.Count; i++) { contentArray.Add(itemList[i].name); }

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

        //Create the description text object
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        if (selectedIndex + scrollIndex < itemList.Count)
        { descriptionText.GetComponent<TextMesh>().text = FormatText(itemList[selectedIndex + scrollIndex].description) + "\n\nQuantity: " + itemList[selectedIndex + scrollIndex].quantity; }
        descriptionText.transform.position = new Vector2(camera.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        // Create the appropriate sub menu
        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/ConsumableItemSubMenu"));
        consumeSub = temp.GetComponent<ConsumableItemSubMenu>();
        consumeSub.parentPos = buttonArray[selectedIndex].transform;
        consumeSub.itemName = contentArray[selectedIndex + scrollIndex];

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();
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
        UpdateMenu();
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex && i < itemList.Count) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
        }
    }

    public override void ButtonAction(string label)
    {
        consumeSub.EnableSubMenu();
    }

    public void UpdateMenu()
    {
        contentArray = new List<string>();
        itemList = new List<Item>();

        // set the correct list of items
        if (GameControl.control.whichInventory == "consumables")
        {
            foreach (GameObject go in GameControl.control.consumables) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ConsumableItem>()); } }
        }
        else if (GameControl.control.whichInventory == "reusables")
        {
            foreach (GameObject go in GameControl.control.reusables) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ReusableItem>()); } }
        }
        else if (GameControl.control.whichInventory == "weapons")
        {
            foreach (GameObject go in GameControl.control.weapons) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<WeaponItem>()); } }
        }
        else if (GameControl.control.whichInventory == "armor")
        {
            foreach (GameObject go in GameControl.control.equipment) { if (go.GetComponent<Item>().quantity != go.GetComponent<Item>().uses) { itemList.Add(go.GetComponent<ArmorItem>()); } }
        }

        // set the content array to the list of item names
        for (int i = 0; i < itemList.Count; i++) { contentArray.Add(itemList[i].name); }

        // if all of the items are gone, leave the scene
        if (contentArray.Count == 0) { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); return; }

        // make sure the selected index remains in range
        if (selectedIndex >= contentArray.Count) 
        {
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled;
            selectedIndex--; 
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
        }
        //update the button labels
        for (int i = 0; i < numOfRow; i++) 
        {
            if (i < contentArray.Count) { buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = contentArray[i]; }
            else { buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = ""; }
        }

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();
    }

	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            base.Update();
            //update which position the submenu should appear in
            consumeSub.parentPos = buttonArray[selectedIndex].transform;

            //update which item is selected for sub menus
            consumeSub.itemName = contentArray[selectedIndex + scrollIndex];

            // update the description text
            if (selectedIndex + scrollIndex < itemList.Count)
            { descriptionText.GetComponent<TextMesh>().text = FormatText(itemList[selectedIndex + scrollIndex].description) + "\n\nQuantity: " + (itemList[selectedIndex + scrollIndex].quantity - itemList[selectedIndex + scrollIndex].uses); }

            PressButton(GameControl.control.selectKey);

            // Exit to the previous room
            if (Input.GetKeyUp(GameControl.control.pauseKey) || Input.GetKeyUp(GameControl.control.backKey)) { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
        }
	}
}
