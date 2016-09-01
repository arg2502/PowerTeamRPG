using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Denigen : MonoBehaviour {

    // attributes
    // stats
    protected int atk, def, mgkAtk, mgkDef, luck, evasion, spd;

    // stat percentages
    protected float hpPer, pmPer, atkPer, defPer, mgkAtkPer, mgkDefPer, luckPer, evasionPer, spdPer;

    // in-battle/temporary stats -- made public so combatants can view and alter in battle stats
    public int hp, pm, hpMax, pmMax, atkBat, defBat, mgkAtkBat, mgkDefBat, luckBat, evasionBat, spdBat;

    //Boolean to block attacks
    protected bool isBlocking = false;
    public bool IsBlocking { get { return isBlocking; } set { isBlocking = value; } }

    //List of passives, useful for enemies and heroes -- ADD LATER

    //List for storing targets of Denigen's attacks and spells
    protected List<Denigen> targets = new List<Denigen>() { };
    public List<Denigen> Targets { get { return targets; } }

    // ratings/leveling up
    protected int stars;
    protected int level;
   // public int Level { get { return level; } }
    protected int baseTotal;
    protected float multiplier;
    protected float boostTotal;

    //Battle menu object
    protected BattleMenu battleMenu;
    protected List<string> takeDamageText, calcDamageText;

    public List<string> TakeDamageText { get { return takeDamageText; } set { takeDamageText = value; } }
    public List<string> CalcDamageText { get { return calcDamageText; } set { calcDamageText = value; } }

    // arrays of techniques
    protected List<string> skillsList, skillsDescription, spellsList, spellsDescription; 
	
    // properties
    public int Level { get { return level; } set { level = value; } }
    public int Atk { get { return atk; } set { atk = value; } }
    public int Def { get { return def; } set { def = value; } }
    public int MgkAtk { get { return mgkAtk; } set { mgkAtk = value; } }
    public int MgkDef { get { return mgkDef; } set { mgkDef = value; } }
    public int Luck { get {return luck; } set { luck = value; } }
    public int Evasion { get { return evasion; } set { evasion = value; } }
    public int Spd { get { return spd; } set { spd = value; } }
    //property for passives -- ADD LATER

    public List<string> SkillsList { get { return skillsList; } }
    public List<string> SkillsDescription { get { return skillsDescription; } }
    public List<string> SpellsList { get { return spellsList; } }
    public List<string> SpellsDescription { get { return spellsDescription; } }

    // status effect
    public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead };
    public Status statusState;// = Status.normal;

    public Status StatusState { get { return statusState; } set { statusState = value; } }

    protected GameObject card;
    public GameObject Card { get { return card; } set { card = value; } }


    // Use this for initialization
	protected void Start () {
        takeDamageText = new List<string>();
        calcDamageText = new List<string>();

        baseTotal = 24 + (12 * stars);

        // setting up stats
        if (hpMax == 0)// checking if they already exist, so they're not overwritten
        {
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
        }
        

        //get a reference to the battleMenu object in the scene
        if (GameObject.FindObjectOfType<BattleMenu>().GetComponent<BattleMenu>())
        {
            battleMenu = GameObject.FindObjectOfType<BattleMenu>().GetComponent<BattleMenu>();
        }

        //statusState = Status.normal;
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

    public virtual void Attack(string atkChoice)
    {
        // specific denigens will pick attack methods based off of user choice
        print(name + " Attacks");
    }

    protected void Block()
    {
        calcDamageText.Add(name + " is blocking!");
    }

    // NEEDED for crits
    // CalcDamage
    // TakeDamage

    protected float CalcDamage(string atkChoice, float power, float crit, float accuracy, bool isMagic) // all floats are percentages
    {
        calcDamageText.Add(name + " uses " + atkChoice + "!");
        // if attack misses, exit early
        float num = Random.Range(0.0f, 1.0f);
        if (num > accuracy) { calcDamageText.Add("The attack misses..."); return 0.0f; }
        else
        {
            int atkStat;
            // if its a magic attack, use magic variables
            if (isMagic)
            {
                atkStat = mgkAtkBat;
            }
            // if not magic, use physical variables
            else
            {
                atkStat = atkBat;
            }

            // calculate damage
            float damage = power * atkStat;

            // check for crit
            num = Random.Range(0.0f, 1.0f);

            // use luck to increase crit chance
            float chance = Mathf.Pow((float)(luckBat), 2.0f / 3.0f); // luck ^ 2/3
            chance /= 100; // make percentage

            // add chance to crit to increase the probability of num being the smaller one
            if (num <= (crit + chance)) { damage *= 1.5f; calcDamageText.Add( name + " strikes a weak spot!"); }

            // check for attack based passives - LATER - GO THROUGH DENIGEN'S LIST OF PASSIVES

            //Clear the target's previous text, to avoid a build up 
            print("Num of targets: " + targets.Count);
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].TakeDamageText.Clear();
                print("Clear target's damage text");
            }
               
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
            defStat = mgkDefBat;
        }
        else
        {
            defStat = defBat;
        }

        // divide damage by the defensive stat
        //damage /= defStat;

        // reduce damage by half the defensive stat
        damage -= (defStat/2);

        // if negative damage, set it to zero -- just in case
        if (damage < 0) { damage = 0; }
        
        // check if this denigen is blocking -- if so, halve the damage received
        if (isBlocking) { damage = damage / 2.0f; takeDamageText.Add(name + " blocks the attack!"); }

        // check for passives - LATER

        // decrease hp based off of damage
        hp -= (int)damage;

        //Now record appropriate text
        takeDamageText.Add(name + " takes " + (int)damage + " damage!");

        // check for dead
        if (hp <= 0) { hp = 0; takeDamageText.Add( name + " falls!"); statusState = Status.dead; }
    }

	// Update is called once per frame
	protected void Update () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
	}
}