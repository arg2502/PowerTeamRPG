using UnityEngine;
using System.Collections;

public class HeroSubMenu : SubMenu {

    TeamSubMenu tsm;
    public int heroID = 0;
    HeroData activeHero;

	// Use this for initialization
	void Start () {
        tsm = GameObject.FindObjectOfType<TeamSubMenu>();
        base.Start();

        if (GameControl.control.isPaused) { GameControl.control.RestorePauseMenu(); }
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

	// Update is called once per frame
	void Update () {
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
