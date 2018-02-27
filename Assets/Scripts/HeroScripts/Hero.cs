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
            case "Strike":
                SelectSingleTarget();
                break;
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
        // TEST -- RANDOMLY PICK FOR NOW
        int num = 0;
        num = Random.Range(0, battleManager.enemyList.Count);
        targetIndex = num;
        targets.Add(battleManager.enemyList[targetIndex]);        
    }

    // this is a 3 target attack
    public void SelectSplashTarget()
    {

    }

    // this method is for an attack that hits all possible targets
    public void SelectAllTargets()
    {
        
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

    
    public override void Attack(string atkChoice)
    {
        // check if the target is still alive, if not -- find a new one
        IsTargetDead();

        // specific denigens will pick attack methods based off of user choice
        switch (atkChoice)
        {
            case "Strike":
                Strike();          
                break;
            case "Block":
                Block();
                break;
            default:
                // if there is no case for this action, then it must be treated as an item
                //ItemUse(atkChoice);
                break;
        }

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
    //Strike is standard attack with 50% power
    //It uses no mana, and its magic properties are determined by the stat breakdown
    protected void Strike()
    {
        float damage;
        bool isMagic;
        if (data.atkPer > data.mgkAtkPer)
        {
            isMagic = false;
        }
        else
        {
            isMagic = true;
        }
        damage = CalcDamage("Strike", 0.3f, 0.1f, 0.95f, isMagic);
        print(name + " strikes! " + damage + " damage");
        targets[0].TakeDamage(this, damage, isMagic);
    }

    // if the current target is dead, clear target list and find new single target
    void IsTargetDead()
    {
        // if they're not dead, stop right here
        if (!targets[0].IsDead)
            return;

        print("Target is dead -- find new target");

        targets.Clear();
        
        for(int i = 1; i < battleManager.enemyList.Count; i++)
        {
            // check below
            if (targetIndex > 0 && !battleManager.enemyList[targetIndex - i].IsDead)
            {
                targetIndex -= i;
                targets.Add(battleManager.enemyList[targetIndex]);
                break;
            }

            // check above
            if (targetIndex < battleManager.enemyList.Count - 1 && !battleManager.enemyList[targetIndex + i].IsDead)
            {
                targetIndex += i;
                targets.Add(battleManager.enemyList[targetIndex]);
                break;
            }
         
        }
        
    }
}