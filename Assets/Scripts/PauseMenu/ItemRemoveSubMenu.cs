using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRemoveSubMenu : SubMenu {

    ConsumableItemSubMenu parent;
    List<GameObject> heroInfo; // displays all manner of info for the hero - stats, passives, etc.
    List<int> statChangeNumbers; // stores the calculated stat changes an item will cause
    List<GameObject> statChanges; // displays any changes an item may cause
    //Item parent.currentItem;

    Color myGreen = new Color(0.1f, 0.6f, 0.2f);
    Color myRed = new Color(5.0f, 0.1f, 0.2f);
    
    // Use this for initialization
    void Start() {
        parent = GameObject.FindObjectOfType<ConsumableItemSubMenu>();
        contentArray = new List<string>();
        statChangeNumbers = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // make the content array reflect the heroes in your party
        foreach (DenigenData hd in GameControl.control.heroList)
        {
            contentArray.Add(hd.denigenName);
            //buttonDescription.Add("Use on:\n" + hd.name + "\nStatus: " + hd.statusState
            //    + "\nHp: " + hd.hp + " / " + hd.hpMax
            //    + "\nPm: " + hd.pm + " / " + hd.pmMax);
            //buttonDescription.Add("");

        }
        base.Start();


        CheckForInactive();

        // create the text objects that will show the items' effects
        statChanges = new List<GameObject>();
        for (int i = 0; i < 11; i++)
        {
            statChanges.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            statChanges[i].name = "StatChanges" + i;
            statChanges[i].transform.position = parent.im.descriptionText.transform.position + new Vector3(3.125f, -(i * 0.55f), 0.0f);
            statChanges[i].GetComponent<Renderer>().enabled = false;
        }

    }

    public void InstantiateHeroInfo(DenigenData h)
    {
        if (heroInfo == null) { heroInfo = new List<GameObject>(); }
        else { foreach (GameObject go in heroInfo) { Destroy(go); } heroInfo.Clear(); }

        // Create all of the text Objs necessary
        for (int i = 0; i < (11 + h.passiveList.Count); i++)
        {
            heroInfo.Add((GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab")));
            heroInfo[i].name = "HeroInfo" + i;
            heroInfo[i].transform.position = parent.im.descriptionText.transform.position + new Vector3(0.0f, -(i * 0.55f), 0.0f);
        }
        heroInfo[0].GetComponent<TextMesh>().text = h.denigenName;
        heroInfo[1].GetComponent<TextMesh>().text = "Status: " + h.statusState;
        heroInfo[2].GetComponent<TextMesh>().text = "Hp: " + Mathf.Clamp((h.hp + statChangeNumbers[1]), 0, h.hpMax) + " / " + h.hpMax;
        heroInfo[3].GetComponent<TextMesh>().text = "Pm: " + Mathf.Clamp((h.pm + statChangeNumbers[2]), 0, h.pmMax) + " / " + h.pmMax;
        heroInfo[4].GetComponent<TextMesh>().text = "Atk: " + (h.atk + statChangeNumbers[3]);
        heroInfo[5].GetComponent<TextMesh>().text = "Def: " + (h.def + statChangeNumbers[4]);
        heroInfo[6].GetComponent<TextMesh>().text = "Mgk Atk: " + (h.mgkAtk + statChangeNumbers[5]);
        heroInfo[7].GetComponent<TextMesh>().text = "Mgk Def: " + (h.mgkDef + statChangeNumbers[6]);
        heroInfo[8].GetComponent<TextMesh>().text = "Luck: " + (h.luck + statChangeNumbers[7]);
        heroInfo[9].GetComponent<TextMesh>().text = "Evasion: " + (h.evasion + statChangeNumbers[8]);
        heroInfo[10].GetComponent<TextMesh>().text = "Spd: " + (h.spd + statChangeNumbers[9]);
        for (int i = 11; (i - 11) < h.passiveList.Count; i++) { heroInfo[i].GetComponent<TextMesh>().text = h.passiveList[i - 11].Name; }
    }

    // this method exists to be more efficient with memory - all items minimally have these 10 attributes
    public void SetStatChanges(DenigenData h)
    {
        // if they don't have the item, don't do anything
        if (GameControl.control.whichInventory == "weapons"
            && (h.weapon == null || h.weapon.GetComponent<Item>() != parent.currentItem))
        {
            for(int i = 1; i < statChangeNumbers.Count; i++)
            {
                statChangeNumbers[i] = 0;
            }
            return;
        }

        if(GameControl.control.whichInventory == "armor")
        {
            bool match = false;
            for(int i = 0; i < h.equipment.Count; i++)
            {
                if (h.equipment[i].GetComponent<Item>() == parent.currentItem)
                    match = true;
                    break;           
                
            }
            if(!match)
            {
                for (int i = 1; i < statChangeNumbers.Count; i++)
                {
                    statChangeNumbers[i] = 0;
                }
                return;
            }
        }
        
        statChangeNumbers[1] = -parent.currentItem.hpChange;
        statChangeNumbers[2] = -parent.currentItem.pmChange;
        statChangeNumbers[3] = -parent.currentItem.atkChange;
        statChangeNumbers[4] = -parent.currentItem.defChange;
        statChangeNumbers[5] = -parent.currentItem.mgkAtkChange;
        statChangeNumbers[6] = -parent.currentItem.mgkDefChange;
        statChangeNumbers[7] = -parent.currentItem.luckChange;
        statChangeNumbers[8] = -parent.currentItem.evadeChange;
        statChangeNumbers[9] = -parent.currentItem.spdChange;
        
    }

    void UpdateStatChanges()
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

    public void EnableSubMenu()
    {
        parent.DeactivateMenu();
        parent.im.descriptionText.GetComponent<Renderer>().enabled = false;

        // create the text objects that will show the hero's stats
        SetStatChanges(GameControl.control.heroList[selectedIndex]);
        InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]);
        //UpdateStatChanges(GameControl.control.heroList[selectedIndex]);
        CheckForInactive();
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = true; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = true; }

        base.EnableSubMenu();
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        // if there are none to remove, don't activate
        if (parent.currentItem.uses <= 0) return;

        if (GameControl.control.whichInventory == "weapons") { parent.currentItem.GetComponent<WeaponItem>().Remove(GameControl.control.heroList[selectedIndex]); }
        else if (GameControl.control.whichInventory == "armor") { parent.currentItem.GetComponent<ArmorItem>().Remove(GameControl.control.heroList[selectedIndex]); }

        // after item use, close all submenus
        parent.ActivateMenu();
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
        DisableSubMenu();
        parent.im.ActivateMenu();
        parent.DisableSubMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible && frameDelay > 0)
        {
            UpdateStatChanges();
            //parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
            if (Input.GetKeyUp(GameControl.control.upKey) || Input.GetKeyUp(GameControl.control.downKey)) { SetStatChanges(GameControl.control.heroList[selectedIndex]); InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); }
            if (Input.GetKeyUp(GameControl.control.backKey) || Input.GetKeyUp(GameControl.control.pauseKey))
            {
                parent.ActivateMenu();
                foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
                foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = false; }
            }
        }

        base.Update();
    }

    void CheckForInactive()
    {
        // deactivate the hero if they don't have the current item
        for (int i = 0; i < buttonArray.Length; i++)
        {
            // check for same weapon
            if (GameControl.control.whichInventory == "weapons")
            {
                if (GameControl.control.heroList[i].weapon == null
                    || GameControl.control.heroList[i].weapon.GetComponent<Item>() != parent.currentItem)
                {
                    if (i == 0)
                    { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                    else
                    { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                }
                else
                {
                    if (i == 0)
                    { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover; }
                    else
                    { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
                }
            }
            // check if they have any of the equipment
            else if (GameControl.control.whichInventory == "armor")
            {
                // if there's no equipment, no need to remove or continue checking
                if (GameControl.control.heroList[i].equipment.Count <= 0)
                {
                    if (i == 0)
                        buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
                    else
                        buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
                    continue;
                }

                for (int j = 0; j < GameControl.control.heroList[i].equipment.Count; j++)
                {
                    if (GameControl.control.heroList[i].equipment[j].GetComponent<Item>() != parent.currentItem)
                    {
                        if (i == 0)
                            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
                        else
                            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
                    }
                    else
                    {
                        if (i == 0)
                            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
                        else
                            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
                    }
                }
            }
        }
    }
}
