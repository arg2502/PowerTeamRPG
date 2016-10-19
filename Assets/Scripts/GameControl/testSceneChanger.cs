using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testSceneChanger : MonoBehaviour {

    string nameList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
        
        if (GUI.Button(new Rect(10, 120, 150, 100), "Add Cole"))
        {
            GameControl.control.heroList.Add(new HeroData());
            GameControl.control.heroList[1].identity = 1;
            GameControl.control.heroList[1].name = "Cole";
            GameControl.control.heroList[1].level = 2;
            GameControl.control.heroList[1].expToLvlUp = 19;
            GameControl.control.heroList[1].exp = 0;
            GameControl.control.heroList[1].levelUpPts = 0;
            GameControl.control.heroList[1].techPts = 0;
            GameControl.control.heroList[1].hp = 14;
            GameControl.control.heroList[1].hpMax = 14;
            GameControl.control.heroList[1].pm = 10;
            GameControl.control.heroList[1].pmMax = 10;
            GameControl.control.heroList[1].atk = 5;
            GameControl.control.heroList[1].def = 4;
            GameControl.control.heroList[1].mgkAtk = 11;
            GameControl.control.heroList[1].mgkDef = 8;
            GameControl.control.heroList[1].luck = 5;
            GameControl.control.heroList[1].evasion = 4;
            GameControl.control.heroList[1].spd = 5;
            GameControl.control.heroList[1].skillsList = new List<Skill>();
            GameControl.control.heroList[1].spellsList = new List<Spell>();
            GameControl.control.heroList[1].passiveList = new List<Passive>();

            // the below code would probably be found in the "Add Spell" functions
            Spell hellfire = new Spell();
            hellfire.Name = "HellFire";
            hellfire.Pm = 8;
            hellfire.Description = "An all-encompassing spell with considerable power and accuracy. \n Str 40, Crit 03, Acc 100";
            GameControl.control.heroList[1].spellsList.Add(hellfire);

            Spell splashflame = new Spell();
            splashflame.Name = "Splash Flame";
            splashflame.Pm = 3;
            splashflame.Description = "An explosive fireball that deals light damage to enemies adjacent to the target. \n Str 60, Crit 05, Acc 85";
            GameControl.control.heroList[1].spellsList.Add(splashflame);

            Spell testtarget = new Spell();
            testtarget.Name = "Test Target";
            testtarget.Pm = 0;
            testtarget.Description = "Targets a hero. \n Str 0, Crit 0, Acc 100";
            GameControl.control.heroList[1].spellsList.Add(testtarget);
        }
       
        if (GUI.Button(new Rect(10, 240, 150, 100), "Load"))
        {
            GameControl.control.Load();
        }
        nameList = null;
        for (int i = 0; i < GameControl.control.heroList.Count; i++)
        {
            nameList += " " + GameControl.control.heroList[i].name;
        }
        GUI.Label(new Rect(10, 600, 1000, 1000), nameList);
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add Items"))
        {
            GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Restorative"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserElixir"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Elixir"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SpareSword"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TomeOfPractical"));
            GameControl.control.AddItem(temp);
            temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/HelmetOfFortitude"));
            GameControl.control.AddItem(temp);
        }
	}
}
