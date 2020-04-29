using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SolomvaleEnter : Cutscene {

    public GameObject cs_camera;
    CameraController cam; // make this better in the future -- perhaps create a CameraController in GameControl

    public override void Play()
    {
        cam = FindObjectOfType<CameraController>();
        cam.SwitchTarget(cs_camera);
        cam.SetLerp(true);
        
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
        cam.SetLerp(false);                
        StartCoroutine(FadeThenSwitchScene());
    }

    IEnumerator FadeThenSwitchScene()
    {
        yield return cam.Fade(false);

        GameControl.control.LoadSceneAsync("SolomvaleEleanorRigby");
    }
}
