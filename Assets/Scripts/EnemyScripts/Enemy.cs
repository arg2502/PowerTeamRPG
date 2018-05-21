﻿using UnityEngine;
using System.Collections;

public class Enemy : Denigen {

    //Amount of experience awarded for defeating this enemy
    int exp;
    // A number for tweaking the amount of experience rewarded, based on species
    //protected int expMultiplier;

    public int ExpGiven { get { return exp; } }

    //amount of gold awarded for defeating this enemy
    int gold;
    //a number for tweaking the amount of gold awarded, based on species
    //protected int goldMultiplier;

    public int Gold { get { return gold; } }

    //Median level for enemies in this region, dependant on player's team's highest level when they first entered this region.
    //Will probably be pulled from an array
    int areaLevel;

    //States dependant on health, to influence the enemy decision making. This should make them appear smarter
    protected enum Health { high, average, low, dangerous};
    protected Health healthState = Health.high;

    EnemyData enemyData;
    public int ExpMultiplier { get { return enemyData.expMultiplier; } set { enemyData.expMultiplier = value; } }
    public int GoldMultiplier { get { return enemyData.goldMultiplier; } set { enemyData.goldMultiplier = value; } }

	// Use this for initialization
	public void Init () {
        //set the base stats for the enemy
        base.Awake();

        // cast as EnemyData to get enemy-specific variables
        enemyData = data as EnemyData;

        //get the areaLevel from the gameControl obj -- ADD LATER
        areaLevel = 3;

        //set the enemy's level within a range of +/- 2 of the area level -- this range can be changed later, if desired
        Level = Random.Range((areaLevel - 2), (areaLevel + 2));
        //level up until desired level is hit
        for (int i = 0; i < Level; i++)
        {
            data.LevelUp(i + 1);
        }

        // Calculate the experience and gold this enemy should award
        exp = Stars * ExpMultiplier * Level;
        gold = Stars * GoldMultiplier;

        Rename();
	}

    protected void TakeDamage(float damage, bool isMagic)
    {
        //calculate damage
        base.TakeDamage(this, damage, isMagic);

        //after loss of health, a change of healthStatus may be required
        if (Hp >= HpMax * 0.8f) { healthState = Health.high; }
        else if (Hp < HpMax * 0.8f && Hp >= HpMax * 0.5f) { healthState = Health.average; }
        else if (Hp < HpMax * 0.5f && Hp >= HpMax * 0.2f) { healthState = Health.low; }
        else { healthState = Health.dangerous; }
    }

    // The brain of the enemy
    // Every enemy will have this method, but the code for each will be tailored to it's species
    // Since every attack will be different, choosing a target should be handled in specific attack methods.
    public virtual string ChooseAttack()
    {
        return null;
    }

    public override void Attack()
    {
        attackAnimation = CurrentAttackName;
        
        base.Attack();
    }

    // This method will make it easier to distinguish the enemies
    protected void Rename()
    {
        int i = 0;

        foreach (Enemy e in battleManager.enemyList)
        {
            if (e != this && e.name.Contains(name)) { i++; }
            if (e == this) { break; }
        }
        if (i == 1) { name += " B"; }
        if (i == 2) { name += " C"; }
        if (i == 3) { name += " D"; }
        if (i == 4) { name += " E"; }
    }

    protected void ChooseSelfTarget()
    {
        targets.Add(this);
    }

    protected void ChooseRandomTarget()
    {
        int random = 0;
        do
        {
            random = Random.Range(0, battleManager.heroList.Count);
        } while (battleManager.heroList[random].IsDead);

        targets.Add(battleManager.heroList[random]);
    }

    protected void ChooseHighestHPTarget()
    {
        Hero highestHP = null;
        
        foreach (var h in battleManager.heroList)
        {
            // if null (first hero), set it
            if (highestHP == null)
                highestHP = h;

            // otherwise, if we find a hero with higher HP, set it to that new hero
            else if (highestHP.Hp > h.Hp)
                highestHP = h;
        }

        targets.Add(highestHP);
    }
}
