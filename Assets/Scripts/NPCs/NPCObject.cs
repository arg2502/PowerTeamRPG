using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : MovableOverworldObject
{
    public virtual void PostDialogueInvoke(string functionName)
    {
        Invoke(functionName, 0f);
    }
    
    public virtual void BackToNormal() { }	
}
