using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SolomvaleEnter : Cutscene {
    
    public override void Play()
    {        
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
