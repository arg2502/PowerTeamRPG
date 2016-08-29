using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Goikko : Enemy {

	// Use this for initialization -- goikko is basically mudpuppy, other than stats
	void Start () {
        stars = 1;
        expMultiplier = 1;
        goldMultiplier = 1;

        //Skills and spells will probably be static for enemies
        skillsList = new List<string>() { "Tackle" };
        skillsDescription = new List<string>() { "The attacker rushes headlong into their target. \n Str 50, Crit 25, Acc 90" };
        spellsList = new List<string>() { "Frenzy" };
        spellsDescription = new List<string>() { "The attacker gets riled up, boosting ATK.\n +10% ATK" };

        // stats - should total to 1.00f
        hpPer = 0.09f;
        pmPer = 0.09f;
        atkPer = 0.11f;
        defPer = 0.09f;
        mgkAtkPer = 0.10f;
        mgkDefPer = 0.09f;
        luckPer = 0.11f;
        evasionPer = 0.14f;
        spdPer = 0.18f;

        name = "Goikko";

        base.Start();	
	}

    //Attack method for bite -- straight forward physical attack
    void Tackle()
    {
        //pass tackle's values into the calc damage method, then pass them to the target's TakeDamage
        float damage = CalcDamage("Tackle", 0.5f, 0.25f, 0.9f, false);
        // code for choosing the target of this attack
        // because goikko is an early enemy, let's have it randomly select a target
        targets.Clear();
        int random = Random.Range(0, battleMenu.heroList.Count);
        targets.Add(battleMenu.heroList[random]);

        //Using index 0 because there is only one target for this attack
        targets[0].TakeDamage(damage, false);
    }

    public override string ChooseAttack()
    {
        // Use rng to provide variety to decision making
        float rng = Random.value; //returns a random number between 0 and 1, apparently RandomRange is depricated

        //Use health states to change goikko's behavior throughout the battle
        if (healthState == Health.high)
        {
            if (rng < 0.5f) { return "Frenzy"; } //boost atk
            else { return "Tackle"; } //attack
        }
        else if (healthState == Health.average)
        {
            if (rng < 0.25f) { return "Frenzy"; } //boost atk
            else { return "Tackle"; } //attack
        }
        else
        {
            return "Tackle";
        }
    }

    public override void Attack(string atkChoice)
    {
        switch (atkChoice)
        {
            case "Tackle":
                Tackle();
                break;
            case "Frenzy":
                calcDamageText.Add(name + " used frenzy");
                break;
            default:
                Tackle();
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
