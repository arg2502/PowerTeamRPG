using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

    public override float GetPmMult(Technique t = null)
    {
        if (t != null & t is Spell)
        {
            var percentage = 0.1f;
            var totalCasters = PassivesList.FindAll((p) => p.Name.Contains("Caster"));
            return 1f - (percentage * totalCasters.Count);
        }
        else return 1f;

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
            case "Candleshot":
            case "Fireball":
            case "Grand Fireball":
            case "Splash Flame":
            case "Firewall":
            case "Hellfire":
                StartAttack(CurrentAttackName);
                break;
        }

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
}
