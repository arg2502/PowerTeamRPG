﻿using UnityEngine;
using System.Collections;

public class HeroSubMenu : SubMenu {

    TeamSubMenu tsm;
    public int heroID = 0;
    HeroData activeHero;

	// Use this for initialization
	void Start () {
        tsm = GameObject.FindObjectOfType<TeamSubMenu>();
        base.Start();

        if (GameControl.control.isPaused) { GameControl.control.RestorePauseMenu(); tsm.Update(); }
	}

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Equip Weapon":
                break;
            case "Equip Armor":
                break;
            case "View Skill Tree":
                activeHero.skillTree = true;
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTreeMenu");
                break;
            case "Allocate Stat Points":
                activeHero.statBoost = true;
                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                GameControl.control.RecordRoom();
                GameControl.control.RecordPauseMenu();
                GameControl.control.RecordEnemyPos();
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelUpMenu");
                break;
            default:
                break;
        }
    }

    public void EnableSubMenu()
    {
        tsm.DeactivateMenu();
        // find the hero we're working with
        foreach (HeroData h in GameControl.control.heroList)
        {
            if (h.identity == heroID) { activeHero = h; }
        }
        base.EnableSubMenu();
    }

    void CheckForInactive()
    {
        // disable the two item related buttons associated with this submenu
        if (buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
            buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover &&
            GameControl.control.weapons.Count == 0) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
        else if (buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
                 buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover &&
                 GameControl.control.weapons.Count > 0) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }

        if (buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
            buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover &&
            GameControl.control.equipment.Count == 0) { buttonArray[3].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
        else if (buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
                 buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover &&
                 GameControl.control.equipment.Count > 0) { buttonArray[3].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
    }

	// Update is called once per frame
	void Update () {
        CheckForInactive();
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            tsm.ActivateMenu();
        }

        // unpause the game
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //tsm.ActivateMenu();
            //pm.descriptionText.GetComponent<Renderer>().enabled = true;
            tsm.DisableSubMenu();
        }
        base.Update();
	}
}