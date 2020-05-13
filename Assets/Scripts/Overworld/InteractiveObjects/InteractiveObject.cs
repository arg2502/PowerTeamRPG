using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : OverworldObject {

    protected string notificationMessage = "DEFAULT";

    /// <summary>
    /// Child classes should override this function with their own individual code
    /// that will perform once the player interacts with the object (presses Space/Select)
    /// </summary>
	public virtual void PerformAction()
    {
        print("Hello from PerformAction()!");
    }

    public override void ShowInteractionNotification(string message = "")
    {
        base.ShowInteractionNotification(notificationMessage);
    }
}
