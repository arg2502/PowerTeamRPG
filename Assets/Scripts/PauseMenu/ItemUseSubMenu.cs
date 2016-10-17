using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUseSubMenu : SubMenu {

    ConsumableItemSubMenu parent;

	// Use this for initialization
	void Start () {
        parent = GameObject.FindObjectOfType<ConsumableItemSubMenu>();
        contentArray = new List<string>();

        // make the content array reflect the heroes in your party
        foreach (HeroData hd in GameControl.control.heroList)
        {
            contentArray.Add(hd.name);
            buttonDescription.Add("Use on:\n" + hd.name + "\nStatus: " + hd.statusState
                + "\nHp: " + hd.hp + " / " + hd.hpMax
                + "\nPm: " + hd.pm + " / " + hd.pmMax);
        }

        base.Start();
	}

    public void EnableSubMenu()
    {
        parent.DeactivateMenu();
        base.EnableSubMenu();
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisible && frameDelay > 0) 
        {
            parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q)) { parent.ActivateMenu(); }
        }
        
        base.Update();
	}
}
