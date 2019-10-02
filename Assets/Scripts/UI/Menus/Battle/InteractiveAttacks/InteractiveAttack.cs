using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAttack : MonoBehaviour {

    public enum Quality { MISS, POOR, OKAY, GOOD, GREAT, PERFECT }
    protected Quality quality;
    float damage;
    protected BattleManager battleManager;

    protected void Init(float originalDamage)
    {
        damage = originalDamage;
        battleManager = FindObjectOfType<BattleManager>();
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
