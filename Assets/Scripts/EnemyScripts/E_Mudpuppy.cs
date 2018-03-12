using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Mudpuppy : Enemy {

	// Use this for initialization
	void Start () {
        //stars = 1; // SET ELSEWHERE
        //ExpMultiplier = 2;
        //GoldMultiplier = 2;
        
        // TEMPORARY
        Skill bite = new Skill();
        bite.Name = "Bite";
        bite.Pm = 0;
        bite.Description = "The attacker uses their powerful jaws to deal physical damage. \n Str 80, Crit 15, Acc 95";
        SkillsList.Add(bite);
        Skill frenzy = new Skill();
        frenzy.Name = "Frenzy";
        frenzy.Pm = 0;
        frenzy.Description = "The attacker gets riled up, boosting ATK.\n +10% ATK";
        SkillsList.Add(frenzy);

        // SET ELSEWHERE
        // stats - should total to 1.00f
        //hpPer = 0.18f;
        //pmPer = 0.06f;
        //atkPer = 0.30f;
        //defPer = 0.11f;
        //mgkAtkPer = 0.03f;
        //mgkDefPer = 0.08f;
        //luckPer = 0.15f;
        //evasionPer = 0.12f;
        //spdPer = 0.06f;

        //name = "Mudpuppy";

        //base.Start();	
	}

    //Attack method for bite -- straight forward physical attack
    //IEnumerator Bite()
    void Bite()
    {
        // code for choosing the target of this attack
        // because mudpuppy is an early enemy, let's have it attack the hero with the most remaining hp

        // COMMENT FOR NOW
        // -AG

        //Denigen tempTarget = battleMenu.heroList[0];
        //for (int i = 0; i < battleMenu.heroList.Count; i++)
        //{
        //    if (battleMenu.heroList[i].hpChange > tempTarget.hpChange)
        //    {
        //        tempTarget = battleMenu.heroList[i];
        //    }
        //}
        //targets.Add(tempTarget);

        //pass bite's values into the calc damage method, then pass them to the target's TakeDamage
        float damage = CalcDamage(0.8f, 0.15f, 0.95f, false);

        // play bite animation
        //yield return StartCoroutine(PlayAnimation("Bite"));

        //Using index 0 because there is only one target for this attack
        targets[0].TakeDamage(this, damage, false);
    }

    //IEnumerator Frenzy()
    void Frenzy()
    {
        AtkChange += (int)(Atk * 0.1f);
        calcDamageText.Add(name + " used frenzy!");

        // play frenzy animation
        //yield return StartCoroutine(PlayAnimation("Frenzy"));

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
                //StartCoroutine(Bite());
                Bite();
                break;
            case "Frenzy":
                //StartCoroutine(Frenzy());
                Frenzy();
                break;
            default:
                //StartCoroutine(Bite());
                Bite();
                break;
        }
    }    
}
