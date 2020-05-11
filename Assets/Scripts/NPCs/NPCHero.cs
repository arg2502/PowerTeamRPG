using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHero : StationaryNPCControl {
        
    public void MergeIntoPlayer()
    {
        GameControl.control.currentNPC = null;
        GetComponentInChildren<NPCDialogue>().canTalk = false;
        GameControl.control.SetCharacterState(characterControl.CharacterState.Cutscene);
        var pos = GameObject.FindObjectOfType<characterControl>().transform.position;
        StartCoroutine(Merge(pos));
    }      

    IEnumerator Merge(Vector3 finalPos)
    {
        var spd = 0.1f;
        var vec = (finalPos - transform.position) * spd;
        while(true)
        {
            transform.Translate(vec);
            if (Vector3.Magnitude(finalPos - transform.position) <= 0.1f) break;
            yield return null;
        }

        gameObject.SetActive(false);
        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
    }
}
