using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Denigen : MonoBehaviour {

    // attributes
    // stats
    protected int hpMax, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;

    // stat percentages
    protected float hpPer, pmPer, atkPer, defPer, mgkAtkPer, mgkDefPer, luckPer, evasionPer, spdPer;

    // in-battle/temporary stats -- made public so combatants can view and alter in battle stats
    public int hp, pm, atkBat, defBat, mgkAtkBat, mgkDefBat, luckBat, evasionBat, spdBat;

    //List of passives, useful for enemies and heroes -- ADD LATER

    //List for storing targets of Denigen's attacks and spells
    protected List<Denigen> targets;

    // ratings/leveling up
    protected int stars;
    protected int level;
    protected int baseTotal;
    protected float multiplier;
    protected float boostTotal;

    //Battle menu object
    protected BattleMenu battleMenu;

    // arrays of techniques
    protected List<string> skillsList, skillsDescription, spellsList, spellsDescription; 
	
    // properties
    public int Spd { get { return spd; } }

    public List<string> SkillsList { get { return skillsList; } }
    public List<string> SkillsDescription { get { return skillsDescription; } }
    public List<string> SpellsList { get { return spellsList; } }
    public List<string> SpellsDescription { get { return spellsDescription; } }

    // status effect
    enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead };
    Status statusState = Status.normal;


    // Use this for initialization
	protected void Start () {
        baseTotal = 24 + (12 * stars);

        // setting up stats
        hp = (int)(baseTotal * hpPer);        
        pm = (int)(baseTotal * pmPer);        
        atkBat = atk = (int)(baseTotal * atkPer);
        defBat = def = (int)(baseTotal * defPer);
        mgkAtkBat = mgkAtk = (int)(baseTotal * mgkAtkPer);
        mgkDefBat = mgkDef = (int)(baseTotal * mgkDefPer);
        luckBat = luck = (int)(baseTotal * luckPer);
        evasionBat = evasion = (int)(baseTotal * evasionPer);
        spdBat = spd = (int)(baseTotal * spdPer);
        hpMax = hp;
        pmMax = pm;

        //get a reference to the battleMenu object in the scene
        battleMenu = GameObject.FindObjectOfType<BattleMenu>().GetComponent<BattleMenu>();
	}
    protected void LevelUp(int lvl)
    {
        multiplier = (lvl / 10.0f) + 1.0f;
        boostTotal = stars * 9 * multiplier; // 9 = number of stats
    
        // increase stats
        hp += (int)(boostTotal * hpPer);
        pm += (int)(boostTotal * pmPer);
        hpMax += (int)(boostTotal * hpPer);
        pmMax += (int)(boostTotal * pmPer);
        atk += (int)(boostTotal * atkPer);
        def += (int)(boostTotal * defPer);
        mgkAtk += (int)(boostTotal * mgkAtkPer);
        mgkDef += (int)(boostTotal * mgkDefPer);
        luck += (int)(boostTotal * luckPer);
        evasion += (int)(boostTotal * evasionPer);
        spd += (int)(boostTotal * spdPer);

        //just in case we're in battle when we level up, let's also increase the bsttle stats
        atkBat += (int)(boostTotal * atkPer);
        defBat += (int)(boostTotal * defPer);
        mgkAtkBat += (int)(boostTotal * mgkAtkPer);
        mgkDefBat += (int)(boostTotal * mgkDefPer);
        luckBat += (int)(boostTotal * luckPer);
        evasionBat += (int)(boostTotal * evasionPer);
        spdBat += (int)(boostTotal * spdPer);
        
    }

    public void Attack(string atkChoice)
    {
        // specific denigens will pick attack methods based off of user choice
    }

    // NEEDED for crits
    // CalcDamage
    // TakeDamage

    protected float CalcDamage(float power, float crit, float accuracy, bool isMagic) // all floats are percentages
    {
        // if attack misses, exit early
        float num = Random.RandomRange(0.0f, 1.0f);
        if (num > accuracy){ return 0.0f; }
        else
        {
            int atkStat;
            // if its a magic attack, use magic variables
            if (isMagic)
            {
                atkStat = atk;
            }
            // if not magic, use physical variables
            else
            {
                atkStat = mgkAtk;
            }

            // calculate damage
            float damage = power * atkStat;

            // check for crit
            num = Random.RandomRange(0.0f, 1.0f);

            // use luck to increase crit chance
            float chance = Mathf.Pow((float)(luck), 2.0f / 3.0f); // luck ^ 2/3
            chance /= 100; // make percentage

            // add chance to crit to increase the probability of num being the smaller one
            if (num <= (crit + chance)) { damage *= 1.5f; }

            // check for attack based passives - LATER - GO THROUGH DENIGEN'S LIST OF PASSIVES

            // return final damage amount
            return damage;
        }
    }

    //Made public to allow other denigens to deal damage
    public void TakeDamage(float damage, bool isMagic)
    {
        // use stat based on if magic or physical
        int defStat;
        if (isMagic)
        {
            defStat = mgkDef;
        }
        else
        {
            defStat = def;
        }

        // divide damage by the defensive stat
        damage /= defStat;

        // if negative damage, set it to zero -- just in case
        if (damage < 0) { damage = 0; }

        // check for passives - LATER

        // decrease hp based off of damage
        hp -= (int)damage;

        // check for dead
        if (hp <= 0) { statusState = Status.dead; }
    }

	// Update is called once per frame
	void Update () {
	
	}
}