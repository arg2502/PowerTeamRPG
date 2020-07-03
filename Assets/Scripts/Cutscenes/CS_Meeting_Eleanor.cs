using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Meeting_Eleanor : Cutscene {

    public GameObject cs_camera;
    CameraController cam; // make this better in the future -- perhaps create a CameraController in GameControl

    public NPCHero cs_jethro, cs_cole, cs_eleanor;
    public OverworldObject cs_father, real_father;


    public override void Play()
    {
        cam = FindObjectOfType<CameraController>();
        cam.SwitchTarget(cs_camera);
        cam.SetLerp(true);
        cam.SetSpeed(10);
        real_father.gameObject.SetActive(false);
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();

        cam.SwitchBack();
        cam.SetSpeedBack();

        StartCoroutine(MergePlayers());

        cam.SetLerp(false);

        real_father.transform.position = cs_father.transform.position;
        real_father.gameObject.SetActive(true);
        cs_father.gameObject.SetActive(false);

        GameControl.questTracker.NextSubquest("solomvale");
    }
}
