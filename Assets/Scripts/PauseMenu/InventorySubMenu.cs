﻿using UnityEngine;
using System.Collections;

public class InventorySubMenu : SubMenu {

	// Use this for initialization
	void Start () {
        base.Start();
	}

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Consumable Items":
                GameControl.control.whichInventory = "consumables";
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("InventoryMenu");
                break;
            case "Weapons":
                GameControl.control.whichInventory = "weapons";
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("InventoryMenu");
                break;
            case "Armor":
                GameControl.control.whichInventory = "armor";
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("InventoryMenu");
                break;
            case "Key Items":
                GameControl.control.whichInventory = "reusables";
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("InventoryMenu");
                break;
            default:
                break;
        }
    }

    void CheckForInactive()
    {
        for (int i = 0; i < contentArray.Count; i++)
        {
            if (buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
                buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover)
            {
                //if (i == selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                if (i == 0 && GameControl.control.consumables.Count == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                if (i == 1 && GameControl.control.weapons.Count == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                if (i == 2 && GameControl.control.equipment.Count == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                if (i == 3 && GameControl.control.reusables.Count == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
            }
            else if (buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
                     buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover)
            {
                
                if (i == 0 && GameControl.control.consumables.Count > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover; }
                if (i == 1 && GameControl.control.weapons.Count > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
                if (i == 2 && GameControl.control.equipment.Count > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
                if (i == 3 && GameControl.control.reusables.Count > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckForInactive();
        if (isVisible && frameDelay > 0)
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                pm.ActivateMenu();
            }
        }
        base.Update();
	}
}