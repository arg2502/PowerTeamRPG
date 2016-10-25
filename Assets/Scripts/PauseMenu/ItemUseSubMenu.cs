using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUseSubMenu : SubMenu {

    ConsumableItemSubMenu parent;
    List<GameObject> heroInfo; // displays all manner of info for the hero - stats, passives, etc.
    List<int> statChangeNumbers; // stores the calculated stat changes an item will cause
    List<GameObject> statChanges; // displays any changes an item may cause
    Item currentItem;

    Color myGreen = new Color(0.1f, 0.6f, 0.2f);
    Color myRed = new Color(5.0f, 0.1f, 0.2f);

	// Use this for initialization
	void Start () {
        parent = GameObject.FindObjectOfType<ConsumableItemSubMenu>();
        contentArray = new List<string>();
        statChangeNumbers = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // make the content array reflect the heroes in your party
        foreach (HeroData hd in GameControl.control.heroList)
        {
            contentArray.Add(hd.name);
            //buttonDescription.Add("Use on:\n" + hd.name + "\nStatus: " + hd.statusState
            //    + "\nHp: " + hd.hp + " / " + hd.hpMax
            //    + "\nPm: " + hd.pm + " / " + hd.pmMax);
            buttonDescription.Add("");
        }

        base.Start();

        // disable the buttons for any fallen heroes unless item can be used on the dead
        if (GameControl.control.whichInventory == "consumables")
        {
            // this switch statement will separate items that can be used on fallen heroes from items which cannot
            // the default case will handle every item not usable on fallen enemies
            // all other cases will have the name of specific items (Ex: "Revive")
            switch (parent.itemName)
            {
                default:
                    for (int i = 0; i < GameControl.control.heroList.Count; i++)
                    {
                        if (GameControl.control.heroList[i].statusState == HeroData.Status.dead || GameControl.control.heroList[i].statusState == HeroData.Status.overkill)
                        {
                            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
                        }
                    }
                        break;
            }
        }

        // create the text objects that will show the items' effects
        statChanges = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            statChanges.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            statChanges[i].transform.position = parent.im.descriptionText.transform.position + new Vector3(200.0f, -(i * 35.0f), 0.0f);
            statChanges[i].GetComponent<Renderer>().enabled = false;
        }
	}

    public void InstantiateHeroInfo(HeroData h)
    {
        if (heroInfo == null) { heroInfo = new List<GameObject>(); }
        else { foreach (GameObject go in heroInfo) { Destroy(go); } heroInfo.Clear(); }

        // Create all of the text Objs necessary
        for (int i = 0; i < (10 + h.passiveList.Count); i++) {
            heroInfo.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            heroInfo[i].transform.position = parent.im.descriptionText.transform.position + new Vector3(0.0f, -(i * 35.0f), 0.0f);
        }
        heroInfo[0].GetComponent<TextMesh>().text = "Status: " + h.statusState;
        heroInfo[1].GetComponent<TextMesh>().text = "Hp: " + Mathf.Clamp((h.hp + statChangeNumbers[1]), 0, h.hpMax) + " / " + h.hpMax;
        heroInfo[2].GetComponent<TextMesh>().text = "Pm: " + Mathf.Clamp((h.pm + statChangeNumbers[2]), 0, h.pmMax) + " / " + h.pmMax;
        heroInfo[3].GetComponent<TextMesh>().text = "Atk: " + (h.atk + statChangeNumbers[3]);
        heroInfo[4].GetComponent<TextMesh>().text = "Def: " + (h.def + statChangeNumbers[4]);
        heroInfo[5].GetComponent<TextMesh>().text = "Mgk Atk: " + (h.mgkAtk + statChangeNumbers[5]);
        heroInfo[6].GetComponent<TextMesh>().text = "Mgk Def: " + (h.mgkDef + statChangeNumbers[6]);
        heroInfo[7].GetComponent<TextMesh>().text = "Luck: " + (h.luck + statChangeNumbers[7]);
        heroInfo[8].GetComponent<TextMesh>().text = "Evasion: " + (h.evasion + statChangeNumbers[8]);
        heroInfo[9].GetComponent<TextMesh>().text = "Spd: " + (h.spd + statChangeNumbers[9]);
        for (int i = 10; (i - 10) < h.passiveList.Count; i++) { heroInfo[i].GetComponent<TextMesh>().text = h.passiveList[i - 10].Name; }
    }

    // this method exists to be more efficient with memory - all items minimally have these 10 attributes
    public void SetStatChanges(HeroData h)
    {
        // draw the current item from the correct list of items
        if (GameControl.control.whichInventory == "consumables") 
        { 
            //currentItem = GameControl.control.consumables[parent.itemIndex].GetComponent<Item>();
            foreach (GameObject go in GameControl.control.consumables) { if (go.GetComponent<Item>().name == parent.itemName) { currentItem = go.GetComponent<Item>(); } }

            statChanges[0].GetComponent<TextMesh>().text = currentItem.statusChange;
            statChangeNumbers[1] = currentItem.hpChange;
            statChangeNumbers[2] = currentItem.pmChange;
            statChangeNumbers[3] = currentItem.atkChange;
            statChangeNumbers[4] = currentItem.defChange;
            statChangeNumbers[5] = currentItem.mgkAtkChange;
            statChangeNumbers[6] = currentItem.mgkDefChange;
            statChangeNumbers[7] = currentItem.luckChange;
            statChangeNumbers[8] = currentItem.evadeChange;
            statChangeNumbers[9] = currentItem.spdChange;
        }
        //else if (GameControl.control.whichInventory == "reusables") { currentItem = GameControl.control.reusables[parent.itemIndex].GetComponent<Item>(); }
        else if (GameControl.control.whichInventory == "weapons") 
        { 
            //currentItem = GameControl.control.weapons[parent.itemIndex].GetComponent<Item>();
            foreach (GameObject go in GameControl.control.weapons) { if (go.GetComponent<Item>().name == parent.itemName) { currentItem = go.GetComponent<Item>(); } }

            Item heroWeapon = null;
            if (h.weapon != null) { heroWeapon = h.weapon.GetComponent<Item>(); }

            if (h.weapon == null)
            {
                statChangeNumbers[1] = currentItem.hpChange;
                statChangeNumbers[2] = currentItem.pmChange;
                statChangeNumbers[3] = currentItem.atkChange;
                statChangeNumbers[4] = currentItem.defChange;
                statChangeNumbers[5] = currentItem.mgkAtkChange;
                statChangeNumbers[6] = currentItem.mgkDefChange;
                statChangeNumbers[7] = currentItem.luckChange;
                statChangeNumbers[8] = currentItem.evadeChange;
                statChangeNumbers[9] = currentItem.spdChange;
            }
            else
            {
                statChangeNumbers[1] = currentItem.hpChange - heroWeapon.hpChange;
                statChangeNumbers[2] = currentItem.pmChange - heroWeapon.pmChange;
                statChangeNumbers[3] = currentItem.atkChange - heroWeapon.atkChange;
                statChangeNumbers[4] = currentItem.defChange - heroWeapon.defChange;
                statChangeNumbers[5] = currentItem.mgkAtkChange - heroWeapon.mgkAtkChange;
                statChangeNumbers[6] = currentItem.mgkDefChange - heroWeapon.mgkDefChange;
                statChangeNumbers[7] = currentItem.luckChange - heroWeapon.luckChange;
                statChangeNumbers[8] = currentItem.evadeChange - heroWeapon.evadeChange;
                statChangeNumbers[9] = currentItem.spdChange - heroWeapon.spdChange;
            }
        }
        else if (GameControl.control.whichInventory == "armor")
        { 
            //currentItem = GameControl.control.equipment[parent.itemIndex].GetComponent<Item>();
            foreach (GameObject go in GameControl.control.equipment) { if (go.GetComponent<Item>().name == parent.itemName) { currentItem = go.GetComponent<Item>(); } }

            ArmorItem relevantItem = null; // this will need to be of the same type as the current item (Ex: helmet, gloves, etc)
            for (int i = 0; i < h.equipment.Count; i++) 
            {
                if (h.equipment[i].GetComponent<ArmorItem>().type == currentItem.GetComponent<ArmorItem>().type) { relevantItem = h.equipment[i].GetComponent<ArmorItem>(); }
            }

            if (relevantItem == null)
            {
                statChangeNumbers[1] = currentItem.hpChange;
                statChangeNumbers[2] = currentItem.pmChange;
                statChangeNumbers[3] = currentItem.atkChange;
                statChangeNumbers[4] = currentItem.defChange;
                statChangeNumbers[5] = currentItem.mgkAtkChange;
                statChangeNumbers[6] = currentItem.mgkDefChange;
                statChangeNumbers[7] = currentItem.luckChange;
                statChangeNumbers[8] = currentItem.evadeChange;
                statChangeNumbers[9] = currentItem.spdChange;
            }
            else
            {
                statChangeNumbers[1] = currentItem.hpChange - relevantItem.hpChange;
                statChangeNumbers[2] = currentItem.pmChange - relevantItem.pmChange;
                statChangeNumbers[3] = currentItem.atkChange - relevantItem.atkChange;
                statChangeNumbers[4] = currentItem.defChange - relevantItem.defChange;
                statChangeNumbers[5] = currentItem.mgkAtkChange - relevantItem.mgkAtkChange;
                statChangeNumbers[6] = currentItem.mgkDefChange - relevantItem.mgkDefChange;
                statChangeNumbers[7] = currentItem.luckChange - relevantItem.luckChange;
                statChangeNumbers[8] = currentItem.evadeChange - relevantItem.evadeChange;
                statChangeNumbers[9] = currentItem.spdChange - relevantItem.spdChange;
            }
        }
    }

    void UpdateStatChanges()
    {
        for (int i = 1; i < statChangeNumbers.Count; i++)
        {
            if (statChangeNumbers[i] > 0)
            {
                statChanges[i].GetComponent<TextMesh>().text = "(+" + statChangeNumbers[i] + ")";
                statChanges[i].GetComponent<TextMesh>().color = myGreen;
                heroInfo[i].GetComponent<TextMesh>().color = myGreen;
            }
            else if (statChangeNumbers[i] < 0)
            {
                statChanges[i].GetComponent<TextMesh>().text = "(" + statChangeNumbers[i] + ")";
                statChanges[i].GetComponent<TextMesh>().color = myRed;
                heroInfo[i].GetComponent<TextMesh>().color = myRed;
            }
            else
            {
                statChanges[i].GetComponent<TextMesh>().text = "";
                statChanges[i].GetComponent<TextMesh>().color = Color.black;
                heroInfo[i].GetComponent<TextMesh>().color = Color.black;
            }
        }
    }

    public void EnableSubMenu()
    {
        parent.DeactivateMenu();
        parent.im.descriptionText.GetComponent<Renderer>().enabled = false;

        // create the text objects that will show the hero's stats
        SetStatChanges(GameControl.control.heroList[selectedIndex]);
        InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]);
        //UpdateStatChanges(GameControl.control.heroList[selectedIndex]);
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = true; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = true; }

        base.EnableSubMenu();
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        if (GameControl.control.whichInventory == "consumables") { currentItem.GetComponent<ConsumableItem>().Use(GameControl.control.heroList[selectedIndex]); }
        else if (GameControl.control.whichInventory == "weapons") { currentItem.GetComponent<WeaponItem>().Use(GameControl.control.heroList[selectedIndex]); }
        else if (GameControl.control.whichInventory == "armor") { currentItem.GetComponent<ArmorItem>().Use(GameControl.control.heroList[selectedIndex]); }

        // after item use, close all submenus
        parent.ActivateMenu();
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
        DisableSubMenu();
        parent.im.ActivateMenu();
        parent.DisableSubMenu();
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisible && frameDelay > 0) 
        {
            UpdateStatChanges();
            //parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) { SetStatChanges(GameControl.control.heroList[selectedIndex]); InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); }
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q)) 
            {
                parent.ActivateMenu();
                foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
                foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
            }
        }
        
        base.Update();
	}
}
