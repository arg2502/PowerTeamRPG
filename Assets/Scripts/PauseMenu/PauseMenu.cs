﻿//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class PauseMenu : Menu {

//    protected List<string> buttonDescription;
//    //public GameObject descriptionText;

//    SpriteRenderer sr;

    
//	//public bool isActive = true; // a sub menu is not present
    

//    protected TeamSubMenu teamSub;
//    protected InventorySubMenu inventorySub;
//    protected LoadSubMenu loadSub;

//    public int SelectedIndex { get { return selectedIndex; } set { selectedIndex = value; } }

//	// Use this for initialization
//	void Start () {

//        // find the sprite renderer
//        sr = gameObject.GetComponent<SpriteRenderer>();
//        sr.enabled = false;

//        // find the player
//        player = GameObject.FindObjectOfType<characterControl>();

//        base.Start();
//        buttonArray = new GameObject[contentArray.Count];
//        buttonDescription = new List<string>();

//        buttonDescription.Add("Return to the game.");
//        buttonDescription.Add("View your team's stats, status ailments, and assign them equipment.");
//        buttonDescription.Add("Access your entire inventory.");
//        buttonDescription.Add("Save your game.");
//        buttonDescription.Add("Load a previously saved game.");
        
//        for (int i = 0; i < contentArray.Count; i++)
//        {
//            // create a button
//            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
//            buttonArray[i].name = "PauseMenuButton" + i.ToString();
//            MyButton b = buttonArray[i].GetComponent<MyButton>();
//            buttonArray[i].transform.position = new Vector2(player.transform.position.x - 9f, player.transform.position.y + (4f + b.height) + (i * -(b.height + b.height / 2)));
//            b.GetComponent<Renderer>().sortingOrder = 9900;

//            // assign text
//            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
//            b.textObject.name = "PauseMenuText" + i.ToString();
//            b.labelMesh = b.textObject.GetComponent<TextMesh>();
//            b.labelMesh.text = contentArray[i];
//            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
//            b.labelMesh.GetComponent<Renderer>().sortingOrder = 9900;

//        }

//        //create sub menus
//        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/InventorySubMenu"));
//        temp.name = "InventorySubMenu";
//        inventorySub = temp.GetComponent<InventorySubMenu>();
//        inventorySub.parentPos = buttonArray[2].transform;
//        temp = (GameObject)Instantiate(Resources.Load("Prefabs/LoadSubMenu"));
//        temp.name = "LoadSubMenu";
//        loadSub = temp.GetComponent<LoadSubMenu>();
//        loadSub.parentPos = buttonArray[4].transform;
//        temp = (GameObject)Instantiate(Resources.Load("Prefabs/TeamSubMenu"));
//        temp.name = "TeamSubMenu";
//        teamSub = temp.GetComponent<TeamSubMenu>();
//        teamSub.parentPos = buttonArray[1].transform;

//        // set selected button
//        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

//        //Create the description text object
//        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
//        descriptionText.name = "DescriptionText";
//        descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);
//        descriptionText.transform.position = new Vector2(player.transform.position.x + 3.125f, buttonArray[0].transform.position.y + 0.25f);
//        descriptionText.GetComponent<Renderer>().sortingOrder = 9900;

//        //call change text method to correctly size text and avoid a certain bug
//        ChangeText();


//		// calling this method at start called ToggleMovement on all Overworld objs
//		// while they were still being created resulting in some objs with canMove true
//		// and others with it false
//		// the following code is the Disable Pause Menu method with the ToggleMovement line
//		// commented out.
//		// A better way can be made later

//        //DisablePauseMenu(); 
//		// hide the menu until the player opens it
//		// reset the menu for the next use
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
//		selectedIndex = 0;
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

//		// hide the menu
//		isVisible = false;
//		//GameControl.control.isPaused = false;
//		sr.enabled = false;

//		// release the overworld objects
//		//if (player.canMove == false) { player.ToggleMovement(); }

//		base.DisableMenu();
//		descriptionText.GetComponent<Renderer>().enabled = false;
//		//

//        isVisible = false;
//        isActive = true;

//	}

//    public string FormatText(string str)
//    {
//        string formattedString = null;
//        int desiredLength = 40;
//        string[] wordArray = str.Split(' ');
//        int lineLength = 0;
//        foreach (string s in wordArray)
//        {
//            //if the current line plus the length of the next word and SPACE is greater than the desired line length
//            if (s.Length + 1 + lineLength > desiredLength)
//            {
//                //go to new line
//                formattedString += "\n" + s + " ";
//                //starting a new line
//                lineLength = s.Length;
//            }
//            else
//            {
//                formattedString += s + " ";
//                lineLength += s.Length + 1;
//            }
//        }
//        return formattedString;
//    }
//    public override void ButtonAction(string label)
//    {
//        switch (label)
//        {
//            case "Team Info":
//                teamSub.EnableSubMenu();
//                break;
//            case "Inventory":
//                inventorySub.EnableSubMenu();
//                break;
//            case "Save":
//                // set the current scene variable
//                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
//                GameControl.control.RecordRoom();
//                //current position is at the entrance, unless a savestatue is tagged
//                if (GameControl.control.taggedStatue == false) { GameControl.control.currentPosition = GameControl.control.areaEntrance; }
//                else { GameControl.control.currentPosition = GameControl.control.savedStatue; }
//                // Save the game
//                GameControl.control.Save();
//                break;
//            case "Load":
//                //Open the submenu for saveFiles
//                loadSub.EnableSubMenu();
//                //GameControl.control.Load();
//                break;
//            case "Exit Menu":
//                GameControl.control.isPaused = false;
//                DisablePauseMenu();
//                break;
//        }
//    }

//    public void EnablePauseMenu()
//    {
//        GameControl.control.isPaused = true;
//        sr.enabled = true;
//        base.EnableMenu();
//        descriptionText.GetComponent<Renderer>().enabled = true;
//    }

   



//    public void HighlightButton()
//    {
//        for (int i = 0; i < buttonArray.Length; i++)
//        {
//            if (i != selectedIndex && !isActive) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
//            else if (i != selectedIndex && isActive) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
//            else if (i == selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover; }
//        }
//    }

//    void CheckForInactive()
//    {
//        if(GameControl.control.consumables.Count == 0 && GameControl.control.reusables.Count == 0
//            && GameControl.control.weapons.Count == 0 && GameControl.control.equipment.Count == 0 && (buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
//                buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover)) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
//        else if ((GameControl.control.consumables.Count > 0 || GameControl.control.reusables.Count > 0
//            || GameControl.control.weapons.Count > 0 || GameControl.control.equipment.Count > 0) 
//            && (buttonArray[2].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
//    }

//	// Update is called once per frame
//	void Update () {
//        if (isVisible && isActive)
//        {
//            CheckForInactive();
//            base.Update();

//            ////update the position of the menu
//            //for (int i = 0; i < contentArray.Count; i++)
//            //{
//            //    MyButton b = buttonArray[i].GetComponent<MyButton>();
//            //    buttonArray[i].transform.position = new Vector2(player.transform.position.x - 600, player.transform.position.y + (250 + b.height) + (i * -(b.height + b.height / 2)));
//            //    b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
//            //}
//            //descriptionText.transform.position = new Vector2(player.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

//            // Update the description
//            descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);

//            PressButton(GameControl.control.selectKey);

//            if (Input.GetKeyUp(GameControl.control.backKey))
//            {
//                GameControl.control.isPaused = false;
//                DisablePauseMenu();
//            }
//        }
//        // the way to pause the game
//        if (Input.GetKeyUp(GameControl.control.pauseKey))
//        {
//            if (isVisible == false && player.canMove)
//            {
//                GameControl.control.isPaused = true;
//                isVisible = true;
//                EnablePauseMenu();
//                player.ToggleMovement();
//            }
//            else if (isVisible)
//            {
//                GameControl.control.isPaused = false;
//                DisablePauseMenu();
//            }

//        }
//        //update the position of the menu
//        for (int i = 0; i < contentArray.Count; i++)
//        {
//            MyButton b = buttonArray[i].GetComponent<MyButton>();
//            buttonArray[i].transform.position = new Vector2(player.transform.position.x - 9f, player.transform.position.y + (4f + b.height) + (i * -(b.height + b.height / 2)));
//            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
//        }
//        descriptionText.transform.position = new Vector2(player.transform.position.x + 3.125f, buttonArray[0].transform.position.y + 0.25f);
//	}
//}
