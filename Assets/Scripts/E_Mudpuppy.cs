﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Mudpuppy : Enemy {

	// Use this for initialization
	void Start () {
        stars = 1;
        expMultiplier = 2;
        goldMultiplier = 2;

        //Skills and spells will probably be static for enemies
        //skillsList = new List<string>() { "Bite" };
        //skillsDescription = new List<string>() { "The attacker uses their powerful jaws to deal physical damage. \n Str 90, Crit 20, Acc 95" };
        //spellsList = new List<string>() { "Frenzy" };
        //spellsDescription = new List<string>() { "The attacker gets riled up, boosting ATK.\n +10% ATK" };
        skillsList = new List<Technique>() { };
        spellsList = new List<Technique>() { };
        Technique bite = new Technique();
        bite.Name = "Bite";
        bite.Pm = 0;
        bite.Description = "The attacker uses their powerful jaws to deal physical damage. \n Str 90, Crit 20, Acc 95";
        skillsList.Add(bite);
        Technique frenzy = new Technique();
        frenzy.Name = "Frenzy";
        frenzy.Pm = 0;
        frenzy.Description = "The attacker gets riled up, boosting ATK.\n +10% ATK";
        skillsList.Add(frenzy);

        // stats - should total to 1.00f
        hpPer = 0.18f;
        pmPer = 0.06f;
        atkPer = 0.30f;
        defPer = 0.12f;
        mgkAtkPer = 0.02f;
        mgkDefPer = 0.09f;
        luckPer = 0.14f;
        evasionPer = 0.12f;
        spdPer = 0.06f;

        name = "Mudpuppy";

        base.Start();	
	}

    //Attack method for bite -- straight forward physical attack
    void Bite()
    {
        // code for choosing the target of this attack
        // because mudpuppy is an early enemy, let's have it attack the hero with the most remaining hp
        Denigen tempTarget = battleMenu.heroList[0];
        for (int i = 0; i < battleMenu.heroList.Count; i++)
        {
            if (battleMenu.heroList[i].hp > tempTarget.hp)
            {
                tempTarget = battleMenu.heroList[i];
            }
        }
        targets.Add(tempTarget);

        //pass bite's values into the calc damage method, then pass them to the target's TakeDamage
        float damage = CalcDamage("Bite", 0.9f, 0.2f, 0.95f, false);
        
        //Using index 0 because there is only one target for this attack
        targets[0].TakeDamage(damage, false);
    }

    void Frenzy()
    {
        atkBat += (int)(atkBat * 0.1f);
        calcDamageText.Add(name + " used frenzy!");
        calcDamageText.Add("It's atk power has increased!");
    }

    public override string ChooseAttack()
    {
        // clear the targets list
        targets.Clear();

        // Use rng to provide variety to decision making
        float rng = Random.value; //returns a random number between 0 and 1, apparently RandomRange is depricated

        //Use health states to change mudpuppy's behavior throughout the battle
        if (healthState == Health.high)
        {
            if (rng < 0.5f) { return "Frenzy"; } //boost atk
            else { return "Bite"; } //attack
        }
        else if (healthState == Health.average)
        {
            if (rng < 0.25f) { return "Frenzy"; } //boost atk
            else { return "Bite"; } //attack
        }
        else
        {
            return "Bite";
        }
    }

    public override void Attack(string atkChoice)
    {
        switch (atkChoice)
        {
            case "Bite":
                Bite();
                break;
            case "Frenzy":
                Frenzy();
                break;
            default:
                Bite();
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
