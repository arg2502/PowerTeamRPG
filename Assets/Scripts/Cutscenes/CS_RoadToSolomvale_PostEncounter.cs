using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_RoadToSolomvale_PostEncounter : Cutscene {

    public override void Play()
    {        
        cam.SwitchTarget(cs_camera);        

        base.Play();
    }

    public override void Stop()
    {
        base.Stop();

        StartCoroutine(CS_Stop());
    }

    IEnumerator CS_Stop()
    {
        GameControl.control.currentCharacter = characterControl.HeroCharacter.JETHRO; // force back to Jethro
        GameControl.control.currentPosition = cs_jethro.transform.position;
        GameControl.control.SetCharacterPositionToCurrent();
        GameControl.control.Character.SetLastMovement(-1, 0);
        cs_jethro.gameObject.SetActive(false);
        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
        cam.SetLerp(true);
        cam.SwitchBack();
        yield return new WaitForSeconds(1); // wait a sec before setting lerp off
        cam.SetLerp(false);
    }

}
