using UnityEngine;
using System.Collections;

public class LoadSubMenu : SubMenu {

	// Use this for initialization
	void Start () {
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
