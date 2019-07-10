﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestDialogue : NPCDialogue {

	public override void StartDialogue()
    {
        base.StartDialogue();

        GetComponentInParent<TreasureChest>().Open();
    }
}