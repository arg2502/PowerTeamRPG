using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamSubMenu : SubMenu {

    GameObject heroDescription; // the obj that will show the hero info
    HeroSubMenu heroSub; // A universal submenu for all heroes

    //public bool isActive;

	// Use this for initialization
	void Start () {
        contentArray = new List<string>();
        buttonDescription = new List<string>();

        //set up the description obj
        heroDescription = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        heroDescription.GetComponent<Renderer>().sortingOrder = 900;
        //heroDescription.transform.position = pm.descriptionText.transform.position;

        // make the content array reflect the heroes in your party
        foreach (HeroData hd in GameControl.control.heroList)
        {
            contentArray.Add(hd.name);
            buttonDescription.Add("O_o");
        }

        base.Start();

        // Create the hero sub menu
        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/HeroSubMenu"));
        heroSub = temp.GetComponent<HeroSubMenu>();
        heroSub.parentPos = buttonArray[selectedIndex].transform;

        //set the herodescription to the appropriate info
        heroDescription.GetComponent<TextMesh>().text = ("Status: " + GameControl.control.heroList[selectedIndex].statusState
            + "\nHP: " + GameControl.control.heroList[selectedIndex].hp + " / " + GameControl.control.heroList[selectedIndex].hpMax
            + "\nPM: " + GameControl.control.heroList[selectedIndex].pm + " / " + GameControl.control.heroList[selectedIndex].pmMax
            + "\nAtk: " + GameControl.control.heroList[selectedIndex].atk + "\nDef: " + GameControl.control.heroList[selectedIndex].def
            + "\nMgk Atk: " + GameControl.control.heroList[selectedIndex].mgkAtk + "\nMgk Def: " + GameControl.control.heroList[selectedIndex].mgkDef
            + "\nLuck: " + GameControl.control.heroList[selectedIndex].luck + "\nEvasion: " + GameControl.control.heroList[selectedIndex].evasion
            + "\nSpd: " + GameControl.control.heroList[selectedIndex].spd);
	}

    public void EnableSubMenu()
    {
        pm.descriptionText.GetComponent<Renderer>().enabled = false;
        heroDescription.GetComponent<Renderer>().enabled = true;
        //isActive = true;
        base.EnableSubMenu();
    }

    public void DisableSubMenu()
    {
        if (GameControl.control.isPaused) { pm.descriptionText.GetComponent<Renderer>().enabled = true; }
        base.DisableSubMenu();
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        heroSub.EnableSubMenu();
        heroSub.heroID = selectedIndex;
    }

    public void DeactivateMenu()
    {
        isActive = false;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
        }
    }

    public void ActivateMenu()
    {
        frameDelay = 0.0f;
        isActive = true;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisible && isActive)
        {
            //update which position the submenu should appear in
            heroSub.parentPos = buttonArray[selectedIndex].transform;

            //heroDescription.transform.position = pm.descriptionText.transform.position;
            
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                pm.ActivateMenu();
                pm.descriptionText.GetComponent<Renderer>().enabled = true;
            }

            // unpause the game
            if (Input.GetKeyUp(KeyCode.Q))
            {
                pm.descriptionText.GetComponent<Renderer>().enabled = true;
                //pm.descriptionText.GetComponent<Renderer>().enabled = false;
            }

            base.Update();
        }
        if (isVisible)
        {
            heroDescription.transform.position = pm.descriptionText.transform.position;
            //set the herodescription to the appropriate info
            heroDescription.GetComponent<TextMesh>().text = ("Status: " + GameControl.control.heroList[selectedIndex].statusState
                + "\nHP: " + GameControl.control.heroList[selectedIndex].hp + " / " + GameControl.control.heroList[selectedIndex].hpMax
                + "\nPM: " + GameControl.control.heroList[selectedIndex].pm + " / " + GameControl.control.heroList[selectedIndex].pmMax
                + "\nAtk: " + GameControl.control.heroList[selectedIndex].atk + "\nDef: " + GameControl.control.heroList[selectedIndex].def
                + "\nMgk Atk: " + GameControl.control.heroList[selectedIndex].mgkAtk + "\nMgk Def: " + GameControl.control.heroList[selectedIndex].mgkDef
                + "\nLuck: " + GameControl.control.heroList[selectedIndex].luck + "\nEvasion: " + GameControl.control.heroList[selectedIndex].evasion
                + "\nSpd: " + GameControl.control.heroList[selectedIndex].spd + "\n\nExp to next level: " + GameControl.control.heroList[selectedIndex].expToLvlUp
                + "\nTotal Exp: " + GameControl.control.heroList[selectedIndex].exp);
        }
        //base.Update();
        if (!isVisible) { isActive = false;  heroDescription.GetComponent<Renderer>().enabled = false; }
	}
}
