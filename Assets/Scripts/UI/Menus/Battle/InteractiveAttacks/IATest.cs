using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATest : InteractiveAttack {

    private new void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
			quality = Quality.PERFECT;
            Attack();
            Destroy(gameObject);
        }

        base.Update();
    }
}
