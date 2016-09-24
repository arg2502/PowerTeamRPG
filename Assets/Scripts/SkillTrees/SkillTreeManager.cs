using UnityEngine;
using System.Collections;

public class SkillTreeManager : MonoBehaviour {

    HeroData hero;

	// Use this for initialization
	void Start () {
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
                    default:
                        break;
                }

                break;
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
