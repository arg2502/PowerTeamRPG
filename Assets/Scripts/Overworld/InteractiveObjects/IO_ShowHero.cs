using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_ShowHero : InteractiveObject {

    private void Start()
    {
        base.Start();
        notificationMessage = "Show Cole";
    }

    public override void PerformAction()
    {
        GameControl.control.CreateNPCCole(NPCHero.DisperseDirection.LEFT, StationaryNPCControl.Direction.up);
    }
}
