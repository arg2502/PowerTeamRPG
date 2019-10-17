﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// This script will contain all of the base classes of passives
[Serializable]
public abstract class Passive : Technique {
    public Passive() { }
    public Passive(string[] list)
        :base(list)
    {
        for (int i = 1; i < list.Length; i++)
        {
            switch (i)
            {
                case 1:
                    name = list[i];
                    break;

                case 2:
                    description = list[i];
                    break;

                case 3:
                    int.TryParse(list[i], out cost);
                    break;

                case 4:
                    int.TryParse(list[i], out pm);
                    break;

                case 5:
                    int.TryParse(list[i], out damage);
                    break;

                case 6:
                    int.TryParse(list[i], out critical);
                    break;

                case 7:
                    int.TryParse(list[i], out accuracy);
                    break;

                case 8:
                    int.TryParse(list[i], out colPos);
                    break;

                case 9:
                    int.TryParse(list[i], out rowPos);
                    break;

                case 10:
                    int.TryParse(list[i], out level);
                    break;

            }
        }
    }
    
    public abstract void Start();

    //every passive should override this
    //public abstract int Use() { return 0; }
    public abstract void Use(Denigen attackingDen, Denigen other);
}

// Call this during the CalcDamage method
// attack based passives
// ex: 10% chance of inflicting poison
[Serializable]
public abstract class CalcDamagePassive : Passive {

    public CalcDamagePassive(string[] list)
        : base(list) { }

    public abstract float CalcDamage(Denigen attackingDen, float damage);
}

// Call this during the TakeDamage method
// defensive passives (increase defense, evasion)
// ex: item gives 50% resistance to fire attacks
[Serializable]
public abstract class TakeDamagePassive : Passive
{
    public TakeDamagePassive() { }
    public TakeDamagePassive(string[] list)
        :base(list) { }

    public abstract float TakeDamage(Denigen attackingDen, Denigen other, float damage);
}

// call this for every denigen at the end of each turn
// 
[Serializable]
public abstract class PerTurnPassive : Passive
{
    public PerTurnPassive(string[] list)
        :base(list) { }
}

// this passive just restores 1hp per turn
[Serializable]
public class LightRegeneration : PerTurnPassive {

    public LightRegeneration(string[] list)
        :base(list) { }

    public override void Start()
    {
        name = "Light Regeneration";
        description = "1hp is restored at the end of every turn.";
    }

    public override void Use(Denigen attackingDen, Denigen other)
    {
        if (attackingDen.Hp < attackingDen.HpMax)
        {
            attackingDen.Hp += 1;
            GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Effects/HealEffect"), attackingDen.transform.position, Quaternion.identity);
            be.name = "HealEffect";
            be.GetComponent<Effect>().damage = 1 + "hp";
        }
        
    }
}
[Serializable]
public class SiegeBreaker : CalcDamagePassive
{
    public SiegeBreaker(string[] list)
        :base(list) { }

    public override void Start()
    {
                
    }
    public override void Use(Denigen attackingDen, Denigen other)
    {
        
    }
    
    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        float additionalDamage = 0f;

        if (attackingDen.Hp < attackingDen.HpMax * 0.125f) // increase by 15% if less than 12.5% HP
            additionalDamage = damage * 0.15f;
        else if (attackingDen.Hp < attackingDen.HpMax * 0.25f) // increase by 10% if less than 25% HP
            additionalDamage = damage * 0.1f;
        else if (attackingDen.Hp < attackingDen.HpMax * 0.5f) // increase by 5% if less than 50% HP
            additionalDamage = damage * 0.05f;

        // make sure there's at least 1 additional damage
        if (additionalDamage > 0 && additionalDamage < 1)
            additionalDamage = 1;

        return additionalDamage;
    }
}

[Serializable]
public class Duelist : CalcDamagePassive
{
    int lvl;
    public Duelist(string[] list, int _lvl)
        :base(list)
    {
        // the level will be passed in and all calculations can be based on that
        // Duelist I : level = 1
        // Duelist II : level = 2
        // Duelist III : level = 3
        lvl = _lvl;
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        var totalDuelists = attackingDen.PassivesList.FindAll((p) => p.Name.Contains("Duelist"));
        if (totalDuelists.Count > lvl) { return 0f; }

        float additionalDamage = 0f;
        float additionalPercentage = 0.05f * lvl;

        if (attackingDen.CurrentAttack is Skill)
            additionalDamage = damage * additionalPercentage;
        return additionalDamage;
    }
}
[Serializable]
public class Resilience : PerTurnPassive
{
    public Resilience(string[] list)
        :base(list)
    {

    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}
[Serializable]
public class Unbreakable: TakeDamagePassive
{
    public Unbreakable(string[] list)
        :base(list)
    {

    }

    public override float TakeDamage(Denigen attackingDen, Denigen thisDen, float damage)
    {
        float additionDamage = 0;

        // if it's a crit and it's going to kill Jethro, make sure it leaves him with 1hp
        if(attackingDen.attackType == Denigen.AttackType.CRIT
            && damage >= thisDen.Hp)
        {
            additionDamage = -(thisDen.Hp - 1);
        }

        return additionDamage;
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}
[Serializable]
public class Magician : CalcDamagePassive
{
    int lvl;
    public Magician(string[] list, int _lvl)
        : base(list)
    {
        // the level will be passed in and all calculations can be based on that
        // Magician I : level = 1
        // Magician II : level = 2
        // Magician III : level = 3
        lvl = _lvl;
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }

    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        var totalDuelists = attackingDen.PassivesList.FindAll((p) => p.Name.Contains("Magician"));
        if (totalDuelists.Count > lvl) { Debug.Log("Magician exists: " + lvl); return 0f; }

        float additionalDamage = 0f;
        float additionalPercentage = 0.05f * lvl;
        Debug.Log("Magician original dmg: " + damage);
        if (attackingDen.CurrentAttack is Spell)
            additionalDamage = damage * additionalPercentage;
        Debug.Log("Magician new dmg: " + additionalDamage);
        return additionalDamage;
    }

}
[Serializable]
public class Caster : PerTurnPassive
{
    public Caster(string[] list)
        :base(list)
    { }
    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}
[Serializable]
public class Karmaic : CalcDamagePassive
{
    public Karmaic(string[] list)
        : base(list)
    { }
    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }

    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        return 0;
    }
}
[Serializable]
public class Disciple : CalcDamagePassive
{
    public Disciple(string[] list)
        : base(list)
    { }
    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        return 0;
    }
}
[Serializable]
public class Conductor : CalcDamagePassive
{
    public Conductor(string[] list)
        : base(list)
    { }
    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        return 0;
    }
}
[Serializable]
public class Rushdown : CalcDamagePassive
{
    public Rushdown(string[] list)
        : base(list)
    { }
    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
    public override float CalcDamage(Denigen attackingDen, float damage)
    {
        return 0;
    }
}
[Serializable]
public class Untouchable : TakeDamagePassive
{
    public Untouchable(string[] list)
        : base(list)
    { }

    public override float TakeDamage(Denigen attackingDen, Denigen other, float damage)
    {
        throw new NotImplementedException();
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}
[Serializable]
public class IceArmorPassive : TakeDamagePassive
{
    public IceArmorPassive()        
    {
        name = "Ice Armor Passive";
    }

    public override float TakeDamage(Denigen attackingDen, Denigen other, float damage)
    {
        // decrease attack by 3/4
        var additional = damage * 3f / 4f;
        return -additional;
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}

[Serializable]
public class IceBarrierPassive : TakeDamagePassive
{
    public IceBarrierPassive()
    {
        name = "Ice Barrier Passive";
    }

    public override float TakeDamage(Denigen attackingDen, Denigen other, float damage)
    {
        // decrease attack by half
        var additional = damage / 2f;
        return -additional;
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}