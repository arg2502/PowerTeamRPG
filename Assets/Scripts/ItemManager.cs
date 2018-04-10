using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager {
    
    public ItemManager()
    {

    }

    public bool ItemForLiving(string item)
    {
        // TEST FOR NOW
        if (item == "Lesser Restorative")
            return false;
        return true;
    }
}
