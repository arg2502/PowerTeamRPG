using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_FlammableTapestry : OW_Flammable {

    [SerializeField] private TextAsset textAsset;

    public override void Ignite()
    {
        base.Ignite();
        StartCoroutine(BurnThenTalk());
    }

    IEnumerator BurnThenTalk()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        GetComponentInChildren<NPCDialogue>().StartDialogue(textAsset);
    }
}
