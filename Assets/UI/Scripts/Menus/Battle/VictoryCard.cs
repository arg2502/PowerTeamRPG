using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCard : MonoBehaviour {

    public Image portrait;
    public Text title;
    public Text level;
    public Image expValue;
    public Text currentExp;
    public Text maxExp;
    Denigen currentDenigen;

    public void Init(Denigen denigen)
    {
        currentDenigen = denigen;
        portrait.sprite = currentDenigen.Portrait;
        title.text = currentDenigen.DenigenName;
        level.text = currentDenigen.Level.ToString();
        currentExp.text = currentDenigen.Exp.ToString();
        maxExp.text = currentDenigen.ExpToLevelUp.ToString();
        expValue.fillAmount = (float)currentDenigen.Exp / currentDenigen.ExpToLevelUp;
    }
}
