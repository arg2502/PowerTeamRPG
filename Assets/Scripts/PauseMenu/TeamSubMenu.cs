using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamSubMenu : SubMenu {

    GameObject heroDescription; // the obj that will show the hero info
    HeroSubMenu heroSub; // A universal submenu for all heroes

    List<GameObject> heroInfo; // displays all manner of info for the hero - stats, passives, etc.
    List<int> statChangeNumbers; // stores the calculated stat changes an item will cause
    List<GameObject> statChanges; // displays any changes an item may cause

    Color myGreen = new Color(0.1f, 0.6f, 0.2f);
    Color myRed = new Color(5.0f, 0.1f, 0.2f);

    //public bool isActive;

	// Use this for initialization
	void Start () {
        contentArray = new List<string>();
        buttonDescription = new List<string>();

        statChangeNumbers = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();

        // create the text objects that will show the items' effects
        statChanges = new List<GameObject>();
        for (int i = 0; i < 11; i++)
        {
            statChanges.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            statChanges[i].transform.position = pm.descriptionText.transform.position + new Vector3(200.0f, -(i * 35.0f), 0.0f);
            statChanges[i].GetComponent<Renderer>().enabled = false;
        }

        // Create the hero sub menu
        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/HeroSubMenu"));
        heroSub = temp.GetComponent<HeroSubMenu>();
        heroSub.parentPos = buttonArray[selectedIndex].transform;
	}

    public void EnableSubMenu()
    {
        pm.descriptionText.GetComponent<Renderer>().enabled = false;
        //heroDescription.GetComponent<Renderer>().enabled = true;
        //isActive = true;
        // create the text objects that will show the hero's stats
        SetStatChanges(0);
        InstantiateHeroInfo();
        for (int i = 0; i < statChanges.Count; i++) { statChanges[i].transform.position = pm.descriptionText.transform.position + new Vector3(200.0f, -(i * 35.0f), 0.0f); }
            //UpdateStatChanges(GameControl.control.heroList[selectedIndex]);
            foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = true; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = true; }
        base.EnableSubMenu();
    }

    public void DisableSubMenu()
    {
        if (GameControl.control.isPaused) { pm.descriptionText.GetComponent<Renderer>().enabled = true; }
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
        base.DisableSubMenu();
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        heroSub.heroID = selectedIndex;
        heroSub.EnableSubMenu();
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

    public void InstantiateHeroInfo()
    {
        if (heroInfo == null) { heroInfo = new List<GameObject>(); }
        else { foreach (GameObject go in heroInfo) { Destroy(go); } heroInfo.Clear(); }

        // Create all of the text Objs necessary
        for (int i = 0; i < (11 + GameControl.control.heroList[selectedIndex].passiveList.Count); i++)
        {
            heroInfo.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            heroInfo[i].transform.position = pm.descriptionText.transform.position + new Vector3(0.0f, -(i * 35.0f), 0.0f);
        }
        heroInfo[0].GetComponent<TextMesh>().text = GameControl.control.heroList[selectedIndex].name;
        heroInfo[1].GetComponent<TextMesh>().text = "Status: " + GameControl.control.heroList[selectedIndex].statusState;
        heroInfo[2].GetComponent<TextMesh>().text = "Hp: " + Mathf.Clamp((GameControl.control.heroList[selectedIndex].hp + statChangeNumbers[1]), 0,
            GameControl.control.heroList[selectedIndex].hpMax) + " / " + GameControl.control.heroList[selectedIndex].hpMax;
        heroInfo[3].GetComponent<TextMesh>().text = "Pm: " + Mathf.Clamp((GameControl.control.heroList[selectedIndex].pm + statChangeNumbers[2]), 0,
            GameControl.control.heroList[selectedIndex].pmMax) + " / " + GameControl.control.heroList[selectedIndex].pmMax;
        heroInfo[4].GetComponent<TextMesh>().text = "Atk: " + (GameControl.control.heroList[selectedIndex].atk + statChangeNumbers[3]);
        heroInfo[5].GetComponent<TextMesh>().text = "Def: " + (GameControl.control.heroList[selectedIndex].def + statChangeNumbers[4]);
        heroInfo[6].GetComponent<TextMesh>().text = "Mgk Atk: " + (GameControl.control.heroList[selectedIndex].mgkAtk + statChangeNumbers[5]);
        heroInfo[7].GetComponent<TextMesh>().text = "Mgk Def: " + (GameControl.control.heroList[selectedIndex].mgkDef + statChangeNumbers[6]);
        heroInfo[8].GetComponent<TextMesh>().text = "Luck: " + (GameControl.control.heroList[selectedIndex].luck + statChangeNumbers[7]);
        heroInfo[9].GetComponent<TextMesh>().text = "Evasion: " + (GameControl.control.heroList[selectedIndex].evasion + statChangeNumbers[8]);
        heroInfo[10].GetComponent<TextMesh>().text = "Spd: " + (GameControl.control.heroList[selectedIndex].spd + statChangeNumbers[9]);
        for (int i = 11; (i - 11) < GameControl.control.heroList[selectedIndex].passiveList.Count; i++) 
            { heroInfo[i].GetComponent<TextMesh>().text = GameControl.control.heroList[selectedIndex].passiveList[i - 11].Name; }
    }

    // this method exists to be more efficient with memory - all items minimally have these 10 attributes
    public void SetStatChanges(int currentButton)
    {
        if (currentButton == 2 && GameControl.control.heroList[selectedIndex].weapon != null) // remove weapon
        {
            statChangeNumbers[1] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().hpChange;
            statChangeNumbers[2] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().pmChange;
            statChangeNumbers[3] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().atkChange;
            statChangeNumbers[4] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().defChange;
            statChangeNumbers[5] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().mgkAtkChange;
            statChangeNumbers[6] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().mgkDefChange;
            statChangeNumbers[7] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().luckChange;
            statChangeNumbers[8] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().evadeChange;
            statChangeNumbers[9] = -GameControl.control.heroList[selectedIndex].weapon.GetComponent<WeaponItem>().spdChange;
        }
        else if (currentButton == 3 && GameControl.control.heroList[selectedIndex].equipment.Count > 0) // remove armor
        {
            for (int i = 1; i < 10; i++) { statChangeNumbers[i] = 0; }
            for (int i = 0; i < GameControl.control.heroList[selectedIndex].equipment.Count; i++)
            {
                statChangeNumbers[1] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().hpChange;
                statChangeNumbers[2] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().pmChange;
                statChangeNumbers[3] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().atkChange;
                statChangeNumbers[4] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().defChange;
                statChangeNumbers[5] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().mgkAtkChange;
                statChangeNumbers[6] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().mgkDefChange;
                statChangeNumbers[7] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().luckChange;
                statChangeNumbers[8] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().evadeChange;
                statChangeNumbers[9] += -GameControl.control.heroList[selectedIndex].equipment[i].GetComponent<ArmorItem>().spdChange;
            }
        }
        else
        {
            for (int i = 1; i < 10; i++) { statChangeNumbers[i] = 0; }
        }
    }

    public void UpdateStatChanges()
    {
        for (int i = 1; i < statChangeNumbers.Count; i++)
        {
            if (statChangeNumbers[i] > 0)
            {
                statChanges[i + 1].GetComponent<TextMesh>().text = "(+" + statChangeNumbers[i] + ")";
                statChanges[i + 1].GetComponent<TextMesh>().color = myGreen;
                heroInfo[i + 1].GetComponent<TextMesh>().color = myGreen;
            }
            else if (statChangeNumbers[i] < 0)
            {
                statChanges[i + 1].GetComponent<TextMesh>().text = "(" + statChangeNumbers[i] + ")";
                statChanges[i + 1].GetComponent<TextMesh>().color = myRed;
                heroInfo[i + 1].GetComponent<TextMesh>().color = myRed;
            }
            else
            {
                statChanges[i + 1].GetComponent<TextMesh>().text = "";
                statChanges[i + 1].GetComponent<TextMesh>().color = Color.black;
                heroInfo[i + 1].GetComponent<TextMesh>().color = Color.black;
            }
        }
    }
	
	// Update is called once per frame
	public void Update () {
        if (isVisible && isActive)
        {
            //update which position the submenu should appear in
            heroSub.parentPos = buttonArray[selectedIndex].transform;

            //heroDescription.transform.position = pm.descriptionText.transform.position;
            
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                pm.ActivateMenu();
                pm.descriptionText.GetComponent<Renderer>().enabled = true;

                foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
                foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
            }

            // unpause the game
            if (Input.GetKeyUp(KeyCode.Q) && isActive)
            {
                pm.descriptionText.GetComponent<Renderer>().enabled = true;
                //pm.descriptionText.GetComponent<Renderer>().enabled = false;
                foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
                foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
            }

            base.Update();
        }
        if (isVisible)
        {
            UpdateStatChanges();
            //update the position of the menu
            for (int i = 0; i < contentArray.Count; i++)
            {
                MyButton b = buttonArray[i].GetComponent<MyButton>();
                buttonArray[i].transform.position = new Vector2(parentPos.position.x + b.width + 50, parentPos.position.y + (i * -(b.height + b.height / 2)));
                b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
            }

            //heroDescription.transform.position = pm.descriptionText.transform.position;
        }
        if (!isVisible) { isActive = false;  heroDescription.GetComponent<Renderer>().enabled = false; }
	}
}
