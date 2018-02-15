using UnityEngine;
using System.Collections;

public class Hero : Denigen {
    // this variable makes the hero unique (more than a name), and will be used for maintaining persistency
    //public int identity;

    // Hero stats
    //protected int exp;

    //the amount of experience required to level up
    //protected int expToLvlUp;
    //public bool statBoost = false;
    //public bool skillTree = false;

    // item/equipment list - NEED LATER

    // allocating levelup points
    protected int levelUpPts;

    // points for unlocking techniques
    protected int techPts;

    // used for finding the player's intended target
    protected int targetIndex = 0;
    protected int prevTargetIndex1 = 0;
    protected int prevTargetIndex2 = 0;
    protected int targetIndex2 = 0;
    protected int targetIndex3 = 0;

    //properties
    public int TargetIndex { get { return targetIndex; } set { targetIndex = value; } }
    public int LevelUpPts { get { return levelUpPts; } set { levelUpPts = value; } }
    public int TechPts { get { return techPts; } set { techPts = value; } }




    // Use this for initialization
    protected new void Awake () {
        // all heroes have 3 stars
        //stars = 2; // DO THIS ELSEWHERE
        base.Awake();
    }

    //public void LevelUp(int rollover = 0)
    //{
    //    data.LevelUp(Level);
    //    Level++;

    //    // allocate stats
    //    levelUpPts += (int)(Stars * Multiplier); // 3 * (level/10 + 1)

    //    // actually allocating the points will be done through a levelup menu

    //    // increase technique points each level up
    //    techPts++; 

    //    //calc new required points to level up
    //    expToLvlUp = CalcExpToLvlUp(rollover);
    //}

    //protected void LevelUpOnAwake(int startingLevel)
    //{
    //    // minus 1 because it will level up "startingLevel" number of times
    //    // we already start at 1, so we wanna start at (startingLevel-1)
    //    for (int i = 0; i < startingLevel - 1; i++)
    //    {
    //        LevelUp();
    //    }
    //}

    //protected int CalcExpToLvlUp(int rollover = 0)
    //{
    //    float expToGo = 0;
    //    expToGo = Level * growthSpeed * 10;
    //    return ((int) expToGo - rollover);
    //}

    // add skill to list
    protected void AddSkill(string skill, string descrip)
    {
        //skillsList.Add(skill);
        //skillsDescription.Add(descrip);
    }

    // add spell to list
    protected void AddSpell(string spell, string descrip)
    {
        //spellsList.Add(spell);
        //spellsDescription.Add(descrip);
    }

    // the method for handling item use
    public void ItemUse( string itemName)
    {
        // this switch statement will separate items that can be used on fallen heroes from items which cannot
        // the default case will handle every item not usable on fallen enemies
        // all other cases will have the name of specific items (Ex: "Revive")
        switch (itemName)
        {
            default:
                if (targets[0].StatusState != Status.dead && targets[0].StatusState != Status.overkill)
                {
                    if (targets.Count > 1 || name == targets[0].name) { calcDamageText.Add(name + " uses " + itemName + "!"); }
                    else { calcDamageText.Add(name + " uses " + itemName + " on " + targets[0].name + "!"); }

                    //search through all of the items
                    for (int j = 0; j < GameControl.control.consumables.Count; j++)
                    {
                        // Disable an item if the number of denigens commanded to use said item is >= its quantity
                        if (itemName == GameControl.control.consumables[j].GetComponent<ConsumableItem>().name)
                        {
                            GameControl.control.consumables[j].GetComponent<ConsumableItem>().Use(targets[0]);
                        }
                    }
                }
                else
                {
                    calcDamageText.Add(name + " tried to use " + itemName + " on " + targets[0].name + ", but it would have no effect. "
                        + name + " put the item away.");
                }
                break;
        }
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
                SelectSelfTarget(attack);
                break;
            default:
                // the default cases will be used for items
                break;
        }
    }

    //a generic, single target selecting method
    //any of this code can of course be changed later, this is just for testing purposes
    public void SelectSingleTarget()
    {
        //Activate the first cursor for targeting
        /*if(battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled == false)
        {
            battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled = true;
        }*/

        // Avoid starting on a dead target

        // COMMENT FOR NOW
        // 2/14/18 -- AG


  //      while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
  //      {
  //          battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //          battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //          battleMenu.enemyList[targetIndex].Sr.color = invisible;
  //          foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
  //          targetIndex--;
  //          if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }
            
  //      }

  //      //handle input
  //      if (Input.GetKeyUp(GameControl.control.downKey))
  //      {
  //          //reset previous target's color
  //          battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //          battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //          if (battleMenu.enemyList[targetIndex].StatusState != Status.dead && battleMenu.enemyList[targetIndex].StatusState != Status.overkill) { battleMenu.enemyList[targetIndex].Sr.color = Color.white; }
  //          targetIndex--;
		//	if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }

		//	// if enemy is dead, skip to next
  //          while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
  //          {
		//	//if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
		//		battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //              battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //              battleMenu.enemyList[targetIndex].Sr.color = invisible;
		//		targetIndex--;
		//		if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }
		//	}

            
  //      }
  //      if (Input.GetKeyUp(GameControl.control.upKey))
  //      {
  //          //reset previous target's color
  //          battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //          battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //          if (battleMenu.enemyList[targetIndex].StatusState != Status.dead && battleMenu.enemyList[targetIndex].StatusState != Status.overkill) { battleMenu.enemyList[targetIndex].Sr.color = Color.white; }
  //          targetIndex++;
		//	if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }

		//	// if enemy is dead, skip to next
  //          while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
  //          {
		//	//if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
		//		battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //              battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //              battleMenu.enemyList[targetIndex].Sr.color = invisible;
		//		targetIndex++;
		//		if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
		//	}

            
  //      }

  //      battleMenu.enemyList[targetIndex].Sr.material.shader = targetShader;
  //      battleMenu.enemyList[targetIndex].Sr.color = targetRed;

  //      if (Input.GetKeyUp(GameControl.control.selectKey))
  //      {
  //          targets.Add(battleMenu.enemyList[targetIndex]);
  //          battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
  //          battleMenu.enemyList[targetIndex].Sr.color = Color.white;
  //          targetIndex = 0;
  //          //deactivate the cursor
  //          //battleMenu.cursors[0].GetComponent<SpriteRenderer>().enabled = false;
  //      }

  //      //move the cursor to the correct position
  //      //battleMenu.cursors[0].transform.position = new Vector2(battleMenu.enemyList[targetIndex].transform.position.x, battleMenu.enemyList[targetIndex].transform.position.y + 100);

		//// if enemy is dead, skip to next
  //      //if (battleMenu.enemyList [targetIndex].StatusState == Status.dead) {
  //      //    battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
  //      //    battleMenu.enemyList[targetIndex].Sr.color = Color.white;
  //      //    targetIndex++;
  //      //    if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
  //      //}

		//battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.red;
    }

    // this is a 3 target attack
    public void SelectSplashTarget()
    {
        //Activate up to 3 cursors, less if there is less than 3 denigens
        int numOfCursors = 0;

        // Avoid starting on a dead target


        // COMMENT FOR NOW
        // 2/14/18 -- AG

        //while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        //{
        //    battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //    battleMenu.enemyList[targetIndex].Sr.color = invisible;
        //    foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //    targetIndex--;
        //    if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }

        //    if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex + 1, targetIndex + 2, 1); }
        //    if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex - 1, targetIndex, 2); }
        //    prevTargetIndex1 = targetIndex + 1;
        //    prevTargetIndex2 = targetIndex - 1;
        //}

        //if (battleMenu.enemyList.Count < 3) { numOfCursors = battleMenu.enemyList.Count; }
        //else { numOfCursors = 3; }
        //for (int i = 0; i < numOfCursors; i++)
        //{
        //    /*if (battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled == false)
        //    {
        //        battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = true;
        //    }*/
        //    if (i > 0)
        //    {
        //        //battleMenu.cursors[i].GetComponent<TargetCursor>().isSplash = true;
        //        //battleMenu.cursors[i].GetComponent<TargetCursor>().ChangeSprite();
        //        if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex + 1, targetIndex + 2, 1); }
        //        if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex - 1, targetIndex, 2); }
        //        prevTargetIndex1 = targetIndex + 1;
        //        prevTargetIndex2 = targetIndex - 1;
        //    }
        //}
        
        //handle input
        //if (Input.GetKeyUp(GameControl.control.downKey))
        //{
        //    //reset previous target's color
        //    battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //    if (battleMenu.enemyList[targetIndex].StatusState != Status.dead && battleMenu.enemyList[targetIndex].StatusState != Status.overkill) { battleMenu.enemyList[targetIndex].Sr.color = Color.white; }

        //    targetIndex--;
        //    if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1;
        //    foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //    }

        //    // if enemy is dead, skip to next
        //    if (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        //    {
        //        //int loopCount = 0;
        //        while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        //        {
        //            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //            battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //            battleMenu.enemyList[targetIndex].Sr.color = invisible;
        //            foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //            targetIndex--;
        //            if (targetIndex < 0) { targetIndex = battleMenu.enemyList.Count - 1; }

        //            // Move the other 2 cursors
        //            if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, prevTargetIndex1, 1); }
        //            if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, prevTargetIndex2, 2); }
        //            prevTargetIndex1 = targetIndex + 1;
        //            prevTargetIndex2 = targetIndex - 1;
        //            //loopCount++;
        //            //if (loopCount > battleMenu.enemyList.Count) { break; }
        //        }
        //    }

        //    // Move the other 2 cursors
        //    //if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, targetIndex, 1); }
        //    //if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, targetIndex + 2, 2); }
        //    if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, prevTargetIndex1, 1); }
        //    if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, prevTargetIndex2, 2); }
        //    prevTargetIndex1 = targetIndex + 1;
        //    prevTargetIndex2 = targetIndex - 1;
        //}
        //if (Input.GetKeyUp(GameControl.control.upKey))
        //{
        //    //reset previous target's color
        //    battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //    if (battleMenu.enemyList[targetIndex].StatusState != Status.dead && battleMenu.enemyList[targetIndex].StatusState != Status.overkill) { battleMenu.enemyList[targetIndex].Sr.color = Color.white; }

        //    targetIndex++;
        //    if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0;
        //    foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //    }

        //    // if enemy is dead, skip to next
        //    if (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        //    {
        //        while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        //        {
        //            battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //            battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //            battleMenu.enemyList[targetIndex].Sr.color = invisible;
        //            foreach (Enemy e in battleMenu.enemyList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //            targetIndex++;
        //            if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }

        //            // Move the other 2 cursors
        //            if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, prevTargetIndex1, 1); }
        //            if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, prevTargetIndex2, 2); }
        //            prevTargetIndex1 = targetIndex + 1;
        //            prevTargetIndex2 = targetIndex - 1;
        //        }
        //    }

            // Move the other 2 cursors
            //if (numOfCursors > 1) { targetIndex2 = MoveSecondaryCursor(targetIndex - 1, prevTargetIndex1, 1); }
            //if (numOfCursors > 2) { targetIndex3 = MoveSecondaryCursor(targetIndex + 1, prevTargetIndex2, 2); }
            //prevTargetIndex1 = targetIndex + 1;
            //prevTargetIndex2 = targetIndex - 1;
        //}

        // if enemy is dead, skip to next
        //while (battleMenu.enemyList[targetIndex].StatusState == Status.dead || battleMenu.enemyList[targetIndex].StatusState == Status.overkill)
        ////if (battleMenu.enemyList[targetIndex].StatusState == Status.dead)
        //{
        //    battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.enemyList[targetIndex].Sr.material.shader = normalShader;
        //    battleMenu.enemyList[targetIndex].Sr.color = Color.white;
        //    targetIndex++;
        //    if (targetIndex >= battleMenu.enemyList.Count) { targetIndex = 0; }
        //}


        //battleMenu.enemyList[targetIndex].Card.GetComponent<TextMesh>().color = Color.red;
        //battleMenu.enemyList[targetIndex].Sr.material.shader = targetShader;
        //battleMenu.enemyList[targetIndex].Sr.color = targetRed;

        //if (Input.GetKeyUp(GameControl.control.selectKey))
        //{
        //    for (int i = 0; i < battleMenu.enemyList.Count; i++)
        //    {
        //        battleMenu.enemyList[i].Sr.material.shader = normalShader;
        //        if (battleMenu.enemyList[i].statusState != Status.dead && battleMenu.enemyList[i].statusState != Status.overkill) battleMenu.enemyList[i].Sr.color = Color.white;
        //    }
        //    targets.Add(battleMenu.enemyList[targetIndex]);

        //    //add the splash targets, if they are within range
        //    if (numOfCursors > 1 && (targetIndex2 >= 0 && targetIndex2 < battleMenu.enemyList.Count) && 
        //        (battleMenu.enemyList[targetIndex2].statusState != Status.dead && battleMenu.enemyList[targetIndex2].statusState != Status.overkill)) { targets.Add(battleMenu.enemyList[targetIndex2]); }

        //    if (numOfCursors == 2 && !(targetIndex2 >= 0 && targetIndex2 < battleMenu.enemyList.Count) &&
        //        (battleMenu.enemyList[0].statusState != Status.dead && battleMenu.enemyList[0].statusState != Status.overkill)) { targets.Add(battleMenu.enemyList[0]); }

        //    if (numOfCursors > 2 && (targetIndex3 >= 0 && targetIndex3 < battleMenu.enemyList.Count) &&
        //        (battleMenu.enemyList[targetIndex3].statusState != Status.dead && battleMenu.enemyList[targetIndex3].statusState != Status.overkill)) { targets.Add(battleMenu.enemyList[targetIndex3]); }

        //    targetIndex = 0;
        //    //deactivate the cursors
        //    //for (int i = 0; i < numOfCursors; i++)
        //    //{
        //    //    battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = false;
        //    //}
        //}
        //move the cursor to the correct position
        //battleMenu.cursors[0].transform.position = new Vector2(battleMenu.enemyList[targetIndex].transform.position.x, battleMenu.enemyList[targetIndex].transform.position.y + 100);
    }

    // this method is for an attack that hits all possible targets
    public void SelectAllTargets()
    {

        // COMMENT FOR NOW
        // 2/14/18 -- AG

        //Activate as many cursors as there are enemies
        //int numOfCursors = battleMenu.enemyList.Count;
        ///*for (int i = 0; i < numOfCursors; i++)
        //{
        //    if (battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled == false)
        //    {
        //        battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = true;
        //    }
        //}*/

        //// put a cursor above each target
        ///*for (int i = 0; i < numOfCursors; i++)
        //{
        //    //move the cursor to the correct position
        //    battleMenu.cursors[i].transform.position = new Vector2(battleMenu.enemyList[i].transform.position.x, battleMenu.enemyList[i].transform.position.y + 100);
        //}*/

        //// tint the enemies red to indicate targeting
        //for (int i = 0; i < battleMenu.enemyList.Count; i++ )
        //{
        //    if (battleMenu.enemyList[i].statusState != Status.dead && battleMenu.enemyList[i].statusState != Status.overkill)
        //    { 
        //        battleMenu.enemyList[i].Sr.material.shader = targetShader;
        //        battleMenu.enemyList[i].Sr.color = targetRed;
        //        battleMenu.enemyList[i].Card.GetComponent<TextMesh>().color = Color.red;
        //    }
        //}

        //// handle input
        //if (Input.GetKeyUp(GameControl.control.selectKey))
        //{
        //    //deactivate the cursors and select targets
        //    for (int i = 0; i < numOfCursors; i++)
        //    {
        //        if (battleMenu.enemyList[i].statusState != Status.dead && battleMenu.enemyList[i].statusState != Status.overkill)
        //        {
        //            targets.Add(battleMenu.enemyList[i]);
        //            battleMenu.enemyList[i].Sr.material.shader = normalShader;
        //            battleMenu.enemyList[i].Sr.color = Color.white;
        //        }
        //        //battleMenu.cursors[i].GetComponent<SpriteRenderer>().enabled = false;
        //    }
        //    targetIndex = 0;
        //}
    }

    //supports the selectSplashTarget method
    public int MoveSecondaryCursor(int index, int prevIndex, int cursorIndex)
    {

        // COMMENT FOR NOW
        // 2/14/18 -- AG

        // highlight active target, only if the index is within the scope of the enemy list
        //if (index >= 0 && index < battleMenu.enemyList.Count && (battleMenu.enemyList[index].statusState != Status.dead && battleMenu.enemyList[index].statusState != Status.overkill))
        //{
        //    battleMenu.enemyList[index].Card.GetComponent<TextMesh>().color = new Vector4(1.0f, 0.5f, 0.5f, 1.0f);
        //    battleMenu.enemyList[index].Sr.material.shader = normalShader;
        //    battleMenu.enemyList[index].Sr.color = splashTargetRed;
        //    // if the target is in the range of the enemy list count, draw the cursor above the appropriate enemy
        //    //battleMenu.cursors[cursorIndex].transform.position = new Vector2(battleMenu.enemyList[index].transform.position.x, battleMenu.enemyList[index].transform.position.y + 100);
        //}
        //if (prevIndex >= 0 && prevIndex < battleMenu.enemyList.Count && (battleMenu.enemyList[prevIndex].statusState != Status.dead && battleMenu.enemyList[prevIndex].statusState != Status.overkill))
        //{
        //    battleMenu.enemyList[prevIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.enemyList[prevIndex].Sr.material.shader = normalShader;
        //    battleMenu.enemyList[prevIndex].Sr.color = Color.white;
        //}

        // disable the cursor if it goes out of range
        /*if (index > battleMenu.enemyList.Count || index < 0)
        {
            battleMenu.cursors[cursorIndex].GetComponent<SpriteRenderer>().enabled = false;
        }
        // enable the cursor when it comes back into range
        if (prevIndex > battleMenu.enemyList.Count || prevIndex < 0)
        {
            battleMenu.cursors[cursorIndex].GetComponent<SpriteRenderer>().enabled = true;
        }*/
        return index;
    }

    // This is used for self buffs, self healing, item use, and blocking
    public void SelectSelfTarget(string attack)
    {
        sr.material.shader = targetShader;
        sr.color = targetGreen;
        card.GetComponent<TextMesh>().color = Color.green;

        if (Input.GetKeyUp(GameControl.control.selectKey))
        {
            if (attack == "Block") { isBlocking = true; }
            else { targets[0] = this; }
            sr.material.shader = normalShader;
            sr.color = Color.white;
        }
    }

    //a generic, single target selecting method
    //any of this code can of course be changed later, this is just for testing purposes
    public void SelectSingleTeamTarget(string atkChoice)
    {

        // COMMENT FOR NOW
        // 2/14/18 -- AG

        // Avoid starting on a dead target
        //while (battleMenu.heroList[targetIndex].StatusState == Status.dead || battleMenu.heroList[targetIndex].StatusState == Status.overkill)
        //{
        //    battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //    battleMenu.heroList[targetIndex].Sr.color = invisible;
        //    foreach (Hero e in battleMenu.heroList) { e.Card.GetComponent<TextMesh>().color = Color.white; }
        //    targetIndex--;
        //    if (targetIndex < 0) { targetIndex = battleMenu.heroList.Count - 1; }

        //}

        ////handle input
        //if (Input.GetKeyUp(GameControl.control.downKey))
        //{
        //    //reset previous target's color
        //    battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //    if (battleMenu.heroList[targetIndex].StatusState != Status.dead && battleMenu.heroList[targetIndex].StatusState != Status.overkill) { battleMenu.heroList[targetIndex].Sr.color = Color.white; }
        //    targetIndex--;
        //    if (targetIndex < 0) { targetIndex = battleMenu.heroList.Count - 1; }

        //    // if hero is dead, skip to next
        //    while (battleMenu.heroList[targetIndex].StatusState == Status.dead || battleMenu.heroList[targetIndex].StatusState == Status.overkill)
        //    {
        //        //if (battleMenu.heroList [targetIndex].StatusState == Status.dead) {
        //        battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //        battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //        battleMenu.heroList[targetIndex].Sr.color = invisible;
        //        targetIndex--;
        //        if (targetIndex < 0) { targetIndex = battleMenu.heroList.Count - 1; }
        //    }


        //}
        //if (Input.GetKeyUp(GameControl.control.upKey))
        //{
        //    //reset previous target's color
        //    battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //    battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //    if (battleMenu.heroList[targetIndex].StatusState != Status.dead && battleMenu.heroList[targetIndex].StatusState != Status.overkill) { battleMenu.heroList[targetIndex].Sr.color = Color.white; }
        //    targetIndex++;
        //    if (targetIndex >= battleMenu.heroList.Count) { targetIndex = 0; }

        //    // if hero is dead, skip to next
        //    while (battleMenu.heroList[targetIndex].StatusState == Status.dead || battleMenu.heroList[targetIndex].StatusState == Status.overkill)
        //    {
        //        //if (battleMenu.heroList [targetIndex].StatusState == Status.dead) {
        //        battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.white;
        //        battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //        battleMenu.heroList[targetIndex].Sr.color = invisible;
        //        targetIndex++;
        //        if (targetIndex >= battleMenu.heroList.Count) { targetIndex = 0; }
        //    }


        //}

        //battleMenu.heroList[targetIndex].Sr.material.shader = targetShader;
        //battleMenu.heroList[targetIndex].Sr.color = targetGreen;

        //if (Input.GetKeyUp(GameControl.control.selectKey))
        //{
        //    targets.Add(battleMenu.heroList[targetIndex]);
        //    battleMenu.heroList[targetIndex].Sr.material.shader = normalShader;
        //    battleMenu.heroList[targetIndex].Sr.color = Color.white;

        //    //if the command is to use an item, we must increase its uses variable
        //    //search through all of the items
        //    for (int j = 0; j < GameControl.control.consumables.Count; j++)
        //    {
        //        // Disable an item if the number of denigens commanded to use said item is >= its quantity
        //        if (atkChoice == GameControl.control.consumables[j].GetComponent<ConsumableItem>().name)
        //        {
        //            GameControl.control.consumables[j].GetComponent<ConsumableItem>().uses += 1;
        //        }
        //    }

        //    targetIndex = 0;
        //}

        //battleMenu.heroList[targetIndex].Card.GetComponent<TextMesh>().color = Color.green;
    }

    // this method is for an attack that hits all possible teammates
    public void SelectAllTeamTargets(string atkChoice)
    {

        // COMMENT FOR NOW
        // 2/14/18 -- AG


        //Activate as many cursors as there are enemies
        //int numOfCursors = battleMenu.heroList.Count;

        //// tint the enemies red to indicate targeting
        //for (int i = 0; i < battleMenu.heroList.Count; i++)
        //{
        //    if (battleMenu.heroList[i].statusState != Status.dead && battleMenu.heroList[i].statusState != Status.overkill)
        //    {
        //        battleMenu.heroList[i].Sr.material.shader = targetShader;
        //        battleMenu.heroList[i].Sr.color = targetGreen;
        //        battleMenu.heroList[i].Card.GetComponent<TextMesh>().color = Color.green;
        //    }
        //}

        //// handle input
        //if (Input.GetKeyUp(GameControl.control.selectKey))
        //{
        //    //deactivate the cursors and select targets
        //    for (int i = 0; i < numOfCursors; i++)
        //    {
        //        if (battleMenu.heroList[i].statusState != Status.dead && battleMenu.heroList[i].statusState != Status.overkill)
        //        {
        //            targets.Add(battleMenu.heroList[i]);
        //            battleMenu.heroList[i].Sr.material.shader = normalShader;
        //            battleMenu.heroList[i].Sr.color = Color.white;
        //        }
        //    }
        //    targetIndex = 0;
        //}
    }

    //Strike is standard attack with 50% power
    //It uses no mana, and its magic properties are determined by the stat breakdown
    protected void Strike()
    {
        float damage;
        bool isMagic;
        if(data.atkPer > data.mgkAtkPer)
        {
            isMagic = false;
        }
        else
        {
            isMagic = true;
        }
        damage = CalcDamage("Strike", 0.3f, 0.1f, 0.95f, isMagic);
        targets[0].TakeDamage(this, damage, isMagic);
    }

    public new virtual void Attack(string atkChoice)
    {

        // COMMENT FOR NOW
        // 2/14/18 -- AG

        //while (targets.Count == 1 && (targets[0].StatusState == Denigen.Status.dead || targets[0].StatusState == Denigen.Status.overkill))
        //{
        //    for (int i = 0; i < battleMenu.enemyList.Count; i++)
        //    {
        //        if (battleMenu.enemyList[i].statusState != Status.dead && battleMenu.enemyList[i].statusState != Denigen.Status.overkill)
        //        {
        //            targets[0] = battleMenu.enemyList[i];
        //        }
        //    }
        //}


        // specific denigens will pick attack methods based off of user choice
        //switch (atkChoice)
        //{
        //    case "Strike":
        //    if(targets[0].StatusState != Status.dead && targets[0].StatusState != Status.overkill) Strike();
        //        break;
        //    case "Block":
        //        base.Block();
        //        break;
        //    default:
        //        // if there is no case for this action, then it must be treated as an item
        //        //ItemUse(atkChoice);
        //        break;
        //}

        //subtract the appropriate pm from the attacker -- this value will remain 0 for strike and block
        //go through all techniques to find the correct value
        for (int i = 0; i < SkillsList.Count; i++ )
        {
            if (SkillsList[i].Name == atkChoice) { Pm -= SkillsList[i].Pm; break; }
        }
        for (int i = 0; i < SpellsList.Count; i++)
        {
            if (SpellsList[i].Name == atkChoice) { Pm -= SpellsList[i].Pm; break; }
        }
    }
    
}