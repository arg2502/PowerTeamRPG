using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : OverworldObject {

    public List<Lock> listOfLocks;

    public float customWeightNeeded = 0f;
    private float objectWeightNeeded;
    private float totalWeight = 0f;
    private bool filled = false;

    private void Start()
    {
        base.Start();

        // assigning how much weight the switch needs in order to activate
        if (weightClass == Weight.NORMAL)
            objectWeightNeeded = NormalWeight;
        else if (weightClass == Weight.LIGHT)
            objectWeightNeeded = LightWeight;
        else if (weightClass == Weight.HEAVY)
            objectWeightNeeded = HeavyWeight;
        else
            objectWeightNeeded = customWeightNeeded;
    }

    public void PressSwitch(MovableOverworldObject placedObj)
    {        
        // add weight
        // if we have the proper weight and weren't already filled, toggle all locks
        totalWeight += placedObj.ObjectWeight;

        if(totalWeight >= objectWeightNeeded && !filled)
        {
            filled = true;
            ToggleAllLocks();
        }
    }

    public void UndoSwitch(MovableOverworldObject liftedObj)
    {
        // remove weight
        // if we had the needed weight but no longer do, toggle all locks
        totalWeight -= liftedObj.ObjectWeight;

        if (totalWeight < objectWeightNeeded && filled)
        {
            filled = false;
            ToggleAllLocks();
        }
    }

    void ToggleAllLocks()
    {
        foreach (var llock in listOfLocks)
        {
            llock.ToggleLock();
        }
    }
    	
}
