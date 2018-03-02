using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

    int currentDenigen = 0;

    public List<Denigen> denigenList = new List<Denigen>();
    //List<Denigen> availableDenigens = new List<Denigen>();
    public List<Hero> heroList = new List<Hero>();
    public List<Enemy> enemyList = new List<Enemy>();

    List<string> enemiesToAdd = new List<string>();
    int numOfEnemies;

    public Transform heroContainer;
    public Transform enemyContainer;

    // starting positions
    public List<GameObject> heroPositions;
    public List<GameObject> enemyPositions;
    //Vector3 jethroStart;// = new Vector3(1.5f, -3f);
    //Vector3 coleStart;// = new Vector3(-3f, -1f);
    //Vector3 eleanorStart;// = new Vector3(0f, 3f);
    //Vector3 joulietteStart;// = new Vector3(4.5f, 1f);
    List<Vector3> heroStartingPositions;
    List<Vector3> enemyStartingPositions;

    // Battle states
    public enum BattleState
    {
        TARGET,
        ATTACK,
        VICTORY,
        FAILURE
    }
    public BattleState battleState;

    
    public TargetType targetState;

    public UI.BattleUI battleUI;

    //UI.UIManager uiManager;
    UI.BattleMenu battleMenu;

	void Start ()
    {
        CreateBattleMenu();
        AddHeroes();
        AddEnemies();
        battleUI.Init();
        SortBySpeed();
        //PrintHeroes();
	}

    void CreateBattleMenu()
    {
        var uiManager = GameControl.UIManager;
        var battleMenuObj = uiManager.uiDatabase.BattleMenu;
        uiManager.PushMenu(battleMenuObj);
        battleMenu = uiManager.FindMenu(battleMenuObj) as UI.BattleMenu;
    }
    void AddHeroes()
    {
        // set hero positions
        heroStartingPositions = new List<Vector3>();// { jethroStart, coleStart, eleanorStart, joulietteStart };
        foreach (var go in heroPositions)
        {
            heroStartingPositions.Add(go.transform.localPosition);
            go.GetComponent<SpriteRenderer>().enabled = false; // turn off placeholder renderer
        }

        // create the heroes that we currently have in our party
        foreach (var hero in GameControl.control.heroList)
        {
            CreateHero(hero.denigenName, hero.identity);
        }
    }
    
    void CreateHero(string heroName, int index) 
    {
        // special case -- if we're Jethro, our player name may not be Jethro, but the prefab is
        if (index == 0)
            heroName = "Jethro";

        var heroObj = GameObject.Instantiate(Resources.Load("Prefabs/HeroesBattle/" + heroName + "Prefab")) as GameObject;
        heroObj.transform.SetParent(heroContainer);
        var hero = heroObj.GetComponent<Hero>();
        hero.Data = GameControl.control.heroList[index];
        hero.transform.localPosition = heroStartingPositions[index];
        denigenList.Add(hero);
        heroList.Add(hero);
    }
    
    void AddEnemies()
    {
        // set enemy positions
        enemyStartingPositions = new List<Vector3>();
        foreach (var go in enemyPositions)
        {
            enemyStartingPositions.Add(go.transform.localPosition);
            go.GetComponent<SpriteRenderer>().enabled = false; // turn off placeholder renderer
        }

        // enemiesToAdd should probably be set by the Enemy that you collided with to start the battle
        // this could probably be stored inside GameControl to transfer the data over to the battle
        // FOR NOW -- LET'S JUST MANUALLY ADD A BUNCH OF SHIT
        int numOfGoikkos = 3;
        for (int i = 0; i < numOfGoikkos; i++)
            enemiesToAdd.Add("Goikko");

        // call CreateEnemies on each enemy to add to create the enemies
        foreach (var enemy in enemiesToAdd)
            CreateEnemy(enemy);
    }

    void CreateEnemy(string enemyName)
    {
        // create Enemy object
        var enemyObj = Instantiate(Resources.Load("Prefabs/EnemiesBattle/" + enemyName)) as GameObject;
        enemyObj.transform.SetParent(enemyContainer);
        var enemy = enemyObj.GetComponent<Enemy>();

        var enemyData = Instantiate(Resources.Load<EnemyData>("Data/Enemies/" + enemyName));

        enemy.Data = enemyData;
        enemy.Init();
        if(numOfEnemies < enemyStartingPositions.Count)
            enemy.transform.localPosition = enemyStartingPositions[numOfEnemies];
        numOfEnemies++;
        denigenList.Add(enemy);
        enemyList.Add(enemy);
    }

    void SortBySpeed()
    {
        Denigen temp;
        for (int j = 0; j < denigenList.Count; j++)
        {
            for (int i = 0; i < denigenList.Count - 1; i++)
            {
                if (denigenList[i].Spd < denigenList[i + 1].Spd)
                {
                    temp = denigenList[i + 1];
                    denigenList[i + 1] = denigenList[i];
                    denigenList[i] = temp;
                }
            }
        }

        Hero tempHero;
        for (int j = 0; j < heroList.Count; j++)
        {
            for (int i = 0; i < heroList.Count - 1; i++)
            {
                if (heroList[i].Spd < heroList[i + 1].Spd)
                {
                    tempHero = heroList[i + 1];
                    heroList[i + 1] = heroList[i];
                    heroList[i] = tempHero;
                }
            }
        }
    }

    void PrintHeroes()
    {
        foreach(var denigen in denigenList)
        {
            print("object: " + denigen.name);
            print("speed: " + denigen.Data.spd);
        }
    }

    public void TestTarget()
    {
        if (battleState != BattleState.TARGET)
        {
            print("We're not targeting right now");
            return;
        }

        //if (denigenList[currentDenigen] is Hero)
        //{
        //Hero hero = denigenList[currentDenigen] as Hero;
        Hero hero = heroList[currentDenigen];
            hero.CurrentAttack = "Strike";
            //hero.SelectTarget(hero.CurrentAttack);
            print(hero.name + "'s target is " + hero.Targets[0].name);
        //}

        // find the next hero that can target -- if no more, end phase
        do
        {
            if (currentDenigen < heroList.Count - 1)
                currentDenigen++;
            else
                ChangeBattleState(BattleState.ATTACK);
        } while (heroList[currentDenigen].IsDead);

        //print("TARGET -- AFTER WHILE");
    }

    void ChangeBattleState(BattleState state)
    {
        // set the new battle state
        battleState = state;
        print("STATE CHANGE -- " + battleState);
        
        // reset current denigen to traverse through list during attacks
        currentDenigen = 0;

        // determine what happens now in this new state
        if (battleState == BattleState.ATTACK)
        {
            StartAttackPhase();
        }

        else if(battleState == BattleState.VICTORY)
        {
            print("VICTORY SCREECH");
        }

        else if(battleState == BattleState.FAILURE)
        {
            print("*smash announcer voice* FAILURE");
        }
    }

    void StartAttackPhase()
    {
        // have enemies decide their attack
        foreach(var enemy in enemyList)
        {
            enemy.CurrentAttack = enemy.ChooseAttack();
        }

        // resort in case there have been speed changes
        SortBySpeed();

        // make sure the first denigen to attack is alive
        FindNextAlive();

    }

    public void TestAttack()
    {
        if(battleState != BattleState.ATTACK)
        {
            print("We're not attacking right now");
            return;
        }

        var denigen = denigenList[currentDenigen];
        //print(denigen.name + " uses " + denigen.CurrentAttack);
        denigen.Attack(denigen.CurrentAttack);

        //print(denigen.name + " state: " + denigen.StatusState);

        // increment up list -- if the next one is dead, continue to the next
        currentDenigen++;
        if (currentDenigen < denigenList.Count)
            FindNextAlive();
        // if we're at the end, end the phase
        else
            ChangeBattleState(BattleState.TARGET);

        //print("ATTACK -- AFTER WHILE");
    }

    void FindNextAlive()
    {
        while (denigenList[currentDenigen].IsDead)
        {
            currentDenigen++;
            if (currentDenigen >= denigenList.Count)
            {
                // FOR NOW -- JUST GO BACK TO TARGETING
                ChangeBattleState(BattleState.TARGET);
                break;
            }
        }
        if (currentDenigen < denigenList.Count)
            print("NEXT UP -- " + denigenList[currentDenigen].name);
    }

    public void KillOff(Denigen deadDenigen)
    {
        // what to do with a denigen that has been killed
        // FOR NOW -- JUST SET THEIR ALPHA TO ZERO
        var color = deadDenigen.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        deadDenigen.GetComponent<SpriteRenderer>().color = color;

        //availableDenigens.Remove(deadDenigen);

        // check for victory or loss
        if (deadDenigen is Hero)
        {
            if (AreAllHeroesDead())
                ChangeBattleState(BattleState.FAILURE);
        }
        else
        {
            if (AreAllEnemiesDead())
                ChangeBattleState(BattleState.VICTORY);
        }
    }

    bool AreAllHeroesDead()
    {
        foreach(var hero in heroList)
        {
            //if (hero.StatusState != Denigen.Status.dead && hero.StatusState != Denigen.Status.overkill)
            if(hero.IsDead)
                return false;
        }

        return true;
    }
    bool AreAllEnemiesDead()
    {
        foreach (var enemy in enemyList)
        {
            //if (enemy.StatusState != Denigen.Status.dead && enemy.StatusState != Denigen.Status.overkill)
            if(enemy.IsDead)
                return false;
        }

        return true;
    }

    public void DetermineTargetType(string attackName)
    {
        Hero hero = heroList[currentDenigen];
        hero.CurrentAttack = attackName;
        hero.DecideTypeOfTarget();
        //SetUpTarget();
    }
    //public void SetUpTarget()
    //{
    //    switch (targetState)
    //    {
    //        case TargetType.ENEMY_SINGLE:                
    //            AssignSingle();
    //            break;
    //    }
    //}

    //void AssignSingle()
    //{
    //    // find first living denigen


    //}
    public void TargetDenigen(List<Denigen> targets)
    {
        Hero hero = heroList[currentDenigen];
        hero.SelectTarget(targets);
        print(hero.name + "'s target is " + hero.Targets[0].name);

        do
        {
            if (currentDenigen < heroList.Count - 1)
                currentDenigen++;
            else
                ChangeBattleState(BattleState.ATTACK);
        } while (heroList[currentDenigen].IsDead);
    }

    public bool IsTargetEnemy
    {
        get
        {
            switch(targetState)
            {
                case TargetType.ENEMY_SINGLE:
                case TargetType.ENEMY_SPLASH:
                case TargetType.ENEMY_TEAM:
                    return true;
                default:
                    return false;
            }
        }
    }

}
// Target type
// for determining what kind and how many targets a denigen can affect depending on their chosen attack
public enum TargetType
{
    ENEMY_SINGLE,
    ENEMY_SPLASH,
    ENEMY_TEAM,
    HERO_SELF,
    HERO_SINGLE,
    HERO_SPLASH,
    HERO_TEAM
}