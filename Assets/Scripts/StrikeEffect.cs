using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeEffect : MonoBehaviour {
    
    public void PlayAnimation()
    {
        StartCoroutine(PlayClipAndDisable());        
    }

    IEnumerator PlayClipAndDisable()
    {
        var anim = GetComponent<Animator>();
        anim.Play("StrikeEffect", -1, 0f);
        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        gameObject.SetActive(false);
    }
}
