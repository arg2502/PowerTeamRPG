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

        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
        cam.SetLerp(false);
        cam.SwitchBack();
        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
    }
}
