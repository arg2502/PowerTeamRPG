using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveAttack : MonoBehaviour {

    public enum Quality { MISS, POOR, OKAY, GOOD, GREAT, PERFECT }
    protected Quality quality;
    List<float> damage;
    protected BattleManager battleManager;
    protected List<float> xPosList;
    public GameObject parentRectTransform;

    protected void Init(List<float> originalDamage)
    {
		print ("ia init");
        damage = originalDamage;
        battleManager = FindObjectOfType<BattleManager>();        
    }

    protected void CreatePositionsOnLine(int num)
    {
        xPosList = new List<float>();
        var rect = parentRectTransform.GetComponent<RectTransform>().rect;
        switch (num)
        {
            case 1: xPosList = new List<float>() { 0f }; break;
            case 2: xPosList = new List<float>() { -rect.width / 6f, rect.width / 6f }; break;
            case 3: xPosList = new List<float>() { -rect.width / 4f, 0f, rect.width / 4f }; break;
        }
    }

    protected void Attack(Quality q, bool immediately = false)
    {
        float percentage;
        switch(q)
        {
            case Quality.PERFECT: percentage = 1.5f; break;
            case Quality.GREAT: percentage = 1.25f; break;
            case Quality.GOOD: percentage = 1f; break;
            case Quality.OKAY: percentage = .75f; break;
            case Quality.POOR: percentage = .5f; break;
            default: percentage = 1f; break;
        }
        for (int i = 0; i < damage.Count; i++)
            damage[i] *= percentage;
        battleManager.ReturnFromInteraction(damage, immediately);
    }

    protected void SetAttack(GameObject target, Quality quality, bool immediately = false)
    {
        //var textObj = target.GetComponentInChildren<Text>(true);
        var textObj = GameControl.UIManager.ShowQualityUI(target.transform).GetComponent<Text>();
        textObj.gameObject.SetActive(true);
        string text;
        switch (quality)
        {
            case Quality.PERFECT: text = "PERFECT"; break;
            case Quality.GREAT: text = "GREAT"; break;
            case Quality.GOOD: text = "GOOD"; break;
            case Quality.OKAY: text = "OKAY"; break;
            case Quality.POOR: text = "POOR"; break;
            case Quality.MISS: text = "MISS"; break;
            default: text = ""; break;
        }
        textObj.text = text;
        //currentTarget++;
        Attack(quality, immediately);
    }

    // Update is called once per frame
    protected void Update () {
		
	}
}
