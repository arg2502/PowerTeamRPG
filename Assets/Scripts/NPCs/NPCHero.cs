using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHero : StationaryNPCControl {

    characterControl charCon;
    public enum DisperseDirection { NONE, UP, DOWN, LEFT, RIGHT }
    float disperseDist = 1f;
    float mergeSpd = 0.05f;

    private void Start()
    {
        base.Start();
        SetCharacterControl();
    }

    public IEnumerator MergeIntoPlayer()
    {
        //if (GameControl.control.currentObj == GetComponentInChildren<NPCDialogue>())
        //{
        //    GameControl.control.currentObj = null;
        //    GetComponentInChildren<NPCDialogue>().canTalk = false;
        //}
        GameControl.control.SetCharacterState(characterControl.CharacterState.Cutscene);
        var pos = charCon.transform.position;
        yield return StartCoroutine(Merge(pos));
    }      
        
    IEnumerator Merge(Vector3 finalPos)
    {
        var vec = (finalPos - transform.position) * mergeSpd;
        while(true)
        {
            transform.Translate(vec);
            if (Vector3.Magnitude(finalPos - transform.position) <= 0.1f) break;
            yield return null;
        }

        gameObject.SetActive(false);
        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);        
    }

    public void DisperseFromPlayer(DisperseDirection dir1, DisperseDirection dir2 = DisperseDirection.NONE)
    {
        var charPos = charCon.transform.position;
        charPos = AddDirection(charPos, dir1); // adds basic direction
        charPos = AddDirection(charPos, dir2); // optional -- for if we want northwest, or backwards twice (maybe there should be a third? or list of directions?)

        StartCoroutine(Disperse(charPos));
    }

    IEnumerator Disperse(Vector3 finalPos)
    {
        transform.position = charCon.transform.position;
        gameObject.SetActive(true);
        var spd = 0.1f;
        var vec = (finalPos - transform.position) * spd;

        while(true)
        {
            transform.Translate(vec);
            if (Vector3.Magnitude(finalPos - transform.position) <= 0.1f) break;
            yield return null;
        }
    }
    
    public void SetCharacterControl()
    {
        charCon = GameObject.FindObjectOfType<characterControl>();
    }
    
    public void SetHeroAnim(RuntimeAnimatorController cont)
    {
        GetComponent<Animator>().runtimeAnimatorController = cont;
    }

    Vector3 AddDirection(Vector3 pos, DisperseDirection dir)
    {
        if (dir == DisperseDirection.UP) pos.y += disperseDist;
        else if (dir == DisperseDirection.DOWN) pos.y -= disperseDist;
        else if (dir == DisperseDirection.LEFT) pos.x -= disperseDist;
        else if (dir == DisperseDirection.RIGHT) pos.x += disperseDist;
        return pos;
    }

    public void SetFacingDirection(Direction dir)
    {
        defaultDirection = dir;
    }
}
