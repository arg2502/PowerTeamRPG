using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {

    int frostEdgeCounter = 0;
    int frostEdgeDelta = 0;
    int iceArmorCounter = 0;
    int iceBarrierCounter = 0;
    List<Denigen> iceBarrierTargets;

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

    void Frost()
    {
        StatEffect("SPD", -50f);
        StartAttack(CurrentAttackName);        
    }

    void IceArmor()
    {
        GameControl.skillTreeManager.AddTechnique(data, "Ice Armor Passive");
        //PassivesList.Add(GameControl.skillTreeManager.FindTechnique(data, "Ice Armor Passive") as Passive);
        iceArmorCounter = 2;
    }
    void ReverseIceArmor()
    {
        GameControl.skillTreeManager.RemoveTechnique(data, "Ice Armor Passive");
        //PassivesList.Remove(GameControl.skillTreeManager.FindTechnique(data, "Ice Armor Passive") as Passive);
    }

    void IceBarrier()
    {
        iceBarrierTargets = new List<Denigen>();
        for(int i = 0; i < targets.Count; i++)
        {
            iceBarrierTargets.Add(targets[i]);
            GameControl.skillTreeManager.AddTechnique(iceBarrierTargets[i].Data, "Ice Barrier Passive");
        }
        iceBarrierCounter = 2;
    }
    void ReverseIceBarrier()
    {
        for(int i = 0; i < iceBarrierTargets.Count; i++)
        {
            GameControl.skillTreeManager.RemoveTechnique(iceBarrierTargets[i].Data, "Ice Barrier Passive");
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
}
