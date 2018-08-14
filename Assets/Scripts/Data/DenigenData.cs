using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//this class should hold all of the stuff necessary for a hero object
//[Serializable]

public class DenigenData : ScriptableObject
{
    public int identity;
    public string denigenName;

    // stat percentages
    public float hpPer, pmPer, atkPer, defPer, mgkAtkPer, mgkDefPer, luckPer, evasionPer, spdPer;

    public Sprite portrait;

    // Need a creative way to store which items are equipped since items are non-serializable
    public GameObject weapon;
    public List<GameObject> equipment;

    internal bool statBoost = false;
    internal bool skillTree = false;
    internal int level, exp, expToLvlUp, levelUpPts, techPts;
    internal int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
    

    internal List<Skill> skillsList;
    internal List<Spell> spellsList;
    internal List<Passive> passiveList;
    // status effect
    public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead, overkill };
    public Status statusState;
    


    // ratings/leveling up
    public int stars;
    public int startingLevel;
    public float growthSpeed;
    internal float multiplier;
    protected int baseTotal;
    protected float boostTotal;

    //public DenigenData()
    void OnEnable()
    {
        // set base stats
        baseTotal = 24 + (12 * stars);
        level = 1;
        hp = (int)(baseTotal * hpPer);
        pm = (int)(baseTotal * pmPer);
        atk = atk = (int)(baseTotal * atkPer);
        def = def = (int)(baseTotal * defPer);
        mgkAtk = mgkAtk = (int)(baseTotal * mgkAtkPer);
        mgkDef = mgkDef = (int)(baseTotal * mgkDefPer);
        luck = luck = (int)(baseTotal * luckPer);
        evasion = evasion = (int)(baseTotal * evasionPer);
        spd = spd = (int)(baseTotal * spdPer);
        hpMax = hp;
        pmMax = pm;

        // create lists
        skillsList = new List<Skill>();
        spellsList = new List<Spell>();
        passiveList = new List<Passive>();

        // there may be some equipment passed in through inspector -- if not, then create a new list
        if (equipment == null || (equipment != null && equipment.Count > 0))
            equipment = new List<GameObject>();


        LevelUpOnAwake(startingLevel);

        //Debug.Log("Inside Awake() -- my hp is " + hp);
    }


    public void LevelUp(int rollover = 0)
    {
        level++;
        multiplier = (level / 10.0f) + 1.0f;
        boostTotal = stars * 9 * multiplier; // 9 = number of stats

        // increase stats
        hpMax += (int)(boostTotal * hpPer);        
        pmMax += (int)(boostTotal * pmPer);
        hp = hpMax; // refill hp
        pm = pmMax; // refill pm
        atk += (int)(boostTotal * atkPer);
        def += (int)(boostTotal * defPer);
        mgkAtk += (int)(boostTotal * mgkAtkPer);
        mgkDef += (int)(boostTotal * mgkDefPer);
        luck += (int)(boostTotal * luckPer);
        evasion += (int)(boostTotal * evasionPer);
        spd += (int)(boostTotal * spdPer);
        
        // allocate stats
        //levelUpPts += (int)(stars * multiplier); // 3 * (level/10 + 1)

        // actually allocating the points will be done through a levelup menu

        // increase technique points each level up
        techPts++;

        //calc new required points to level up
        expToLvlUp = CalcExpToLvlUp(rollover);
    }

    
    protected void LevelUpOnAwake(int startingLevel, int rollover = 0)
    {
        expToLvlUp = CalcExpToLvlUp(); // at least once if level one -- kinda hacky

        // minus 1 because it will level up "startingLevel" number of times
        // we already start at 1, so we wanna start at (startingLevel-1)
        for (int i = 0; i < startingLevel - 1; i++)
        {
            LevelUp(rollover);
            
        }
    }

    protected int CalcExpToLvlUp(int rollover = 0)
    {
        float expToGo = 0;
        expToGo = level * growthSpeed * 10;
        return ((int)expToGo - rollover);
    }

    public int MaxExpOfLevel(int thisLevel)
    {
        return (int)(thisLevel * growthSpeed * 10);
    }

    /// <summary>
    /// Returns true if the denigen's status is either "dead" or "overkill"
    /// A simpler solution to writing if(dead || overkill) all the time
    /// </summary>
    /// <returns></returns>
    public bool IsDead
    {
        get
        {
            if (statusState == Status.dead)
                return true;
            else if (statusState == Status.overkill)
                return true;
            else
                return false;
        }
    }
    /// <summary>
    /// only dead -- not overkill
    /// </summary>
    public bool IsJustDead
    {
        get
        {
            if (statusState == Status.dead)
                return true;
            else
                return false;
        }
    }
}