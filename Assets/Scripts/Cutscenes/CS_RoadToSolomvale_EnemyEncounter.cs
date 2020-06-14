﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_RoadToSolomvale_EnemyEncounter : Cutscene {

    public GameObject cs_camera;
    CameraController cam;

    public List<EnemyData> commonEnemy;

    public override void Play()
    {        
        cam = FindObjectOfType<CameraController>();
        cam.SwitchTarget(cs_camera);
        cam.SetSpeed(3);
        cam.SetLerp(true);

        base.Play();
    }

    public override void Stop()
    {
        base.Stop();

        GameControl.control.GoToBattleScene(commonEnemy);
    }

}
