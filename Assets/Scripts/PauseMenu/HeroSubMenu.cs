using UnityEngine;
using System.Collections;

public class HeroSubMenu : SubMenu {

    TeamSubMenu tsm;

	// Use this for initialization
	void Start () {
        tsm = GameObject.FindObjectOfType<TeamSubMenu>();
        base.Start();
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
                break;
            case "Allocate Stat Points":
                break;
            default:
                break;
        }
    }

    public void EnableSubMenu()
    {
        tsm.DeactivateMenu();
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
            tsm.ActivateMenu();
            //pm.descriptionText.GetComponent<Renderer>().enabled = true;
        }
        base.Update();
	}
}
