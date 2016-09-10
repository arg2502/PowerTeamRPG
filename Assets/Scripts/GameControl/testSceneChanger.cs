using UnityEngine;
using System.Collections;

public class testSceneChanger : MonoBehaviour {

    string nameList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Go to battle"))
        {
            GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene("testMenu");
        }
        if (GUI.Button(new Rect(10, 120, 150, 100), "Add Cole"))
        {
            GameControl.control.heroList.Add(new HeroData());
            GameControl.control.heroList[1].name = "Cole";
            GameControl.control.heroList[1].level = 2;
            GameControl.control.heroList[1].expToLvlUp = 20;
            GameControl.control.heroList[1].exp = 0;
            GameControl.control.heroList[1].levelUpPts = 0;
            GameControl.control.heroList[1].techPts = 0;
            GameControl.control.heroList[1].hp = 18;
            GameControl.control.heroList[1].hpMax = 18;
            GameControl.control.heroList[1].pm = 17;
            GameControl.control.heroList[1].pmMax = 17;
            GameControl.control.heroList[1].atk = 9;
            GameControl.control.heroList[1].def = 6;
            GameControl.control.heroList[1].mgkAtk = 18;
            GameControl.control.heroList[1].mgkDef = 13;
            GameControl.control.heroList[1].luck = 10;
            GameControl.control.heroList[1].evasion = 8;
            GameControl.control.heroList[1].spd = 9;
        }
        if (GUI.Button(new Rect(10, 240, 150, 100), "Save"))
        {
            GameControl.control.Save();
        }
        if (GUI.Button(new Rect(10, 360, 150, 100), "Load"))
        {
            GameControl.control.Load();
        }
        nameList = null;
        for (int i = 0; i < GameControl.control.heroList.Count; i++)
        {
            nameList += " " + GameControl.control.heroList[i].name;
        }
        GUI.Label(new Rect(10, 600, 1000, 1000), nameList);
	}
}
