using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Meeting_Eleanor : Cutscene {

    public GameObject cs_camera;
    CameraController cam; // make this better in the future -- perhaps create a CameraController in GameControl

    public override void Play()
    {
        cam = FindObjectOfType<CameraController>();
        cam.SwitchTarget(cs_camera);
        cam.SetLerp(true);
        cam.SetSpeed(10);
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
        cam.SetLerp(false);
        cam.SwitchBack();
        cam.SetSpeedBack();
        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
    }
}
