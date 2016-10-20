using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUseSubMenu : SubMenu {

    ConsumableItemSubMenu parent;
    List<GameObject> heroInfo; // displays all manner of info for the hero - stats, passives, etc.
    List<int> statChangeNumbers; // stores the calculated stat changes an item will cause
    List<GameObject> statChanges; // displays any changes an item may cause
    Item currentItem;

	// Use this for initialization
	void Start () {
        parent = GameObject.FindObjectOfType<ConsumableItemSubMenu>();
        contentArray = new List<string>();
        

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

        // create the text objects that will show the hero's stats
        //InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]);

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
        heroInfo[1].GetComponent<TextMesh>().text = "Hp: " + h.hp + " / " + h.hpMax;
        heroInfo[2].GetComponent<TextMesh>().text = "Pm: " + h.pm + " / " + h.pmMax;
        heroInfo[3].GetComponent<TextMesh>().text = "Atk: " + h.atk;
        heroInfo[4].GetComponent<TextMesh>().text = "Def: " + h.def;
        heroInfo[5].GetComponent<TextMesh>().text = "Mgk Atk: " + h.mgkAtk;
        heroInfo[6].GetComponent<TextMesh>().text = "Mgk Def: " + h.mgkDef;
        heroInfo[7].GetComponent<TextMesh>().text = "Luck: " + h.luck;
        heroInfo[8].GetComponent<TextMesh>().text = "Evasion: " + h.evasion;
        heroInfo[9].GetComponent<TextMesh>().text = "Spd: " + h.spd;
        for (int i = 10; (i - 10) < h.passiveList.Count; i++) { heroInfo[i].GetComponent<TextMesh>().text = h.passiveList[i - 10].Name; }
    }

    // this method exists to be more efficient with memory - all items minimally have these 10 attributes
    public void UpdateStatChanges(HeroData h)
    {
        // draw the current item from the correct list of items
        if (GameControl.control.whichInventory == "consumables") 
        { 
            currentItem = GameControl.control.consumables[parent.itemIndex].GetComponent<Item>();

            statChanges[0].GetComponent<TextMesh>().text = currentItem.statusChange;
            if (currentItem.hpChange != 0) { statChanges[1].GetComponent<TextMesh>().text = "(" + currentItem.hpChange + ")"; }
            else { statChanges[1].GetComponent<TextMesh>().text = ""; }
            if (currentItem.pmChange != 0) { statChanges[2].GetComponent<TextMesh>().text = "(" + currentItem.pmChange + ")"; }
            else { statChanges[2].GetComponent<TextMesh>().text = ""; }
            if (currentItem.atkChange != 0) { statChanges[3].GetComponent<TextMesh>().text = "(" + currentItem.atkChange + ")"; }
            else { statChanges[3].GetComponent<TextMesh>().text = ""; }
            if (currentItem.defChange != 0) { statChanges[4].GetComponent<TextMesh>().text = "(" + currentItem.defChange + ")"; }
            else { statChanges[4].GetComponent<TextMesh>().text = ""; }
            if (currentItem.mgkAtkChange != 0) { statChanges[5].GetComponent<TextMesh>().text = "(" + currentItem.mgkAtkChange + ")"; }
            else { statChanges[5].GetComponent<TextMesh>().text = ""; }
            if (currentItem.mgkDefChange != 0) { statChanges[6].GetComponent<TextMesh>().text = "(" + currentItem.mgkDefChange + ")"; }
            else { statChanges[6].GetComponent<TextMesh>().text = ""; }
            if (currentItem.luckChange != 0) { statChanges[7].GetComponent<TextMesh>().text = "(" + currentItem.luckChange + ")"; }
            else { statChanges[7].GetComponent<TextMesh>().text = ""; }
            if (currentItem.evadeChange != 0) { statChanges[8].GetComponent<TextMesh>().text = "(" + currentItem.evadeChange + ")"; }
            else { statChanges[8].GetComponent<TextMesh>().text = ""; }
            if (currentItem.spdChange != 0) statChanges[9].GetComponent<TextMesh>().text = "(" + currentItem.spdChange + ")";
            else { statChanges[9].GetComponent<TextMesh>().text = ""; }
        }
        //else if (GameControl.control.whichInventory == "reusables") { currentItem = GameControl.control.reusables[parent.itemIndex].GetComponent<Item>(); }
        else if (GameControl.control.whichInventory == "weapons") 
        { 
            currentItem = GameControl.control.weapons[parent.itemIndex].GetComponent<Item>();
            print(parent.itemIndex);

            Item heroWeapon = null;
            if (h.weapon != null) { heroWeapon = h.weapon.GetComponent<Item>(); }

            if (h.weapon == null)
            {
                if (currentItem.hpChange != 0) { statChanges[1].GetComponent<TextMesh>().text = "(" + currentItem.hpChange + ")"; }
                else { statChanges[1].GetComponent<TextMesh>().text = ""; }
                if (currentItem.pmChange != 0) { statChanges[2].GetComponent<TextMesh>().text = "(" + currentItem.pmChange + ")"; }
                else { statChanges[2].GetComponent<TextMesh>().text = ""; }
                if (currentItem.atkChange != 0) { statChanges[3].GetComponent<TextMesh>().text = "(" + currentItem.atkChange + ")"; }
                else { statChanges[3].GetComponent<TextMesh>().text = ""; }
                if (currentItem.defChange != 0) { statChanges[4].GetComponent<TextMesh>().text = "(" + currentItem.defChange + ")"; }
                else { statChanges[4].GetComponent<TextMesh>().text = ""; }
                if (currentItem.mgkAtkChange != 0) { statChanges[5].GetComponent<TextMesh>().text = "(" + currentItem.mgkAtkChange + ")"; }
                else { statChanges[5].GetComponent<TextMesh>().text = ""; }
                if (currentItem.mgkDefChange != 0) { statChanges[6].GetComponent<TextMesh>().text = "(" + currentItem.mgkDefChange + ")"; }
                else { statChanges[6].GetComponent<TextMesh>().text = ""; }
                if (currentItem.luckChange != 0) { statChanges[7].GetComponent<TextMesh>().text = "(" + currentItem.luckChange + ")"; }
                else { statChanges[7].GetComponent<TextMesh>().text = ""; }
                if (currentItem.evadeChange != 0) { statChanges[8].GetComponent<TextMesh>().text = "(" + currentItem.evadeChange + ")"; }
                else { statChanges[8].GetComponent<TextMesh>().text = ""; }
                if (currentItem.spdChange != 0) statChanges[9].GetComponent<TextMesh>().text = "(" + currentItem.spdChange + ")";
                else { statChanges[9].GetComponent<TextMesh>().text = ""; }
            }
            else
            {
                if (currentItem.hpChange - heroWeapon.hpChange != 0) { statChanges[1].GetComponent<TextMesh>().text = "(" + (currentItem.hpChange - heroWeapon.hpChange) + ")"; }
                else { statChanges[1].GetComponent<TextMesh>().text = ""; }
                if (currentItem.pmChange - heroWeapon.pmChange != 0) { statChanges[2].GetComponent<TextMesh>().text = "(" + (currentItem.pmChange - heroWeapon.pmChange) + ")"; }
                else { statChanges[2].GetComponent<TextMesh>().text = ""; }
                if (currentItem.atkChange - heroWeapon.atkChange != 0) { statChanges[3].GetComponent<TextMesh>().text = "(" + (currentItem.atkChange - heroWeapon.atkChange) + ")"; }
                else { statChanges[3].GetComponent<TextMesh>().text = ""; }
                if (currentItem.defChange - heroWeapon.defChange != 0) { statChanges[4].GetComponent<TextMesh>().text = "(" + (currentItem.defChange - heroWeapon.defChange) + ")"; }
                else { statChanges[4].GetComponent<TextMesh>().text = ""; }
                if (currentItem.mgkAtkChange - heroWeapon.mgkAtkChange != 0) { statChanges[5].GetComponent<TextMesh>().text = "(" + (currentItem.mgkAtkChange - heroWeapon.mgkAtkChange) + ")"; }
                else { statChanges[5].GetComponent<TextMesh>().text = ""; }
                if (currentItem.mgkDefChange - heroWeapon.mgkDefChange != 0) { statChanges[6].GetComponent<TextMesh>().text = "(" + (currentItem.mgkDefChange - heroWeapon.mgkDefChange) + ")"; }
                else { statChanges[6].GetComponent<TextMesh>().text = ""; }
                if (currentItem.luckChange - heroWeapon.luckChange != 0) { statChanges[7].GetComponent<TextMesh>().text = "(" + (currentItem.luckChange - heroWeapon.luckChange) + ")"; }
                else { statChanges[7].GetComponent<TextMesh>().text = ""; }
                if (currentItem.evadeChange - heroWeapon.evadeChange != 0) { statChanges[8].GetComponent<TextMesh>().text = "(" + (currentItem.evadeChange - heroWeapon.evadeChange) + ")"; }
                else { statChanges[8].GetComponent<TextMesh>().text = ""; }
                if (currentItem.spdChange - heroWeapon.spdChange != 0) statChanges[9].GetComponent<TextMesh>().text = "(" + (currentItem.spdChange - heroWeapon.spdChange) + ")";
                else { statChanges[9].GetComponent<TextMesh>().text = ""; }
            }
        }
        else if (GameControl.control.whichInventory == "armor") { currentItem = GameControl.control.equipment[parent.itemIndex].GetComponent<Item>(); }
    }

    public void EnableSubMenu()
    {
        parent.DeactivateMenu();
        parent.im.descriptionText.GetComponent<Renderer>().enabled = false;

        // create the text objects that will show the hero's stats
        InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]);
        UpdateStatChanges(GameControl.control.heroList[selectedIndex]);
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = true; }
        foreach (GameObject go in statChanges) { go.GetComponent<Renderer>().enabled = true; }

        base.EnableSubMenu();
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisible && frameDelay > 0) 
        {
            //parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) { InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); UpdateStatChanges(GameControl.control.heroList[selectedIndex]); }
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
