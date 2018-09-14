using UnityEngine;
using System.Collections;

public class Enemy : Denigen {

    //Amount of experience awarded for defeating this enemy
    int exp;
    // A number for tweaking the amount of experience rewarded, based on species
    //protected int expMultiplier;

    public int ExpGiven { get { return exp; } }

    //amount of gold awarded for defeating this enemy
    int gold;
    //a number for tweaking the amount of gold awarded, based on species
    //protected int goldMultiplier;

    public int Gold { get { return gold; } }

    //Median level for enemies in this region, dependant on player's team's highest level when they first entered this region.
    //Will probably be pulled from an array
    int areaLevel;

    //States dependant on health, to influence the enemy decision making. This should make them appear smarter
    protected enum Health { high, average, low, dangerous};
    protected Health healthState = Health.high;

    EnemyData enemyData;
    public int ExpMultiplier { get { return enemyData.expMultiplier; } set { enemyData.expMultiplier = value; } }
    public int GoldMultiplier { get { return enemyData.goldMultiplier; } set { enemyData.goldMultiplier = value; } }

    protected EnemySkillTree skillTree;

    public Technique defaultAttack; // the fallback free attack for the enemy when they don't have enough PM

    private bool taunted = false;

	// Use this for initialization
	public void Init () {
        //set the base stats for the enemy
        base.Awake();

        // cast as EnemyData to get enemy-specific variables
        enemyData = data as EnemyData;

        //get the areaLevel from the gameControl obj -- ADD LATER
        areaLevel = 3;

        //set the enemy's level within a range of +/- 2 of the area level -- this range can be changed later, if desired
        var startLevel = Random.Range((areaLevel - 2), (areaLevel + 2));
        //level up until desired level is hit (minus 1 because level is already 1)
        for (int i = 0; i < startLevel - 1; i++)
        {
            data.LevelUp();
        }

        // Calculate the experience and gold this enemy should award
        exp = Stars * ExpMultiplier * Level;
        gold = Stars * GoldMultiplier;

        Rename();

        skillTree = GameControl.skillTreeManager.enemySkillTree;

        AssignAttack();
	}

    /// <summary>
    /// Sets the child enemies attacks based off of the enemy Skill Tree
    /// </summary>
    protected virtual void AssignAttack() { }

    protected void TakeDamage(float damage, bool isMagic)
    {
        //calculate damage
        base.TakeDamage(this, damage, isMagic);

        //after loss of health, a change of healthStatus may be required
        if (Hp >= HpMax * 0.8f) { healthState = Health.high; }
        else if (Hp < HpMax * 0.8f && Hp >= HpMax * 0.5f) { healthState = Health.average; }
        else if (Hp < HpMax * 0.5f && Hp >= HpMax * 0.2f) { healthState = Health.low; }
        else { healthState = Health.dangerous; }
    }

    // The brain of the enemy
    // Every enemy will have this method, but the code for each will be tailored to it's species
    // Since every attack will be different, choosing a target should be handled in specific attack methods.
    public virtual Technique ChooseAttack()
    {
        return null;
    }    

    public bool NotEnoughPM()
    {
        return (Pm - CurrentAttack.Pm) < 0; // true if the current attack will take the enemy's PM below 0
    }

    public override void ChooseTarget()
    {
        base.ChooseTarget();

        // choose attack
        if (!taunted)
        {
            CurrentAttackName = ChooseAttack().Name;

            // check if the enemy has enough PM. If not, use the default attack
            if (NotEnoughPM())
                CurrentAttackName = defaultAttack.Name;
        }
    }

    public void TauntAttack()
    {
        // set the target to 'taunted' so that the denigen will not go through their normal targeting phase
        taunted = true;

        // now we need our target to target Joules with a single target attack
        // we could either search through and find an attack with type of single target
        // or we could just set the attack to their default attack and make sure when we assign that they are single targets
        // ...or both
        CurrentAttackName = defaultAttack.Name;
        if (CurrentAttack.targetType != TargetType.ENEMY_SINGLE)
            Debug.LogError("Default Attack is not a single target attack.");

        // make sure Jouliette is the only target -- she will always be heroList[3]
        targets.Clear();
        var joules = battleManager.heroList.Find(hero => hero.Data.identity == 3);
        print(joules.DenigenName);
        targets.Add(joules);
    }

    public override void Attack()
    {
        attackAnimation = CurrentAttackName;

        // reset taunted
        taunted = false;

        base.Attack();
    }

    // This method will make it easier to distinguish the enemies
    protected void Rename()
    {
        int i = 0;

        foreach (Enemy e in battleManager.enemyList)
        {
            if (e != this && e.name.Contains(name)) { i++; }
            if (e == this) { break; }
        }

        string additional = "";

        if (i == 1) { additional += " B"; }
        else if (i == 2) { additional += " C"; }
        else if (i == 3) { additional += " D"; }
        else if (i == 4) { additional += " E"; }

        name += additional;
        DenigenName += additional;
    }

    protected void ChooseSelfTarget()
    {
        targets.Add(this);
    }

    protected void ChooseRandomTarget()
    {
        int random = 0;
        do
        {
            random = Random.Range(0, battleManager.heroList.Count);
        } while (battleManager.heroList[random].IsDead);

        targets.Add(battleManager.heroList[random]);
    }

    protected void ChooseHighestHPTarget()
    {
        Hero highestHP = null;
        
        foreach (var h in battleManager.heroList)
        {
            // if null (first hero), set it
            if (highestHP == null)
                highestHP = h;

            // otherwise, if we find a hero with higher HP, set it to that new hero
            else if (h.Hp > highestHP.Hp)
                highestHP = h;
        }

        targets.Add(highestHP);
    }
}
