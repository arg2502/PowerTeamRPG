using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_DestructiveFlammable : OW_Flammable {

    public override void Ignite()
    {
        base.Ignite();
        Destroy(gameObject);
    }
}
