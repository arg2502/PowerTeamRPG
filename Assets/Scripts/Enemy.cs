using UnityEngine;
using System.Collections;

public class Enemy : Denigen {

    //Amount of experience awarded for defeating this enemy
    int exp;
    // A number for tweaking the amount of experience rewarded, based on species
    protected int expMultiplier;

    //Median level for enemies in this region, dependant on player's team's highest level when they first entered this region.
    //Will probably be pulled from an array
    int areaLevel;

    //States dependant on health, to influence the enemy decision making. This should make them appear smarter
    protected enum Health { high, average, low, dangerous};
    protected Health healthState = Health.high;

	// Use this for initialization
	protected void Start () {
        //set the base stats for the enemy
        base.Start();

        //get the areaLevel -- ADD LATER
        areaLevel = 3;

        //set the enemy's level within a range of +/- 2 of the area level -- this range can be changed later, if desired
        level = Random.Range((areaLevel - 2), (areaLevel + 2));
        //level up until desired level is hit
        for (int i = 0; i < level; i++)
        {
            base.LevelUp(i + 1);
        }

        // Calculate the experience this enemy should award
        exp = stars * expMultiplier * level;
	}

    protected void TakeDamage(float damage, bool isMagic)
    {
        //calculate damage
        base.TakeDamage(damage, isMagic);

        //after loss of health, a change of healthStatus may be required
        if (hp >= hpMax * 0.8f) { healthState = Health.high; }
        else if (hp < hpMax * 0.8f && hp >= hpMax * 0.5f) { healthState = Health.average; }
        else if (hp < hpMax * 0.5f && hp >= hpMax * 0.2f) { healthState = Health.low; }
        else { healthState = Health.dangerous; }
    }

    // The brain of the enemy
    // Every enemy will have this method, but the code for each will be tailored to it's species
    // Since every attack will be different, choosing a target should be handled in specific attack methods.
    public virtual string ChooseAttack()
    {
        return null;
    }

	// Update is called once per frame
	void Update () {
	  
	}
}
