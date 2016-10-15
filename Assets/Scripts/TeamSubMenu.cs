using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamSubMenu : SubMenu {

	// Use this for initialization
	void Start () {
        contentArray = new List<string>();
        buttonDescription = new List<string>();
        // make the content array reflect the heroes in your party
        foreach (HeroData hd in GameControl.control.heroList)
        {
            contentArray.Add(hd.name);
            buttonDescription.Add("Placeholder shit");
        }

        base.Start();
	}

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
