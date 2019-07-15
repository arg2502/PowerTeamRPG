using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : Denigen {
    
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
    public int MainTargetIndex { get { return targetIndex; } set { targetIndex = value; } }
    public int LevelUpPts { get { return levelUpPts; } set { levelUpPts = value; } }
    public int TechPts { get { return techPts; } set { techPts = value; } }

    public TargetType currentTargetType;
    
    public bool EnoughPm(Technique tech) { return Pm >= tech.Pm; } 

    // the method for handling item use
    public void ItemUse()
    {
		GameControl.itemManager.BattleItemUse (this, targets);
    }

    public virtual void DecideTypeOfTarget()
    {
        currentTargetType = TargetType.NULL;

        // Determine if it's a hero's specific technique first
        var tech = GameControl.skillTreeManager.FindTechnique(data, CurrentAttackName);
        if (tech != null)
        {
            currentTargetType = tech.targetType;
            return;
        }

        // if we reached this point, then we haven't found the right target type yet
        switch (CurrentAttackName)
        {
            case "Strike":
                currentTargetType = TargetType.ENEMY_SINGLE;
                break;
            case "Block":
                // special case -- do all blocking code here
                currentTargetType = TargetType.HERO_SELF;
                isBlocking = true;
                var targets = new List<Denigen>() { this };
                battleManager.TargetDenigen(targets);
                break;
            default:
                currentTargetType = DecideItemTarget(CurrentAttackName);
                break;
        }
    }

    TargetType DecideItemTarget(string itemName)
    {
		ScriptableConsumable _item = ItemDatabase.GetItem ("consumable", itemName) as ScriptableConsumable;
		return (TargetType)_item.targetType;
    }

    //select the target for your attack
    public virtual void SelectTarget(List<Denigen> targetsFromCursors)
    {
        //clear any previously selected targets from other turns
        if (targets != null)
        {
            targets.Clear();
        }

        foreach (var d in targetsFromCursors)
            targets.Add(d);

    }
        
    public override void Attack()
    {
        // check if the target is still alive, if not -- find a new one
        // if we're not using items
        if (!usingItem)
        {
            IsTargetDead();

            // specific denigens will pick attack methods based off of user choice
            switch (CurrentAttackName)
            {
                case "Strike":
                    Strike();
                    break;
                case "Block":
                    Block();
                    break;
            }
        }
        else
        {
            ItemUse();
        }
        // just set all attacks with this attack animation for now
        attackAnimation = "Attack";
        
        // Denigen attack -- tells BattleManager that this denigen's attack phase is over
        base.Attack();
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
        damage = CalcDamage(0.3f, 0.1f, 0.95f, isMagic);
        targets[0].TakeDamage(this, damage, isMagic);
    }

    // if the current target is dead, clear target list and find new single target
    void IsTargetDead()
    {
        // if they're not dead, stop right here
        // or if the attack is a team attack, then the first target doesn't matter, return
        if (!targets[0].IsDead || (currentTargetType == TargetType.ENEMY_TEAM || currentTargetType == TargetType.HERO_TEAM))
            return;
        
        print("Target is dead -- find new target");

        targets.Clear();
        print(DenigenName + "'s TargetIndex: " + targetIndex);

        List<Denigen> list = new List<Denigen>();
        if (currentTargetType == TargetType.ENEMY_SINGLE || currentTargetType == TargetType.ENEMY_SPLASH || currentTargetType == TargetType.ENEMY_TEAM)
        {
            foreach (var e in battleManager.enemyList)
                list.Add(e);
        }
        else
        {
            foreach (var h in battleManager.heroList)
                list.Add(h);
        }
            
        // the target index is in reverse order from the enemyList (for menu layout purposes)
        // find the opposite value
        var newTargetIndex = (list.Count - 1) - targetIndex;

        for(int i = 1; i < list.Count; i++)
        {
            // check left
            if ((newTargetIndex - i) >= 0 && !list[newTargetIndex - i].IsDead)
            {
                targetIndex = newTargetIndex - i;
                targets.Add(list[targetIndex]);
                break;
            }

            // check right
            if ((newTargetIndex + i) < list.Count && !list[newTargetIndex + i].IsDead)
            {
                targetIndex = newTargetIndex + i;
                targets.Add(list[targetIndex]);
                break;
            }
         
        }
        
    }

    public void AddExp(int expToAdd)
    {
        var tempData = data as HeroData;
        tempData.AddExp(expToAdd);
    }
}