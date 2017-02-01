using UnityEngine;
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
		for (int i = 0; i < numOfRow; i++)
        {
            if (buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
                buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover)
            {
                //if (i == selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                int counter = 0;
                for (int j = 0; j < GameControl.control.consumables.Count; j++)
                { if (GameControl.control.consumables[j].GetComponent<Item>().uses != GameControl.control.consumables[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 0 && counter == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                counter = 0;
                for (int j = 0; j < GameControl.control.weapons.Count; j++)
                { if (GameControl.control.weapons[j].GetComponent<Item>().uses != GameControl.control.weapons[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 1 && counter == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                counter = 0;
                for (int j = 0; j < GameControl.control.equipment.Count; j++)
                { if (GameControl.control.equipment[j].GetComponent<Item>().uses != GameControl.control.equipment[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 2 && counter == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                counter = 0;
                for (int j = 0; j < GameControl.control.reusables.Count; j++)
                { if (GameControl.control.reusables[j].GetComponent<Item>().uses != GameControl.control.reusables[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 3 && counter == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
            }

            else if (buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
                     buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover)
            {
                int counter = 0;
                for (int j = 0; j < GameControl.control.consumables.Count; j++)
                { if (GameControl.control.consumables[j].GetComponent<Item>().uses != GameControl.control.consumables[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 0 && counter > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover; }
                counter = 0;
                for (int j = 0; j < GameControl.control.weapons.Count; j++)
                { if (GameControl.control.weapons[j].GetComponent<Item>().uses != GameControl.control.weapons[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 1 && counter > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
                counter = 0;
                for (int j = 0; j < GameControl.control.equipment.Count; j++)
                { if (GameControl.control.equipment[j].GetComponent<Item>().uses != GameControl.control.equipment[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 2 && counter > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
                counter = 0;
                for (int j = 0; j < GameControl.control.reusables.Count; j++)
                { if (GameControl.control.reusables[j].GetComponent<Item>().uses != GameControl.control.reusables[j].GetComponent<Item>().quantity) { counter++; } }
                if (i == 3 && counter > 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckForInactive();
        if (isVisible && frameDelay > 0)
        {
            if (Input.GetKeyUp(GameControl.control.backKey))
            {
                pm.ActivateMenu();
            }
        }
        base.Update();
	}
}
