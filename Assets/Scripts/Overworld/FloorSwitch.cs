using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : OverworldObject {

    public List<Lock> listOfLocks;

    public void PressSwitch()
    {
        ToggleAllLocks();
    }

    public void UndoSwitch()
    {
        ToggleAllLocks();
    }

    void ToggleAllLocks()
    {
        foreach (var llock in listOfLocks)
        {
            llock.ToggleLock();
        }
    }
    	
}
