using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {
    string prevAttackName;
    List<string> studyAttacks = new List<string>();

    Spell lastSpellUsed;
    int resivictionTimesUsed = 0;
    List<float> resivictionAcc = new List<float>() { 0.9f, 0.5f, 0.1f };

    public override float GetPmMult(Technique t = null)
    {
		if (t == null)
			return 1f;
		if (t.Name == "Resiviction") 
			return 0f;
        if (t is Spell)
        {
            var percentage = 0.1f;
            var totalCasters = PassivesList.FindAll((p) => p.Name.Contains("Caster"));
            return 1f - (percentage * totalCasters.Count);
        }
        else return 1f;

    }

    protected override void PreAttackCheck(Denigen attacker)
    {
        if(CurrentAttackName == "Resist Enchantment" && attacker.CurrentAttack is Spell)
        {
            attacker.attackType = AttackType.DODGED;
            CalculatedDamage = 0;
            print("Cole dodged the Spell attack due to Resist Enchantment");
        }
        if(studyAttacks.Contains(attacker.CurrentAttackName))
        {
            CalculatedDamage = (int)(CalculatedDamage * 0.25f);
            print("Damage reduced by 75% due to Study's ability");
        }
        prevAttackName = attacker.CurrentAttackName;
        base.PreAttackCheck(attacker);
    }

	public override void DecideTypeOfTarget ()
	{
		if (CurrentAttackName == "Resiviction") 
		{			
			var tech = GameControl.skillTreeManager.FindTechnique(data, lastSpellUsed.Name);
			currentTargetType = tech.targetType;
			return;			
		}
		base.DecideTypeOfTarget ();
	}

    public override void Attack()
    {
        // attacks specific to the character
        switch (CurrentAttackName)
        {
            case "Anathema":
                Anathema();
                break;
            case "Cauterize":
                Cauterize();
                break;
            case "Twilight Cascade":
                TwilightCascade();
                break;
            case "Eclipse":
                Eclipse();
                break;
            case "Hollow":
                Hollow();
                break;
            case "Resist Enchantment":
                ResistEnchantment();
                break;
            case "Study":
                Study();
                break;
            case "Reaper Gaze":
                ReaperGaze();
                break;
            case "Resiviction":
                Resiviction();
				return;
            case "Bonecrush":
                Bonecrush();
                break;
			case "Bucket Splash":
				BucketSplash ();
				break;
			case "Cole Fusion":
				ColeFusion ();
				break;
            case "Candleshot":
            case "Fireball":
            case "Grand Fireball":
            case "Splash Flame":
            case "Firewall":
            case "Hellfire":
                StartAttack(CurrentAttackName);
                break;
        }

		if (CurrentAttack is Spell && CurrentAttackName != "Resiviction")
            lastSpellUsed = CurrentAttack as Spell;
        
        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();

    }

    void Anathema()
    {
        SingleStatusAttack(DenigenData.Status.cursed);
    }

    void Cauterize()
    {
        if (targets[0].StatusState == DenigenData.Status.bleeding)
            SingleStatusAttack(DenigenData.Status.normal);
        StartAttack(CurrentAttackName);
    }

    void TwilightCascade() // speadsheet needs updating (dmg and target type)
    {
        var val = Random.value;
        if (val <= 0.2f)
            TeamStatusAttack(DenigenData.Status.cursed);
        StartAttack(CurrentAttackName);
    }

    void Eclipse()
    {
        SingleStatusAttack(DenigenData.Status.blinded);
    }

    void Hollow()
    {
        // swap atk/mgkatk & def/mgkdef -- by setting the Change values
        var origAtk = Atk;
        var origMgkAtk = MgkAtk;
        var atkDiff = Atk - MgkAtk;

        AtkChange -= atkDiff;
        MgkAtkChange += atkDiff;

        var origDef = Def;
        var origMgkDef = MgkDef;
        var defDiff = Def - MgkDef;

        DefChange -= defDiff;
        MgkDefChange += defDiff;
    }

    void ResistEnchantment()
    {
        print("Cole is ready to resist the next spell.");
    }

    void Study()
    {
        if (studyAttacks == null)
            studyAttacks = new List<string>();

        if (studyAttacks.Contains(prevAttackName))
            print("Technique was already recorded");
        else
            studyAttacks.Add(prevAttackName);
    }

    void ReaperGaze()
    {
        if(StatusState == DenigenData.Status.blinded)
        {
            print("Cannot be cast while blinded");
            return;
        }
        if (targets[0].StatusState == DenigenData.Status.blinded)
        {
            print("Cannot be cast on the blinded");
            return;
        }

        SingleStatusAttack(DenigenData.Status.petrified);
    }

    void Resiviction()
    {
        if(lastSpellUsed == null)
        {
            print("no last spell found");
            return;
        }
        
        var timesUsed = (resivictionTimesUsed > resivictionAcc.Count - 1) ? resivictionAcc.Count - 1 : resivictionTimesUsed;

		Accuracy = resivictionAcc [timesUsed];
        CurrentAttackName = lastSpellUsed.Name;
		AttackCost = 0;
        resivictionTimesUsed++;
        Attack();

		Accuracy = 1f;

    }

    void Bonecrush()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GameControl.skillTreeManager.AddPassiveTechnique(targets[i].Data, "Bonecrush Passive", true);
        }
    }

	void BucketSplash()
	{
		for (int i = 0; i < targets.Count; i++) 
		{
			GameControl.skillTreeManager.AddPassiveTechnique (targets [i].Data, "BucketSplash Passive", true);
		}
	}

	void FlaskSplash()
	{
		for (int i = 0; i < targets.Count; i++) 
		{
			GameControl.skillTreeManager.AddPassiveTechnique (targets [i].Data, "FlaskSplash Passive", true);
		}
	}

	void ColeFusion()
	{
		// high chance of blinding
		var rand = Random.value;
		if (rand <= 0.75f)
			SingleStatusAttack (DenigenData.Status.blinded);

		var amt = Pm;
		AttackCost = amt;
		for (int i = 0; i < targets.Count; i++) {
			targets [i].CalculatedDamage = amt;
		}
	}
}
