using UnityEngine;
using System.Collections;

public class SkillTreeManager : MonoBehaviour {

    HeroData hero;
    bool skillTree;

	// Use this for initialization
	void Start () {
        skillTree = false;

        //Set the hero
        for (int i = 0; i < GameControl.control.heroList.Count; i++)
        {
            if (GameControl.control.heroList[i].skillTree)
            {
                GameControl.control.heroList[i].skillTree = false;
                hero = GameControl.control.heroList[i];
                print("Beginning of Start, Skill Count: " + hero.skillsList.Count);
                // set specific skill tree
                switch (hero.name)
                {
                    case "Jethro":
                        gameObject.AddComponent<JethroSkillTree>();
                        break;
                    case "Cole":
                        gameObject.AddComponent<ColeSkillTree>();
                        break;
                    default:
                        break;
                }

                break;
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        // test run through skill tree
        if(Input.GetKeyUp(KeyCode.T))
        {
            // Actually add the allocated points and move on to the next step of leveling up -- skills
            foreach (HeroData hd in GameControl.control.heroList)
            {
                /*if (hd.identity == hero.identity)
                {
                    hd.hpMax += statBoostInts[0];
                    hd.pmMax += statBoostInts[1];
                    hd.atk += statBoostInts[2];
                    hd.def += statBoostInts[3];
                    hd.mgkAtk += statBoostInts[4];
                    hd.mgkDef += statBoostInts[5];
                    hd.luck += statBoostInts[6];
                    hd.evasion += statBoostInts[7];
                    hd.spd += statBoostInts[8];
                    hd.levelUpPts = 0;
                }*/
                // also use this loop to figure out if we need to level up another hero
                if (hd.skillTree) { skillTree = true; }
            }

            if (skillTree == true) { UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTreeMenu"); }
            else { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
        }
	
	}
}
