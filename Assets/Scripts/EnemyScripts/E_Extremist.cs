using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Extremist : Enemy {

    Skill slapstick;

    // Use this for initialization
    protected override void AssignAttack()
    {
        slapstick = skillTree.slapstick;

        SkillTreeManager.AddTechnique(Data, slapstick);
        
        defaultAttack = slapstick;
    }
   
    void SlapStick()
    {
        SingleAttack(slapstick);
    }

    public override Technique ChooseAttack()
    {
        //CLEAR the targets list
        targets.Clear();

        // choose target first, THEN determine type of attack
        ChooseRandomTarget();

        return slapstick;
    }

    public override void Attack()
    {
        if (string.Equals(CurrentAttackName, slapstick.Name))
            SlapStick();

        base.Attack();
    }
}
