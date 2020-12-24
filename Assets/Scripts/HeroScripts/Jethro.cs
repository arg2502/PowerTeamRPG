﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {

    int frostEdgeCounter = 0;
    int frostEdgeDelta = 0;
    int iceArmorCounter = 0;
    int iceBarrierCounter = 0;
    List<Denigen> iceBarrierTargets;
    int fogCounter = 0;
    List<Denigen> fogTargets;

    public override void CheckForResetStats()
    {
        base.CheckForResetStats();
        if (frostEdgeCounter > 0)
        {
            frostEdgeCounter--;
            if (frostEdgeCounter <= 0)
                ReverseFrostEdge();
        }
        //Debug.Log("passive list count: " + PassivesList.Count);
        if (iceArmorCounter > 0)
        {
            iceArmorCounter--;
            if (iceArmorCounter <= 0)
                ReverseIceArmor();
        }

        if(iceBarrierCounter > 0)
        {
            iceBarrierCounter--;
            if (iceBarrierCounter <= 0)
                ReverseIceBarrier();
        }

        if(fogCounter > 0)
        {
            fogCounter--;
            if (fogCounter <= 0)
                ReverseFog();
        }
    }

    public override void Attack ()
	{
		// attacks specific to the character
		switch (CurrentAttackName) {
			case "Riser":
                Riser();
                break;
            case "Frost Edge":
                FrostEdge();
                break;
            case "Rally":
                Rally();
                break;
            case "Gold Soul":
                GoldSoul();
                break;
            case "Fog":
                Fog();
                break;
            case "Frost":
                Frost();
                break;
            case "Ice Armor":
                IceArmor();
                break;
            case "Ice Barrier":
                IceBarrier();
                break;
            case "Frost Bite":
                FrostBite();
                break;
            case "Diamond Peak":
                DiamondPeak();
                break;
			case "Resilience":
				Resilience ();
				break;
            case "Helmsplitter":
            case "Trinity Slice":
            case "Arc Slash":
            case "Siege Breaker":
            case "Mordstreich":
            case "Ice Spear":
                StartAttack(CurrentAttackName);
                break;
		}

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();
    }

    void Riser()
    {
        StartAttack(CurrentAttackName);

        // 60% chance of lowering defense
        var value = Random.value;
        if (value <= 0.6f)
            StatEffect("DEF", -10f);
    }

    void FrostEdge()
    {
        var percentage = 15f;
        var originalLuckChange = LuckChange;
        StatEffect("LUCK", percentage);
        var newLuckChange = LuckChange;
        frostEdgeDelta = newLuckChange - originalLuckChange;
        frostEdgeCounter = 2;
        //print("luck change: " + LuckChange);
    }
    void ReverseFrostEdge()
    {
        RemoveStatEffectChange(this, "LUCK", frostEdgeDelta);
        frostEdgeDelta = 0;
        //print("remove change: " + LuckChange);
    }

    void Rally()
    {
        StatEffect("ATK", 20f);
    }

    void GoldSoul()
    {
        var percentage = 10f;
        StatEffect("ATK", percentage);
        StatEffect("DEF", percentage);
        StatEffect("MGKATK", percentage);
        StatEffect("MGKDEF", percentage);
        StatEffect("EVASION", percentage);
        StatEffect("LUCK", percentage);
        StatEffect("SPD", percentage);
    }

    void Fog()
    {
        StatEffect("ACC", -10f);
        fogTargets = new List<Denigen>();
        for (int i = 0; i < targets.Count; i++)
        {
            fogTargets.Add(targets[i]);
            SkillTreeManager.AddPassiveTechnique(targets[i].Data, "Fog Passive", true);
        }
        fogCounter = 5;
    }
    void ReverseFog()
    {
        for (int i = 0; i < fogTargets.Count; i++)
        {
            SkillTreeManager.RemoveTechnique(fogTargets[i].Data, "Fog Passive");
        }
        fogTargets.Clear();
    }

    void Frost()
    {
        StatEffect("SPD", -50f);
        StartAttack(CurrentAttackName);        
    }

    void IceArmor()
    {
        SkillTreeManager.AddPassiveTechnique(data, "Ice Armor Passive", true);
        iceArmorCounter = 2;
    }
    void ReverseIceArmor()
    {
        SkillTreeManager.RemoveTechnique(data, "Ice Armor Passive");
    }

    void IceBarrier()
    {
        iceBarrierTargets = new List<Denigen>();
        for(int i = 0; i < targets.Count; i++)
        {
            iceBarrierTargets.Add(targets[i]);            
            SkillTreeManager.AddPassiveTechnique(iceBarrierTargets[i].Data, "Ice Barrier Passive", true);
        }
        iceBarrierCounter = 2;
    }
    void ReverseIceBarrier()
    {
        for(int i = 0; i < iceBarrierTargets.Count; i++)
        {
            SkillTreeManager.RemoveTechnique(iceBarrierTargets[i].Data, "Ice Barrier Passive");
        }
        iceBarrierTargets.Clear();
    }

    void FrostBite()
    {
        // 60% chance of petrifying target
        var value = Random.value;
        if (value < 0.6f)
            SingleStatusAttack(DenigenData.Status.petrified);

        StartAttack(CurrentAttackName);
    }

    void DiamondPeak()
    {
        TeamStatusAttack(DenigenData.Status.bleeding);
        StartAttack(CurrentAttackName);
    }

	void Resilience()
	{
		bleedTurn += 2;
		petrifiedTurn += 2;
	}
}
