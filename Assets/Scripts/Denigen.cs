﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Denigen : MonoBehaviour {
    
    // in-battle stats -- these variables will hold any temporary changes made to the Denigen's stats during battle
    // Ex: a move temporarily increases Attack stat by 5 but only for a few turns, or for the rest of the battle
    // so atkChange would be set to +5, and set back to zero after x amount of turns and/or at the end of the battle
    private int hpMaxChange, pmMaxChange, atkChange, defChange, mgkAtkChange, mgkDefChange, luckChange, evasionChange, spdChange;

    //Boolean to block attacks
    protected bool isBlocking = false;
    public bool IsBlocking { get { return isBlocking; } set { isBlocking = value; } }

    //List for storing targets of Denigen's attacks and spells
    protected List<Denigen> targets = new List<Denigen>() { };
    public List<Denigen> Targets { get { return targets; } }
    
    //Battle menu object
    protected BattleMenu battleMenu;
    protected List<string> takeDamageText, calcDamageText;

    protected DenigenData data;

    public List<string> TakeDamageText { get { return takeDamageText; } set { takeDamageText = value; } }
    public List<string> CalcDamageText { get { return calcDamageText; } set { calcDamageText = value; } }
    
    // Changes to stats
    //public int HpChange { get { return hpChange; } set { hpChange = value; } } // HP & PM shouldn't ever temporarily change
    //public int PmChange { get { return pmChange; } set { pmChange = value; } }
    public int HpMaxChange { get { return hpMaxChange; } set { hpMaxChange = value; } }
    public int PmMaxChange { get { return pmMaxChange; } set { pmMaxChange = value; } }
    public int AtkChange { get { return atkChange; } set { atkChange = value; } }
    public int DefChange { get { return defChange; } set { defChange = value; } }
    public int MgkAtkChange { get { return mgkAtkChange; } set { mgkAtkChange = value; } }
    public int MgkDefChange { get { return mgkDefChange; } set { mgkDefChange = value; } }
    public int LuckChange { get { return luckChange; } set { luckChange = value; } }
    public int EvasionChange { get { return evasionChange; } set { evasionChange = value; } }
    public int SpdChange { get { return spdChange; } set { spdChange = value; } }

    // DenigenData linkers
    // fighting stats (with in-battle changes
    public int Hp { get { return data.hp; } set { data.hp = value; } }
    public int Pm { get { return data.pm; } set { data.pm = value; } }
    public int HpMax { get { return data.hpMax + hpMaxChange; } }
    public int PmMax { get { return data.pmMax + pmMaxChange; } }
    public int Atk { get { return data.atk + atkChange; } }
    public int Def { get { return data.def + defChange; } }
    public int MgkAtk { get { return data.mgkAtk + mgkAtkChange; } }
    public int MgkDef { get { return data.mgkDef + mgkDefChange; } }
    public int Luck { get { return data.luck + luckChange; } }
    public int Evasion { get { return data.evasion + evasionChange; } }
    public int Spd { get { return data.spd + spdChange; } }

    // leveling stats
    public int Level { get { return data.level; } set { data.level = value; } }
    public int Stars { get { return data.stars; } }
    public float Multiplier { get { return data.multiplier; } }
    public int Exp { get { return data.exp; } set { data.exp = value; } }
    public int ExpToLevelUp { get { return data.expToLvlUp; } set { data.expToLvlUp = value; } }

    // techniques
    public List<Passive> PassivesList { get { return data.passiveList; } }
    public List<Skill> SkillsList { get { return data.skillsList; } }
    public List<Spell> SpellsList { get { return data.spellsList; } }
    
    // status effect
    public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead, overkill };
    private Status statusState;// = Status.normal;

    public Status StatusState { get { return statusState; } set { statusState = value; } }

    protected GameObject card;
    public GameObject Card { get { return card; } set { card = value; } }

    //sprite renderer, for targeting and fading effects
    protected SpriteRenderer sr;
    public SpriteRenderer Sr { get { return sr; } set { sr = value; } }

    public Shader targetShader;
    public Shader normalShader;

    //colors for targeting and fading effects
    protected Color targetRed = new Color(0.7f, 0.0f, 0.0f, 1.0f);
    protected Color splashTargetRed = new Color(0.7f, 0.0f, 0.0f, 0.5f);
    protected Color targetGreen = new Color(0.0f, 0.5f, 0.1f, 1.0f);
    protected Color fade = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    protected Color invisible = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    // for animation
    protected Animator anim;

    // Use this for initialization
	protected void Awake () {

        takeDamageText = new List<string>();
        calcDamageText = new List<string>();
        
        // COMMENTED OUT ON 12/6/2017
        // BATTLE MENU WILL BE REDONE AT A LATER TIME
        // -AG

        //get a reference to the battleMenu object in the scene
        //if (GameObject.FindObjectOfType<BattleMenu>().GetComponent<BattleMenu>())
        //{
        //    battleMenu = GameObject.FindObjectOfType<BattleMenu>().GetComponent<BattleMenu>();
        //}

        //battleMenu.SortDenigens();
        ////highlight the current denigen
        //// set the current denigen
        //battleMenu.currentDenigen = battleMenu.denigenArray[0];
        //battleMenu.currentDenigen.Card.GetComponent<TextMesh>().color = Color.yellow;
        ////statusState = Status.normal;

        //sr = gameObject.GetComponent<SpriteRenderer>();
        //targetShader = Shader.Find("GUI/Text Shader");
        //normalShader = Shader.Find("Sprites/Default");

        //if (statusState == Status.dead || statusState == Status.overkill) { sr.color = new Color(0.0f, 0.0f, 0.0f, 0.0f); }

        //anim = GetComponent<Animator>();
	}
    //protected void LevelUp(int lvl)
    //{
    //    multiplier = (lvl / 10.0f) + 1.0f;
    //    boostTotal = stars * 9 * multiplier; // 9 = number of stats

    //    // increase stats
    //    hpChange += (int)(boostTotal * hpPer);
    //    pmChange += (int)(boostTotal * pmPer);
    //    hpMaxChange += (int)(boostTotal * hpPer);
    //    pmMaxChange += (int)(boostTotal * pmPer);
    //    atk += (int)(boostTotal * atkPer);
    //    def += (int)(boostTotal * defPer);
    //    mgkAtk += (int)(boostTotal * mgkAtkPer);
    //    mgkDef += (int)(boostTotal * mgkDefPer);
    //    luck += (int)(boostTotal * luckPer);
    //    evasion += (int)(boostTotal * evasionPer);
    //    spd += (int)(boostTotal * spdPer);

    //    //just in case we're in battle when we level up, let's also increase the bsttle stats
    //    atkChange += (int)(boostTotal * atkPer);
    //    defChange += (int)(boostTotal * defPer);
    //    mgkAtkChange += (int)(boostTotal * mgkAtkPer);
    //    mgkDefChange += (int)(boostTotal * mgkDefPer);
    //    luckChange += (int)(boostTotal * luckPer);
    //    evasionChange += (int)(boostTotal * evasionPer);
    //    spdChange += (int)(boostTotal * spdPer);
        
    //}

    public virtual void Attack(string atkChoice)
    {
        // specific denigens will pick attack methods based off of user choice
        
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
                atkStat = mgkAtkChange;
            }
            // if not magic, use physical variables
            else
            {
                atkStat = atkChange;
            }

            // calculate damage
            float damage = power * atkStat;

            // check for crit
            num = Random.Range(0.0f, 1.0f);

            // use luck to increase crit chance
            float chance = Mathf.Pow((float)(luckChange), 2.0f / 3.0f); // luck ^ 2/3
            chance /= 100; // make percentage

            // add chance to crit to increase the probability of num being the smaller one
            if (num <= (crit + chance)) { damage *= 1.5f; calcDamageText.Add( name + " strikes a weak spot!"); }

            // check for attack based passivesList
            foreach (Passive cdp in PassivesList)
            {
                if (cdp is CalcDamagePassive) { cdp.Use(this, null); }
            }

            //Clear the target's previous text, to avoid a build up 
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].TakeDamageText.Clear();
            }
               
            // return final damage amount
            return damage;
        }
    }

    //Made public to allow other denigens to deal damage
    public void TakeDamage(Denigen attackingDen, float damage, bool isMagic)
    {
        // use stat based on if magic or physical
        int defStat;
        if (isMagic)
        {
            defStat = mgkDefChange;
        }
        else
        {
            defStat = defChange;
        }

        // divide damage by the defensive stat
        //damage /= defStat;

        // reduce damage by half the defensive stat
        damage -= (defStat/2);

        // if negative damage, set it to zero -- just in case
        if (damage < 0) { damage = 0; }
        
        // check if this denigen is blocking -- if so, halve the damage received
        if (isBlocking) { damage = damage / 2.0f; takeDamageText.Add(name + " blocks the attack!"); }

        // check for passivesList
        foreach (Passive tdp in PassivesList)
        {
            if (tdp is TakeDamagePassive) { tdp.Use(attackingDen, this); }
        }

        // decrease hp based off of damage
        Hp -= (int)damage;

        //Now record appropriate text
        takeDamageText.Add(name + " takes " + (int)damage + " damage!");

        // create the damage effect, but onlu if the denigen is not dead
        if (statusState != Status.dead && statusState != Status.overkill)
        {
            GameObject be = (GameObject)Instantiate(Resources.Load("Prefabs/DamageEffect"), transform.position, Quaternion.identity);
            be.name = "DamageEffect";
            //be.GetComponent<Effect>().Start();
            be.GetComponent<Effect>().damage = (int)damage + "";
        }

        // check for dead
        if (Hp <= 0)
        {
            Hp = 0;
            takeDamageText.Add( name + " falls!");
            statusState = Status.dead;
        }
    }

	// Update is called once per frame
	protected void Update () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;

        //fade away if fallen
        if (statusState == Status.dead || statusState == Status.overkill)
        {
            if (sr.color.a > 0 && !GameControl.control.isDying)
                GameControl.control.isDying = true;

            sr.color -= fade * Time.deltaTime;

            if (sr.color.a <= 0 && GameControl.control.isDying)
                GameControl.control.isDying = false;
        }
	}
   
    protected IEnumerator PlayAnimation(string animation)
    {
        GameControl.control.isAnimating = true;
        anim.Play(animation); 
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        GameControl.control.isAnimating = false;
        StopCoroutine("PlayAnimation");
    }

}