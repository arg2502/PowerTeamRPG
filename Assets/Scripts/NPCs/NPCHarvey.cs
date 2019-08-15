using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHarvey : StationaryNPCControl {

	public void StartTest()
    {
        GameControl.questTracker.AddNewQuest("test");
    }
}
