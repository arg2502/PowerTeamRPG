using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;

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
    List<Vector3> heroStartingPositions;
    List<Vector3> enemyStartingPositions;
    
    public List<StatsCard> heroStatsList;
    public List<StatsCard> enemyStatsList;

    //bool fleeFailed = false;

    // Battle states
    public enum BattleState
    {
        TARGET,
        ATTACK,
        VICTORY,
        FAILURE,
        FLEE
    }
    public BattleState battleState;

    
    public TargetType targetState;

    //public BattleUI battleUI;
    public BattleCamera battleCamera;
    UIManager uiManager;
    public UI.BattleMenu battleMenu;
    public UnityEngine.UI.Text battleMessage;
    public Hero CurrentHero { get { return heroList[currentDenigen]; } }
    public int CurrentIndex { get { return currentDenigen; } }

	void Start ()
    {
        AddHeroes();
        AddEnemies();
        AssignStatsCards();
        CreateBattleMenu();
        SortBySpeed();
        ShowCurrentFullCard();
    }

    void CreateBattleMenu()
    {
        uiManager = GameControl.UIManager;
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
        //foreach (var hero in GameControl.control.heroList)
        //{
        //    CreateHero(hero.denigenName, hero.identity);
        //}

        
        // TEST ONLY CERTAIN HEROES
        for(int i = 0; i < 3; i++)
        {
            CreateHero(GameControl.control.heroList[i].denigenName, GameControl.control.heroList[i].identity);
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

    void AssignStatsCards()
    {
        // assign heroes their cards and set the stats
        for(int i = 0; i < heroList.Count; i++)
        {
            var hero = heroList[i];
            hero.statsCard = heroStatsList[i];
            hero.statsCard.SetInitStats(hero);
        }

        // turn off any unused cards
        for(int i = heroList.Count; i < heroStatsList.Count; i++)
        {
            heroStatsList[i].gameObject.SetActive(false);
        }

        // ENEMIES
        for(int i = 0; i < enemyList.Count; i++)
        {
            var enemy = enemyList[i];
            enemy.statsCard = enemyStatsList[i];
            enemy.statsCard.SetInitStats(enemy);
        }
        for(int i = enemyList.Count; i < enemyStatsList.Count; i++)
        {
            enemyStatsList[i].gameObject.SetActive(false);
        }
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
            hero.CurrentAttackName = "Strike";
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

    void ShowBattleMenu()
    {
        battleMessage.text = "";
        uiManager.PushMenu(uiManager.uiDatabase.BattleMenu);
    }

    void ChangeBattleState(BattleState state)
    {
        // set the new battle state
        battleState = state;
        print("STATE CHANGE -- " + battleState);
        
        // reset current denigen to traverse through list during attacks
        currentDenigen = 0;

        // determine what happens now in this new state
        if(battleState == BattleState.TARGET)
        {
            StartTargetPhase();
        }
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
        else if (battleState == BattleState.FLEE)
        {
            EndBattle();
        }
    }

    void StartTargetPhase()
    {
        //fleeFailed = false;
        ShowBattleMenu();
        ShowCurrentFullCard();
        battleCamera.BackToStart();
        battleCamera.ZoomTarget();
        ResetBlocking();
    }

    void StartAttackPhase()
    {
        //ToggleAllStatCards(false);

        // have enemies decide their attack
        foreach (var enemy in enemyList)
        {
            enemy.CurrentAttackName = enemy.ChooseAttack();
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
        denigen.Attack(denigen.CurrentAttackName);

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
        while (denigenList[currentDenigen].IsDead || string.IsNullOrEmpty(denigenList[currentDenigen].CurrentAttackName))
        {
            currentDenigen++;
            if (currentDenigen >= denigenList.Count)
            {
                // FOR NOW -- JUST GO BACK TO TARGETING
                ChangeBattleState(BattleState.TARGET);
                return;
            }
        }
        if (currentDenigen < denigenList.Count)
            print("NEXT UP -- " + denigenList[currentDenigen].name);
        AttackDenigen();
    }

    public void KillOff(Denigen deadDenigen)
    {
        // what to do with a denigen that has been killed
        // FOR NOW -- JUST SET THEIR ALPHA TO ZERO
        var color = deadDenigen.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        deadDenigen.GetComponent<SpriteRenderer>().color = color;

        //availableDenigens.Remove(deadDenigen);

        CheckTheDead(deadDenigen);
    }

    void CheckTheDead(Denigen deadDenigen)
    {
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
            if(!hero.IsDead)
                return false;
        }

        return true;
    }
    bool AreAllEnemiesDead()
    {
        foreach (var enemy in enemyList)
        {
            if(!enemy.IsDead)
                return false;
        }

        return true;
    }

    bool IsBattleOver
    {
        get
        {
            if (battleState == BattleState.VICTORY)
                return true;
            else if (battleState == BattleState.FAILURE)
                return true;
            else if (battleState == BattleState.FLEE)
                return true;
            else return false;
        }
    }

    public void DetermineTargetType(string attackName)
    {
        Hero hero = heroList[currentDenigen];
        hero.CurrentAttackName = attackName;
        hero.DecideTypeOfTarget();
        targetState = hero.currentTargetType;
    }
    public bool IsTargetEnemy
    {
        get
        {
            switch (targetState)
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
    public void TargetDenigen(List<Denigen> targets)
    {
        Hero hero = heroList[currentDenigen];
        hero.SelectTarget(targets);
        print(hero.name + "'s target is " + hero.Targets[0].name);

        // disable all menus
        uiManager.DisableAllMenus();
        ShowCurrentShortCard();
        do
        {
            if (currentDenigen < heroList.Count - 1)
                NextTarget();
            else
                GoToAttackState();
        }
        while (heroList[currentDenigen].IsDead);
    }

    void NextTarget()
    {
        currentDenigen++;
        ShowCurrentFullCard();

        // go back to BattleMenu
        ShowBattleMenu();
    }
    void GoToAttackState()
    {
        ChangeBattleState(BattleState.ATTACK);
    }

    void AttackDenigen()
    {
        var denigen = denigenList[currentDenigen];
        denigen.Attack(denigen.CurrentAttackName);
    }
    
    public void NextAttack()
    {
        // if the battle is over, break the cycle        
        if (IsBattleOver)
        {
            EndBattle();
            return;
        }

        // increment up list -- if the next one is dead, continue to the next
        currentDenigen++;
        if (currentDenigen < denigenList.Count)
            FindNextAlive();
        // if we're at the end, end the phase
        else
            ChangeBattleState(BattleState.TARGET);
    }

    public IEnumerator ShowAttack(Denigen attacker, List<Denigen> targeted)
    {
        // first check if we have failed a flee. If we have, then skip all heroes
        //if (fleeFailed && attacker is Hero)
        //{
        //    //NextAttack();
        //    yield break;
        //}

        // show attack
        battleMessage.text = attacker.DenigenName + " uses " + attacker.CurrentAttackName;
        battleCamera.MoveTo(attacker.transform.position);
        battleCamera.ZoomAttack();

        // reduce power magic points at the moment of attack
        attacker.PayPowerMagic();

        // Update UI -- FOR NOW JUST HEROES
//if (attacker is Hero)
        //{
            //ToggleDenigenStatCard(attacker, true);
            attacker.statsCard.UpdateStats();
        //}
        

        var anim = attacker.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(attacker.AttackAnimation))
        {
            // time before and after the animation to give some time to watch the transitions to and from Idle
            var bufferTime = 0.25f;

            yield return new WaitForSeconds(bufferTime);
            //if (attacker is Hero)
                //Debug.Break();
                //Time.timeScale = 0.5f;
            //anim.Play(attacker.AttackAnimation);
            yield return attacker.PlayAnimation();
            //yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + bufferTime);
            Time.timeScale = 1f;
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // hide attacker's stats card
        //if (attacker is Hero)
        //    ToggleDenigenStatCard(attacker, false);

        // we don't need to show any target info if the attacker is just blocking
        if (attacker.IsBlocking)
        {
            NextAttack();
            yield break;
        }

        // show damage
        var messagesToDisplay = new List<string>();

        //if (targeted.Count > 0)
        //    battleCamera.MoveTo(targeted[0].transform.position);
        battleCamera.BackToStart();
        battleCamera.ZoomTarget();
        // wait for camera to get back
        yield return new WaitForSeconds(0.25f);

        foreach (var target in targeted)
        {                        
            // decrease hp based off of damage
            target.Hp -= target.CalculatedDamage;

            //Now record appropriate text
            //takeDamageText.Add(name + " takes " + (int)damage + " damage!");
            var message = "";
            switch(attacker.attackType)
            {
                case Denigen.AttackType.NORMAL:
                    message = "";
                    break;
                case Denigen.AttackType.BLOCKED:
                    message = target.DenigenName + " blocked the attack\n";
                    break;
                case Denigen.AttackType.CRIT:
                    message = attacker.DenigenName + " hit a weak spot!\n";
                    break;
                case Denigen.AttackType.MISS:
                    message = attacker.DenigenName + " missed\n";
                    break;
            }


            message += target.DenigenName + " takes " + target.CalculatedDamage + " damage!";
            messagesToDisplay.Add(message);

            print(target.DenigenName + " takes " + target.CalculatedDamage + " damage!");
            // create the damage effect, but onlu if the denigen is not dead
            //if (statusState != Status.dead && statusState != Status.overkill)
            //{
            GameObject be = (GameObject)Instantiate(Resources.Load("Prefabs/DamageEffect"), target.transform.position, Quaternion.identity);
            be.name = "DamageEffect";
            //be.GetComponent<Effect>().Start();
            be.GetComponent<Effect>().damage = target.CalculatedDamage + "";
            //}

            // check for dead
            print(target.DenigenName + " HP: " + target.Hp);
            if (target.Hp <= 0)
            {
                target.Hp = 0;
                //takeDamageText.Add(name + " falls!");
                print(target.DenigenName + " falls!");
                messagesToDisplay.Add(target.DenigenName + " falls!");
                target.StatusState = DenigenData.Status.dead;
                KillOff(target);
            }

            // Update UI
            //if (target is Hero)
            //{
                //ToggleDenigenStatCard(target, true);
                target.statsCard.UpdateStats();
            //}
        }
        for (int i = 0; i < messagesToDisplay.Count; i++)
        {
            battleMessage.text = messagesToDisplay[i];
            yield return new WaitForSeconds(1f);
        }

        // hide target's cards
        //foreach (var target in targeted)
        //{
        //    if (target is Hero)
        //        ToggleDenigenStatCard(target, false);
        //}

        NextAttack();
    }

    void EndBattle()
    {
        if (battleState == BattleState.VICTORY)
            battleMessage.text = "VICTORY!";
        else if (battleState == BattleState.FAILURE)
            battleMessage.text = "FAILURE";
        else if (battleState == BattleState.FLEE)
            battleMessage.text = "Flee successful";
    }

    /// <summary>
    /// Called after the end of the attack phase -- resets blocking to false for next round
    /// </summary>
    void ResetBlocking()
    {
        foreach(var denigen in denigenList)
        {
            if (denigen.IsBlocking)
                denigen.IsBlocking = false;
        }
    }

    public bool CalcFlee()
    {
        // calculate the likelihood of flight
        int enemyMight = 0;
        int heroMight = 0;
        foreach (Enemy e in enemyList)
        {
            if (!e.IsDead) enemyMight += e.Level * e.Stars;
        }
        foreach (Hero h in heroList)
        {
            if (!h.IsDead) heroMight += h.Level * h.Stars;
        }
        float likelihood = (60 + heroMight - enemyMight) / 100.0f;

        //see if the player succeeds or fails
        float num = Random.Range(0.0f, 1.0f);
        if (num > likelihood) return true;
        else return false;
    }

    public void FleeBattle()
    {
        ChangeBattleState(BattleState.FLEE);
    }

    public void FleeFailed()
    {
        ShowAllShortCards();
        StartCoroutine(ShowFleeFailed());
    }
    IEnumerator ShowFleeFailed()
    {
        battleMessage.text = "Failed to flee";
        //fleeFailed = true;
        foreach (var hero in heroList)
            hero.CurrentAttackName = "";

        yield return new WaitForSeconds(1f);
        GoToAttackState();
    }

    // STATS CARDS
    void ShowCurrentFullCard()
    {
        var card = heroList[currentDenigen].statsCard;
        card.gameObject.SetActive(true);
        card.ShowFullCard();
    }
    void ShowCurrentShortCard()
    {
        var card = heroList[currentDenigen].statsCard;
        card.gameObject.SetActive(true);
        card.ShowShortCard();
    }
    void ToggleAllStatCards(bool show)
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            //heroStatsList[i].gameObject.SetActive(show);
            heroStatsList[i].ShowShortCard();
        }
        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyStatsList[i].ShowShortCard();
        }
    }
    void ToggleDenigenStatCard(Denigen denigen, bool show)
    {
        //denigen.statsCard.gameObject.SetActive(show);
        if (show)
            denigen.statsCard.ShowFullCard();
        else
            denigen.statsCard.ShowShortCard();
    }

    void ShowAllShortCards()
    {
        ToggleAllStatCards(false);
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