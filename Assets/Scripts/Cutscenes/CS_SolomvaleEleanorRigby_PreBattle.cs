using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SolomvaleEleanorRigby_PreBattle : Cutscene {

    public List<EnemyData> clownData;

    public override void Stop()
    {
        base.Stop();

        GameControl.control.GoToBattleScene(clownData);
    }

}
