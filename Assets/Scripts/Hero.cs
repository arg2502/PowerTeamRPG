using UnityEngine;
using System.Collections;

public class Hero: Denigen {

    // Hero stats
    protected int exp;

    // item/equipment list - NEED LATER

    // skill tree variable - NEED LATER
    // skill tree will have to be specific to the character somehow

    // allocating levelup points
    protected int levelUpPts;

    // points for unlocking techniques
    protected int techPts;

	// Use this for initialization
	protected void Start () {
        // all heroes have 3 stars
        stars = 3;
        techPts = 0;        
        base.Start();
	}

    protected void LevelUp()
    {
        base.LevelUp(level);

        // allocate stats
        levelUpPts = (int)(stars * multiplier); // 3 * (level/10 + 1)

        // actually allocating the points will be done through a levelup menu

        // increase technique points each level up
        techPts++; 

    }

    // add skill to list
    protected void AddSkill(string skill, string descrip)
    {
        skillsList.Add(skill);
        skillsDescription.Add(descrip);
    }

    // add spell to list
    protected void AddSpell(string spell, string descrip)
    {
        spellsList.Add(spell);
        spellsDescription.Add(descrip);
    }

    //select the target for your attack
    public void SelectTarget(string attack)
    {
        //this will use a switch statement to determine the type of
        //targeting required, and then pass off to a more specific method
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
