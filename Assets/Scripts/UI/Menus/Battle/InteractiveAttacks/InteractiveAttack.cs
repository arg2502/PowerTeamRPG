﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAttack : MonoBehaviour {

    public enum Quality { MISS, POOR, OKAY, GOOD, GREAT, PERFECT }
    protected Quality quality;
    float damage;
    protected BattleManager battleManager;
    protected List<float> xPosList;
    public GameObject parentRectTransform;

    protected void Init(float originalDamage)
    {
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

        damage *= percentage;
        battleManager.ReturnFromInteraction((int)damage, immediately);
    }

	// Update is called once per frame
	protected void Update () {
		
	}
}
