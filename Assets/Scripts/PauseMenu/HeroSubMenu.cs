//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class HeroSubMenu : SubMenu {

//    TeamSubMenu tsm;
//    public int heroID = 0;
//    DenigenData activeHero;

    

//	// Use this for initialization
//	void Start () {
//        tsm = GameObject.FindObjectOfType<TeamSubMenu>();
//        base.Start();

//        //call change text method to correctly size text and avoid a certain bug
//        ChangeText();

//        if (GameControl.control.isPaused) { GameControl.control.RestorePauseMenu(); tsm.Update(); }
//	}

//    // deal with the button pressed
//    public override void ButtonAction(string label)
//    {
//        switch (label)
//        {
//            case "Remove Weapon":
//                if (activeHero.weapon != null)
//                {
//                    activeHero.weapon.GetComponent<WeaponItem>().Remove(activeHero); // remove current weapon
//                    activeHero.weapon = null;
//                    tsm.SetStatChanges(selectedIndex);
//                }
//                break;
//            case "Remove Armor":
//                if (activeHero.equipment.Count > 0)
//                {
//                    for (int i = 0; i < activeHero.equipment.Count; i++ )
//                    {
//                        activeHero.equipment[i].GetComponent<ArmorItem>().Remove(activeHero);
//                    }
//                    activeHero.equipment.Clear();
//                    tsm.SetStatChanges(selectedIndex);
//                }
//                break;
//            case "View Skill Tree":
//                activeHero.skillTree = true;
//                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
//                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
//                GameControl.control.RecordRoom();
//                GameControl.control.RecordPauseMenu();
//                GameControl.control.RecordEnemyPos();
//                UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTreeMenu");
//                break;
//            case "Allocate Stat Points":
//                activeHero.statBoost = true;
//                GameControl.control.currentPosition = pm.player.transform.position; //record the player's position
//                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
//                GameControl.control.RecordRoom();
//                GameControl.control.RecordPauseMenu();
//                GameControl.control.RecordEnemyPos();
//                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelUpMenu");
//                break;
//            default:
//                break;
//        }
//    }

//    public void EnableSubMenu()
//    {
//        tsm.DeactivateMenu();
//        // find the hero we're working with
//        foreach (DenigenData h in GameControl.control.heroList)
//        {
//            if (h.identity == heroID) { activeHero = h; }
//        }
//        base.EnableSubMenu();
//    }

//    void CheckForInactive()
//    {
//        // disable the two item related buttons associated with this submenu
//        if (buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
//            buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover &&
//            GameControl.control.weapons.Count == 0) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
//        else if (buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
//                 buttonArray[2].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover &&
//                 GameControl.control.weapons.Count > 0) { buttonArray[2].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }

//        if (buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
//            buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover &&
//            GameControl.control.equipment.Count == 0) { buttonArray[3].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
//        else if (buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.normal &&
//                 buttonArray[3].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.hover &&
//                 GameControl.control.equipment.Count > 0) { buttonArray[3].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
//    }

//	// Update is called once per frame
//	void Update () {
//        CheckForInactive();
//        //tsm.UpdateStatChanges();
//        if (Input.GetKeyUp(GameControl.control.backKey) && isActive)
//        {
//            tsm.ActivateMenu();
//            tsm.SetStatChanges(0);
//            tsm.InstantiateHeroInfo();
//        }

//        if ((Input.GetKeyUp(GameControl.control.upKey) || Input.GetKeyUp(GameControl.control.downKey)) && tsm.isVisible) { tsm.SetStatChanges(selectedIndex); tsm.InstantiateHeroInfo(); }
//        // unpause the game
//        if (Input.GetKeyUp(GameControl.control.pauseKey) && isVisible)
//        {
//            //tsm.ActivateMenu();
//            //pm.descriptionText.GetComponent<Renderer>().enabled = true;
//            tsm.DisableSubMenu();
//        }
//        base.Update();
//	}
//}
