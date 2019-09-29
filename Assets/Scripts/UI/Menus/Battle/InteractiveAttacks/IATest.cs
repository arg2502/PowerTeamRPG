using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATest : InteractiveAttack {

    private new void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Attack(Quality.PERFECT);
            Destroy(gameObject);
        }

        base.Update();
    }
}
