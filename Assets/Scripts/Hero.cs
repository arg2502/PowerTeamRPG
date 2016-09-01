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
    protected int targetIndex2 = 0;
    protected int targetIndex3 = 0;

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
    public virtual void SelectTarget(string attack)
    {
        //clear any previously selected targets from other turns
        if (targets != null)
        {
            targets.Clear();
        }


        //this will use a switch statement to determine the type of
        //targeting required, and then pass off to a more specific method
        switch (attack)
        {
            case "Block":
                break;
            default:
                //SelectAllTargets();
                SelectSingleTarget();
                break;
        }
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
        if (Input.GetKeyUp(KeyCode.S))
        {
            //reset previous target's color
            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;

            targetIndex--;
			if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }

			// if enemy is dead, skip to next
			if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
				battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
				targetIndex--;
				if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }
			}

            
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //reset previous target's color
            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;

            targetIndex++;
			if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }

			// if enemy is dead, skip to next
			if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
				battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
				targetIndex++;
				if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
			}

            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            targets.Add(battleMenu.enemyList[targetIndex]);
            //deactivate the cursor
            battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled = false;
        }
        //move the cursor to the correct position
        battleMenu.cursors[0].transform.position = new Vector2(battleMenu.enemyList[targetIndex].transform.position.x, battleMenu.enemyList[targetIndex].transform.position.y + 100);

		// if enemy is dead, skip to next
		if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
			battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
			targetIndex++;
			if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
		}


		battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.red;
    }

    // this is a 3 target attack -- I believe this is fully functional
    public void SelectSplashTarget()
    {
        //Activate up to 3 cursors, less if there is less than 3 denigens
        int numOfCursors = 0;
        
        if (battleMenu.enemyList.Count < 3) { numOfCursors = battleMenu.enemyList.Count; }
        else { numOfCursors = 3; }
        for (int i = 0; i < numOfCursors; i++)
        {
            if (battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled == false)
            {
                battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            if (i > 0)
            {
                battleMenu.cursors[i].GetComponent<TargetCursor>().isSplash = true;
                battleMenu.cursors[i].GetComponent<TargetCursor>().ChangeSprite();
                if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex + 1, targetIndex + 2, 1); }
                if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex - 1, targetIndex, 2); }
            }
        }

        //handle input
        if (Input.GetKeyUp(KeyCode.S))
        {
            //reset previous target's color
            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;

            targetIndex--;
            if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }

            // if enemy is dead, skip to next
            if (battleMenu.enemyList[targetIndex].StatusState == Status.dead)
            {
                battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
                targetIndex--;
                if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }
            }

            // Move the other 2 cursors
            if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, targetIndex, 1); }
            if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, targetIndex + 2, 2); }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //reset previous target's color
            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;

            targetIndex++;
            if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }

            // if enemy is dead, skip to next
            if (battleMenu.enemyList[targetIndex].StatusState == Status.dead)
            {
                battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
                targetIndex++;
                if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
            }

            // Move the other 2 cursors
            if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, targetIndex - 2, 1); }
            if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, targetIndex, 2); }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            targets.Add(battleMenu.enemyList[targetIndex]);

            //add the splash targets, if they are within range
            if (numOfCursors > 1 && (targetIndex2 >= 0 && targetIndex2 < battleMenu.enemyList.Count)) { targets.Add(battleMenu.enemyList[targetIndex2]); }
            if (numOfCursors > 2 && (targetIndex3 >= 0 && targetIndex3 < battleMenu.enemyList.Count)) { targets.Add(battleMenu.enemyList[targetIndex3]); }

            //deactivate the cursors
            for (int i = 0; i < numOfCursors; i++)
            {
                battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //move the cursor to the correct position
        battleMenu.cursors[0].transform.position = new Vector2(battleMenu.enemyList[targetIndex].transform.position.x, battleMenu.enemyList[targetIndex].transform.position.y + 100);

        // if enemy is dead, skip to next
        if (battleMenu.enemyList[targetIndex].StatusState == Status.dead)
        {
            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
            targetIndex++;
            if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
        }


        battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.red;
    }

    // this method is for an attack that hits all possible targets
    public void SelectAllTargets()
    {
        //Activate as many cursors as there are enemies
        int numOfCursors = battleMenu.enemyList.Count;
        for (int i = 0; i < numOfCursors; i++)
        {
            if (battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled == false)
            {
                battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        
        // put a cursor above each target
        for (int i = 0; i < numOfCursors; i++)
        {
            //move the cursor to the correct position
            battleMenu.cursors[i].transform.position = new Vector2(battleMenu.enemyList[i].transform.position.x, battleMenu.enemyList[i].transform.position.y + 100);
        }

        // handle input
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //deactivate the cursors and select targets
            for (int i = 0; i < numOfCursors; i++)
            {
                if (battleMenu.enemyList[i].statusState != Status.dead)
                {
                    targets.Add(battleMenu.enemyList[i]);
                }
                battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    //supports the selectSplashTarget method
    public int MoveSecondaryCursor(int index, int prevIndex, int cursorIndex)
    {
        // highlight active target, only if theindex is within the scope of the enemy list
        if (index >= 0 && index < battleMenu.enemyList.Count)
        {
            battleMenu.enemyList[index].Card.GetComponent<TextMesh>().color = new Vector4(1.0f, 0.5f, 0.5f, 1.0f);
            // if the target is in the range of the enemy list count, draw the cursor above the appropriate enemy
            battleMenu.cursors[cursorIndex].transform.position = new Vector2(battleMenu.enemyList[index].transform.position.x, battleMenu.enemyList[index].transform.position.y + 100);
        }
        if (prevIndex >= 0 && prevIndex < battleMenu.enemyList.Count)
        {
            battleMenu.enemyList[prevIndex].Card.GetComponent<TextMesh>().color = Color.white;
        }

        // disable the cursor if it goes out of range
        if (index > battleMenu.enemyList.Count || index < 0)
        {
            battleMenu.cursors[cursorIndex].GetComponent<SpriteRenderer>().enabled = false;
        }
        // enable the cursor when it comes back into range
        if (prevIndex > battleMenu.enemyList.Count || prevIndex < 0)
        {
            battleMenu.cursors[cursorIndex].GetComponent<SpriteRenderer>().enabled = true;
        }
        return index;
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
        damage = CalcDamage("Strike", 0.5f, 0.1f, 0.95f, isMagic);
        targets[0].TakeDamage(damage, isMagic);
    }

    public virtual void Attack(string atkChoice)
    {
        // specific denigens will pick attack methods based off of user choice
        switch (atkChoice)
        {
            case "Strike":
			if(targets[0].StatusState != Status.dead) Strike();
                break;
            case "Block":
                base.Block();
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	protected void Update () {
        base.Update();
	}
}
