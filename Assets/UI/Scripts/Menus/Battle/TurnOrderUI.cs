using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderUI : MonoBehaviour {

    public Image starburst;
    public Image arrow;
    public Image denigenPortrait;
    public Denigen denigen;
    
    public void Init(Denigen thisDenigen)
    {
        denigen = thisDenigen;
        denigenPortrait.sprite = denigen.Portrait;
    }	
    
    public void SetAsFirst()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetAsFirstSibling();
    }

    public void Disable()
    {
        //denigen = null;
        //denigenPortrait.sprite = null;
        gameObject.SetActive(false);
    }

    public void Highlight(bool isHighlighted)
    {
        arrow.gameObject.SetActive(isHighlighted);
    }
}
