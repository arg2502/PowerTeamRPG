using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUseSubMenu : SubMenu {

    ConsumableItemSubMenu parent;
    List<GameObject> heroInfo; // displays all manner of info for the hero - stats, passives, etc.
    List<int> statChangeNumbers; // stores the calculated stat changes an item will cause
    List<GameObject> statChanges; // displays any changes an item may cause

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
        for (int i = 10; i < h.passiveList.Count; i++) { heroInfo[i].GetComponent<TextMesh>().text = h.passiveList[i].Name; }
    }

    public void EnableSubMenu()
    {
        parent.DeactivateMenu();
        parent.im.descriptionText.GetComponent<Renderer>().enabled = false;

        // create the text objects that will show the hero's stats
        InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]);
        foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = true; }

        base.EnableSubMenu();
    }
	
	// Update is called once per frame
	void Update () {
        if (isVisible && frameDelay > 0) 
        {
            //parent.im.descriptionText.GetComponent<TextMesh>().text = buttonDescription[selectedIndex];
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) { InstantiateHeroInfo(GameControl.control.heroList[selectedIndex]); }
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q)) 
            {
                parent.ActivateMenu();
                foreach (GameObject go in heroInfo) { go.GetComponent<Renderer>().enabled = false; }
            }
        }
        
        base.Update();
	}
}
