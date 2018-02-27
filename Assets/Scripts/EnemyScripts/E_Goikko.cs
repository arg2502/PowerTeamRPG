using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Goikko : Enemy {

	// Use this for initialization -- goikko is basically mudpuppy, other than stats
	void Start () {
        //stars = 1; // SET ELSEWHERE
        //ExpMultiplier = 1;
        //GoldMultiplier = 1;

        //Skills and spells will probably be static for enemies
        //skillsList = new List<string>() { "Tackle" };
        //skillsDescription = new List<string>() { "The attacker rushes headlong into their target. \n Str 50, Crit 25, Acc 90" };
        //spellsList = new List<string>() { "Frenzy" };
        //spellsDescription = new List<string>() { "The attacker gets riled up, boosting ATK.\n +10% ATK" };
        //skillsList = new List<Skill>() { };
        //spellsList = new List<Spell>() { };

        // TEMPORARY
        Skill tackle = new Skill();
        tackle.Name = "Tackle";
        tackle.Pm = 0;
        tackle.Description = "The attacker rushes headlong into their target. \n Str 50, Crit 25, Acc 90";
        SkillsList.Add(tackle);
        Skill frenzy = new Skill();
        frenzy.Name = "Frenzy";
        frenzy.Pm = 0;
        frenzy.Description = "The attacker gets riled up, boosting ATK.\n +10% ATK";
        SkillsList.Add(frenzy);

        // stats - should total to 1.00f
        //hpPer = 0.19f;
        //pmPer = 0.09f;
        //atkPer = 0.13f;
        //defPer = 0.08f;
        //mgkAtkPer = 0.10f;
        //mgkDefPer = 0.08f;
        //luckPer = 0.11f;
        //evasionPer = 0.14f;
        //spdPer = 0.18f;

        //name = "Goikko";

        //base.Start();	
	}

    //Attack method for bite -- straight forward physical attack
    //IEnumerator Tackle()
    void Tackle()
    {
        // code for choosing the target of this attack
        // because goikko is an early enemy, let's have it randomly select a target
        int random = 0;
        do
        {
            random = Random.Range(0, battleManager.heroList.Count);
        } while (battleManager.heroList[random].StatusState == Status.dead);

        targets.Add(battleManager.heroList[random]);


        //pass tackle's values into the calc damage method, then pass them to the target's TakeDamage
        float damage = CalcDamage("Tackle", 0.65f, 0.25f, 0.9f, false);

        //yield return StartCoroutine(PlayAnimation("Attack"));

        //Using index 0 because there is only one target for this attack
        targets[0].TakeDamage(this, damage, false);
    }
    void Frenzy()
    {
        calcDamageText.Add(name + " used frenzy");
        print(name + " used frenzy");
        //yield return StartCoroutine(PlayAnimation("Poison"));
    }
    public override string ChooseAttack()
    {
        //CLEAR the targets list
        targets.Clear();

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
                //StartCoroutine(Tackle());
                Tackle();
                break;
            case "Frenzy":
                //StartCoroutine(Frenzy());
                Frenzy();
                break;
            default:
                //StartCoroutine(Tackle());
                Tackle();
                break;
        }
    }    
}
