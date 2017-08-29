using UnityEngine;
using System.Collections;

public class SkillTreeManager : MonoBehaviour {

    HeroData hero;
    //bool skillTree;

	// Use this for initialization
	void Start () {
        //skillTree = false;

        //Set the hero
        for (int i = 0; i < GameControl.control.heroList.Count; i++)
        {
            if (GameControl.control.heroList[i].skillTree)
            {
                GameControl.control.heroList[i].skillTree = false;
                hero = GameControl.control.heroList[i];
                // set specific skill tree
                switch (hero.identity)
                {
                    case 0:
                        gameObject.AddComponent<JethroSkillTree>();
                        break;
                    case 1:
                        gameObject.AddComponent<ColeSkillTree>();
                        break;
                    default:
                        break;
                }

                break;
            }
        }
	
	}
	
}
