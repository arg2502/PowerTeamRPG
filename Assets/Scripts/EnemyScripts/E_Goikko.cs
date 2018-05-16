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
        ChooseRandomTarget();
        SingleAttack(65, 25, 90, false);
    }
    void Poison()
    {
        ChooseRandomTarget();
        SingleStatusAttack(DenigenData.Status.infected);
    }
    public override string ChooseAttack()
    {
        //CLEAR the targets list
        targets.Clear();

        // Use rng to provide variety to decision making
        float rng = Random.value; //returns a random number between 0 and 1, apparently RandomRange is depricated

        //Use health states to change goikko's behavior throughout the battle
        return "Poison";
        //if (healthState == Health.high)
        //{
        //    if (rng < 0.5f) { return "Poison"; } //boost atk
        //    else { return "Tackle"; } //attack
        //}
        //else if (healthState == Health.average)
        //{
        //    if (rng < 0.25f) { return "Poison"; } //boost atk
        //    else { return "Tackle"; } //attack
        //}
        //else
        //{
        //    return "Tackle";
        //}
    }

    public override void Attack()
    {
        switch (CurrentAttackName)
        {
            case "Tackle":
                Tackle();
                break;
            case "Poison":
                Poison();
                break;
        }

        base.Attack();
    }    
}
