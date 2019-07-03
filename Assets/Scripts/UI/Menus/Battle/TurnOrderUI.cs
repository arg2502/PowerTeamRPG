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
    
    public void SetAsLast()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ArrowHighlight(bool isHighlighted)
    {
        arrow.gameObject.SetActive(isHighlighted);
    }

    public void StarburstHighlight(bool isHighlighted)
    {
        starburst.gameObject.SetActive(isHighlighted);
    }
}
