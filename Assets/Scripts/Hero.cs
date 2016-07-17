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

    // used for finding the player's intended target
    protected int targetIndex = 0;

    //properties
    public int Exp { get { return exp; } set { exp = value; } }
    public int LevelUpPts { get { return levelUpPts; } set { levelUpPts = value; } }
    public int TechPts { get { return techPts; } set { techPts = value; } }
    //property for items and equipment -- ADD LATER

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
        //clear any previously selected targets from other turns
        if (targets != null)
        {
            targets.Clear();
        }


        //this will use a switch statement to determine the type of
        //targeting required, and then pass off to a more specific method
        SelectSingleTarget();
    }

    //a generic, single target selecting method
    //any of this code can of course be changed later, this is just for testing purposes
    public void SelectSingleTarget()
    {
        //Activate the first cursor for targeting
        if(battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled == false)
        {
            battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled = true;
        }

        //handle input
        if (Input.GetKeyUp(KeyCode.W))
        {
            targetIndex--;
            if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }
            print("Target Index: " + targetIndex);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            targetIndex++;
            if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
            print("Target Index: " + targetIndex);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print("Added target " + targetIndex);
            targets.Add(battleMenu.enemyList[targetIndex]);
            //deactivate the cursor
            battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled = false;
        }
        //move the cursor to the correct position
        battleMenu.cursors[0].transform.position = new Vector2(battleMenu.enemyList[targetIndex].transform.position.x, battleMenu.enemyList[targetIndex].transform.position.y + 100);

        //print("Target Index: " + targetIndex);
    }

    //Strike is standard attack with 50% power
    //It uses no mana, and its magic properties are determined by the stat breakdown
    protected void Strike()
    {
        float damage;
        bool isMagic;
        if(atkPer > mgkAtkPer)
        {
            isMagic = false;
        }
        else
        {
            isMagic = true;
        }
        damage = CalcDamage(0.5f, 0.1f, 0.95f, isMagic);
        targets[0].TakeDamage(damage, isMagic);
    }

    public void Attack(string atkChoice)
    {
        // specific denigens will pick attack methods based off of user choice
        //print(name + " Attacks");
        switch (atkChoice)
        {
            case "Strike":
                Strike();
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
