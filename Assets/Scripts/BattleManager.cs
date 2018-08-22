using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UI;

// Not a static var in GameControl, because we do not need a BattleManager at all times
public class BattleManager : MonoBehaviour {

    int currentDenigen = 0;

    public GameObject battlePlatform;
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

    StatsCardManager statsCardManager;    
    //public List<StatsCard> heroStatsList;
    //public List<StatsCard> enemyStatsList;

    const int MAX_HEROES = 4;
    const int MAX_ENEMIES = 5;

    public Sprite damageIcon;
    public Sprite healIcon;
    public Sprite statusEffectIcon;

    public Sprite infectedIcon;
    public Sprite bleedingIcon;
    public Sprite blindedIcon;
    public Sprite petrifiedIcon;
    public Sprite cursedIcon;

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

    public MenuState menuState;

    public TargetType targetState;

    //public BattleUI battleUI;
    public BattleCamera battleCamera;
    UIManager uiManager;
    public UI.BattleMenu battleMenu;
    public Hero CurrentHero { get { return heroList[currentDenigen]; } }
    public int CurrentIndex { get { return currentDenigen; } }
    List<string> messagesToDisplay;

    public GameObject DescriptionObj;
    public Text DescriptionText;

    public List<TurnOrderUI> turnOrder;

    public GameObject hpBarPrefab;

    [Header("TEST VARS")]
    public int TEST_numOfEnemies;
    public List<string> TEST_listOfEnemies;

	void Start ()
    {
        AddHeroes();
        AddEnemies();
        AssignStatsCards();
        InitTurnOrder();
        CreateBattleMenu();
        ChangeBattleState(BattleState.TARGET);
        //SortBySpeed();
        //ShowCurrentFullCard();
        //foreach(var d in denigenList)
        //{
        //    print(d.DenigenName + ": " + d.Hp);
        //}
    }

    void CreateBattleMenu()
    {
        uiManager = GameControl.UIManager;
        var battleMenuObj = uiManager.uiDatabase.BattleMenu;
        //uiManager.PushMenu(battleMenuObj);
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
        for(int i = 0; i < 4; i++)
        {
            CreateHero(GameControl.control.heroList[i].denigenName, GameControl.control.heroList[i].identity);
        }
    }
    
    void CreateHero(string heroName, int index) 
    {
        // if we're already at our max amount, break away (just in case)
        if (heroList.Count >= MAX_HEROES)
            return;

        // special case -- if we're Jethro, our player name may not be Jethro, but the prefab is
        if (index == 0)
            heroName = "Jethro";

        var heroObj = GameObject.Instantiate(Resources.Load("Prefabs/HeroesBattle/" + heroName + "Prefab")) as GameObject;
        heroObj.transform.SetParent(heroContainer);
        var hero = heroObj.GetComponent<Hero>();
        hero.Data = GameControl.control.heroList[index];

        var pos = heroStartingPositions[index];
        pos.y = 0;
        //pos.y = battlePlatform.transform.position.y;
        //var halfHeight = hero.spriteHolder.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        //print("half: " + halfHeight);
        //pos.y += halfHeight;
        hero.transform.position = pos;

        denigenList.Add(hero);
        heroList.Add(hero);

        // if they're dead, remove them from the start
        if (hero.IsDead)
            KillOff(hero);

        // after all creation
        hero.UpdateIcon();
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
        foreach(var enemy in GameControl.control.enemies)
        {
            CreateEnemy(enemy.denigenName);
        }

        // FOR TESTING -- Or for whatever reason the enemies list is empty
        if (GameControl.control.enemies.Count <= 0)
        {
            //int numOfGoikkos = 3;
            for (int i = 0; i < TEST_numOfEnemies; i++)
            {
                var random = Random.Range(0, TEST_listOfEnemies.Count);
                enemiesToAdd.Add(TEST_listOfEnemies[random]);
            }
                

            // call CreateEnemies on each enemy to add to create the enemies
            foreach (var enemy in enemiesToAdd)
                CreateEnemy(enemy);
        }
    }

    void CreateEnemy(string enemyName)
    {
        // if we're already at our max amount, break away (just in case)
        if (enemyList.Count >= MAX_ENEMIES)
            return;

        // create Enemy object
        var enemyObj = Instantiate(Resources.Load("Prefabs/EnemiesBattle/" + enemyName)) as GameObject;
        enemyObj.transform.SetParent(enemyContainer);
        var enemy = enemyObj.GetComponent<Enemy>();

        var enemyData = Instantiate(Resources.Load<EnemyData>("Data/Enemies/" + enemyName));

        enemy.Data = enemyData;
        enemy.Init();
        if (numOfEnemies < enemyStartingPositions.Count)
        {
            var pos = enemyStartingPositions[numOfEnemies];
            pos.y = 0;
            enemy.transform.localPosition = pos;
        }
        numOfEnemies++;
        denigenList.Add(enemy);
        enemyList.Add(enemy);
    }

    void AssignStatsCards()
    {
        statsCardManager = GetComponent<StatsCardManager>();

        // assign heroes their cards and set the stats
        for(int i = 0; i < heroList.Count; i++)
        {
            var hero = heroList[i];
            hero.statsCard = statsCardManager.HeroCards[i];
            hero.statsCard.SetInitStats(hero);
        }

        // turn off any unused cards
        for(int i = heroList.Count; i < statsCardManager.HeroCards.Count; i++)
        {
            statsCardManager.HeroCards[i].gameObject.SetActive(false);
        }

        // ENEMIES
        for(int i = 0; i < enemyList.Count; i++)
        {
            var enemy = enemyList[i];
            enemy.statsCard = statsCardManager.EnemyCards[i];
            enemy.statsCard.SetInitStats(enemy);
        }
        for(int i = enemyList.Count; i < statsCardManager.EnemyCards.Count; i++)
        {
            statsCardManager.EnemyCards[i].gameObject.SetActive(false);
        }

        // set stat cards positions
        statsCardManager.DetermineCardPositions(heroList, enemyList);

        CreateHPBars();
    }

    void CreateHPBars()
    {
        foreach (var denigen in denigenList)
        {
            var hpBar = GameObject.Instantiate(hpBarPrefab, denigen.transform);//FindObjectOfType<Canvas>().transform);            
            hpBar.GetComponent<MiniHP>().Init(denigen);
        }
    }

    public void SetDenigenPositionsToCards()
    {
        foreach (var denigen in denigenList)
        {
            var cardPos = Camera.main.ScreenToWorldPoint(denigen.statsCard.GetComponent<RectTransform>().position);
            cardPos.y = denigen.transform.position.y;
            cardPos.z = denigen.transform.position.z;
            denigen.transform.position = cardPos;
        }
    }

    void InitTurnOrder()
    {
        // add heroes first
        for(int i = 0; i < heroList.Count; i++)
        {
            turnOrder[i].Init(heroList[i]);
        }

        // add enemies next
        for(int i = 0; i < enemyList.Count; i++)
        {
            turnOrder[heroList.Count + i].Init(enemyList[i]);
        }

        // disable any leftover ui
        for(int i = (heroList.Count + enemyList.Count); i < turnOrder.Count; i++)
        {
            turnOrder[i].Disable();
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

        SortUITurnOrder();
    }

    void SortUITurnOrder()
    {
        // set it so that the first to go is the last in the siblings.
        for(int i = 0; i < denigenList.Count; i++)
        {
            for(int j = 0; j < turnOrder.Count; j++)
            {
                if (turnOrder[j].denigen == denigenList[i])
                {
                    if (denigenList[i].IsDead || denigenList[i].IsBlocking || (battleState == BattleState.ATTACK && string.IsNullOrEmpty(denigenList[i].CurrentAttackName)))
                        turnOrder[j].Disable();
                    else
                        turnOrder[j].SetAsFirst();

                }

            }
        }
    }

    TurnOrderUI GetTurnOrderUI(Denigen den)
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].denigen == den)
            {
                return turnOrder[i];
            }
        }
        return null;
    }

    void RemoveFromTurnOrder(Denigen deadDenigen)
    {
        // find the turn order denigen
        var currentSlot = GetTurnOrderUI(deadDenigen);

        if (currentSlot != null)
            currentSlot.Disable();
    }

    void HighlightArrowTurnOrder(Denigen currentHero, bool highlight)
    {
        // find the hero who's up next
        var heroSlot = GetTurnOrderUI(currentHero);

        if (heroSlot != null)
            heroSlot.ArrowHighlight(highlight);
    }

    public void HighlightStarburstTurnOrder(Denigen currentTarget, bool highlight)
    {
        var targetSlot = GetTurnOrderUI(currentTarget);

        if (targetSlot != null)
            targetSlot.StarburstHighlight(highlight);
    }

    public void TurnOffAllHighlightStarburstTurnOrder()
    {
        foreach(var t in turnOrder)
        {
            t.StarburstHighlight(false);
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
    
    void ShowBattleMenu()
    {
        //DescriptionText.text = "";
        uiManager.PushMenu(uiManager.uiDatabase.BattleMenu);
    }

    void ToggleDescription(bool show)
    {
        DescriptionObj.SetActive(show);
    }

    public void SetText(string message = "")
    {
        DescriptionText.text = message;
    }

    void ChangeBattleState(BattleState state)
    {
        // set the new battle state
        battleState = state;
        
        // reset current denigen to traverse through list during attacks
        currentDenigen = FindNextLivingIndex(0);

        // determine what happens now in this new state
        switch(battleState)
        {
            case BattleState.TARGET:
                StartTargetPhase();
                break;
            case BattleState.ATTACK:
                StartAttackPhase();
                break;
            case BattleState.VICTORY:
            case BattleState.FAILURE:
            case BattleState.FLEE:
                EndBattle();
                break;
        }
    }

    void StartTargetPhase()
    {
        statsCardManager.ShowCards();
        battleCamera.BackToStart();
        battleCamera.ZoomTarget();
        ResetDenigen();
        
        // make sure the battle has not ended and we still want TARGET
        // kinda ugly :p
        if (battleState != BattleState.TARGET) return;

        // otherwise, we're ready to actually start targeting
        ShowBattleMenu();
        ToggleDescription(true);
        SortBySpeed();
        ShowCurrentFullCard();

        HighlightArrowTurnOrder(heroList[currentDenigen], true);

    }

    void StartAttackPhase()
    {
        //ToggleDescription(false);
        statsCardManager.HideCards();

        // have enemies decide their attack
        foreach (var enemy in enemyList)
        {
            // standby -- waiting to be set (something so that it's not null
            enemy.CurrentAttackName = "Standby";
            //enemy.CurrentAttackName = enemy.ChooseAttack().Name;

            //// check if the enemy has enough PM. If not, use the default attack
            //if (enemy.NotEnoughPM())
            //    enemy.CurrentAttackName = enemy.defaultAttack.Name;
        }

        // resort in case there have been speed changes
        SortBySpeed();

        // make sure the first denigen to attack is alive
        FindNextAlive();

    }
    
    void FindNextAlive()
    {
        while (denigenList[currentDenigen].IsDead || string.IsNullOrEmpty(denigenList[currentDenigen].CurrentAttackName))
        {
            currentDenigen++;
            if (currentDenigen >= denigenList.Count)
            {
                // FOR NOW -- JUST GO BACK TO TARGETING
                //ChangeBattleState(BattleState.TARGET);
                EndAttackPhase();
                return;
            }
        }
        if (currentDenigen < denigenList.Count)
            print("NEXT UP -- " + denigenList[currentDenigen].name);
        AttackDenigen();
    }

    void EndAttackPhase()
    {
        // before we go back to targeting, check to see if any denigens have a status that requires them to lose health
        foreach (var d in denigenList)
        {
            var callback = d.CheckStatusHealthDamage();
            if (callback != null)
            {
                callback.Invoke();
                d.CalculatedDamage = d.StatusDamage;
                TakeDamage(d);
            }
        }

        ChangeBattleState(BattleState.TARGET);
    }

    public void KillOff(Denigen deadDenigen)
    {        
        deadDenigen.Die();        

        RemoveFromTurnOrder(deadDenigen);
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

        // hide old's starburst
        HighlightArrowTurnOrder(heroList[currentDenigen], false);

        // make sure that the next denigen in the list is living
        var nextDenigen = FindNextLivingIndex(currentDenigen + 1);
        
        if (nextDenigen < heroList.Count)
            NextTarget(nextDenigen);
        else
            GoToAttackState();
    }

    /// <summary>
    /// Sort through the hero list and return the next living hero
    /// </summary>
    /// <param name="startingIndex">Where to start searching. If starting Target, 0. If finding next, current + 1.</param>
    /// <returns></returns>
    int FindNextLivingIndex(int startingIndex)
    {
        var nextDenigen = startingIndex;
        while (nextDenigen < heroList.Count && heroList[nextDenigen].IsDead)
            nextDenigen++;

        return nextDenigen;
    }

    void NextTarget(int newIndex)
    {
        // hide old's starburst
        HighlightArrowTurnOrder(heroList[currentDenigen], false);
        
        currentDenigen = newIndex;
        ShowCurrentFullCard();

        // show new's starburst
        HighlightArrowTurnOrder(heroList[currentDenigen], true);

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
        denigen.ChooseTarget();
        denigen.Attack();
    }
    
    public void NextAttack()
    {
        // if the battle is over, break the cycle        
        if (IsBattleOver)
        {
            //EndBattle();
            return;
        }

        // hide current denigen's turn order UI to show their turn is over
        RemoveFromTurnOrder(denigenList[currentDenigen]);

        // increment up list -- if the next one is dead, continue to the next
        currentDenigen++;
        if (currentDenigen < denigenList.Count)
            FindNextAlive();
        // if we're at the end, end the phase
        else
            EndAttackPhase();
    }

    public IEnumerator ShowAttack(Denigen attacker, List<Denigen> targeted)
    {
        // first check if we have failed a flee. If we have, then skip all heroes
        //if (fleeFailed && attacker is Hero)
        //{
        //    //NextAttack();
        //    yield break;
        //}

        if(attacker.IsBlocking)
        {
            NextAttack();
            yield break;
        }

        // show attack
        DescriptionText.text = attacker.DenigenName + " uses " + attacker.CurrentAttackName;
        battleCamera.MoveTo(attacker.transform.position);
        battleCamera.ZoomAttack();

        // reduce power magic points at the moment of attack
        attacker.PayPowerMagic();

        // Update UI
        attacker.statsCard.UpdateStats();
        
        

        var anim = attacker.spriteHolder.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(attacker.AttackAnimation))
        {
            // time before and after the animation to give some time to watch the transitions to and from Idle
            var bufferTime = 0.25f;

            yield return new WaitForSeconds(bufferTime);
            //if (attacker is Hero)
                //Debug.Break();
                //Time.timeScale = 0.5f;
            //anim.Play(attacker.AttackAnimation);
            yield return attacker.PlayAttackAnimation();
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

        // we don't need to show any target info if there are no targets
        if (targeted.Count <= 0)
        {
            NextAttack();
            yield break;
        }
        
        // show targets being affected by attack/item
        battleCamera.BackToStart();
        battleCamera.ZoomTarget();

        // wait for camera to get back
        yield return new WaitForSeconds(0.25f);


        // check if the attacker is using an item to determine which function to perform
        if (attacker.UsingItem)
            yield return UseChosenItem(attacker, targeted);
        else
            yield return PerformAttack(attacker, targeted);

        
        NextAttack();
    }

    IEnumerator UseChosenItem(Denigen attacker, List<Denigen> targeted)
    {
        // show damage
        var messagesToDisplay = new List<string>();
        foreach (var target in targeted)
        {            
            var healedStatName = "";
            var healedStatValue = 0;

            if(target.healHP != 0)
            {
                healedStatValue = target.HealedByHPValue();
                target.Hp += target.healHP;
                target.LimitHP();                
                healedStatName = "HP";
            }

            else if(target.healPM != 0)
            {
                healedStatValue = target.HealedByPMValue();
                target.Pm += target.healPM;
                target.LimitPM();
                healedStatName = "PM";
            }

            ShowHealing(target, healedStatValue);
            var message = "";

            if (!string.IsNullOrEmpty(healedStatName))
                message = target.DenigenName + "'s " + healedStatName + " restored by " + healedStatValue;

            messagesToDisplay.Add(message);
            target.ResetHealing();
            target.statsCard.UpdateStats();

            // show hp bar
            target.hpBar.UpdateHP();
        }

        DisplayMultiMessage(messagesToDisplay);
        yield return new WaitForSeconds(1f);
        attacker.UsingItem = false;
    }

    IEnumerator PerformAttack(Denigen attacker, List<Denigen> targeted)
    {
        // show damage
        messagesToDisplay = new List<string>();

        foreach (var target in targeted)
        {
            // if the target has died between targeting and now, ignore him
            if (target.IsDead) continue;

            // alter hp based off of damage
            target.Hp -= target.CalculatedDamage;
            print("target calc: " + target.CalculatedDamage);

            // if we're healing, check to make sure we're not going over maxHp
            if (target.Hp > target.HpMax)
                target.Hp = target.HpMax;

            // if the attack only changes status effects, we can't block it
            if (attacker.attackType == Denigen.AttackType.BLOCKED && target.StatusChanged && target.CalculatedDamage <= 0)
                attacker.attackType = Denigen.AttackType.NORMAL;

            //Now record appropriate text
            var message = "";
            switch (attacker.attackType)
            {
                case Denigen.AttackType.NORMAL:
                    message = "";
                    target.Flinch();
                    break;
                case Denigen.AttackType.BLOCKED:
                    message = target.DenigenName + " blocked the attack\n";
                    StartCoroutine(target.PlayBlockAnimation());
                    break;
                case Denigen.AttackType.CRIT:
                    message = attacker.DenigenName + " hit a weak spot!\n";
                    target.Flinch();
                    break;
                case Denigen.AttackType.MISS:
                    message = attacker.DenigenName + " missed\n";
                    break;
                case Denigen.AttackType.DODGED:
                    message = target.DenigenName + " dodged the attack\n";
                    break;
            }


            message += target.DenigenName + " takes " + target.CalculatedDamage + " damage!";
            messagesToDisplay.Add(message);

            print(target.DenigenName + " takes " + target.CalculatedDamage + " damage!");

            TakeDamage(target);
        }

        DisplayMultiMessage(messagesToDisplay);
        yield return new WaitForSeconds(1f);
    }

    void TakeDamage(Denigen target)
    {
        // the status effect is the status was changed
        // show the Damage effect if damage was done
        // or show the heal effect if calcDamage is negative (meaning someone's using their turn to heal)
        if (target.StatusChanged)
            ShowStatusEffect(target);
        else if (target.CalculatedDamage >= 0)
            ShowDamage(target, target.CalculatedDamage);
        else
            ShowHealing(target, -target.CalculatedDamage);

        // show hp bar
        target.hpBar.UpdateHP();

        // check for dead
        print(target.DenigenName + " HP: " + target.Hp);
        if (target.Hp <= 0)
        {
            // check for overkill
            var overkillBoundary = -(target.HpMax * 0.3f);
            if (target.Hp < overkillBoundary)
                target.SetStatus(DenigenData.Status.overkill);
            else
                target.SetStatus(DenigenData.Status.dead);

            target.Hp = 0;
            print(target.DenigenName + " falls!");
            messagesToDisplay.Add(target.DenigenName + " falls!");
            KillOff(target);
        }

        target.statsCard.UpdateStats();
    }

    void ShowDamage(Denigen target, int damage)
    {
        GameObject be = (GameObject)Instantiate(Resources.Load("Prefabs/DamageEffect"), target.transform.position, Quaternion.identity);
        be.name = "DamageEffect";
        //be.GetComponent<Effect>().Start();
        be.GetComponent<SpriteRenderer>().sprite = damageIcon;
        be.GetComponent<Effect>().damage = damage.ToString();
    }

    void ShowHealing(Denigen target, int heal)
    {
        GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/DamageEffect"), target.transform.position, Quaternion.identity);
        be.name = "HealEffect";
        be.GetComponent<SpriteRenderer>().sprite = healIcon;
        be.GetComponent<Effect>().damage = heal.ToString();
    }

    void ShowStatusEffect(Denigen target)
    {
        GameObject be = null;
        if (target.GetComponentInChildren<Effect>())
            be = target.GetComponentInChildren<Effect>().gameObject;
        if (be == null)
        {
            be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/DamageEffect"), target.transform.position, Quaternion.identity);
            be.name = "StatusEffect";
        }

        // if the status is changed to normal, that's "normally" (lol) a good thing. So show the heart.
        // otherwise, show a status symbol
        if (target.StatusState == DenigenData.Status.normal)
            be.GetComponent<SpriteRenderer>().sprite = healIcon;
        else
            be.GetComponent<SpriteRenderer>().sprite = statusEffectIcon;

        be.GetComponent<Effect>().damage = "";

        target.UpdateIcon();
        
        target.StatusChanged = false;
    }

    void DisplayMultiMessage(List<string> messages)
    {
        if (messages.Count > 0)
        {
            DescriptionText.text = messages[0];
            for (int i = 1; i < messages.Count; i++)
            {
                DescriptionText.text += "\n" + messages[i];
            }
        }
    }

    void EndBattle()
    {
        if (battleState == BattleState.VICTORY)
            StartCoroutine(WinBattle());
        else if (battleState == BattleState.FAILURE)
            StartCoroutine(FailBattle());
        else if (battleState == BattleState.FLEE)
            StartCoroutine(FleeBattle());
            
    }

    /// <summary>
    /// Called after the end of the attack phase -- resets blocking to false for next round
    /// </summary>
    void ResetDenigen()
    {
        foreach(var denigen in denigenList)
        {
            // reset blocking
            if (denigen.IsBlocking)
                denigen.IsBlocking = false;

            denigen.CurrentAttackName = ""; // reset attack
            denigen.CalculatedDamage = 0; // reset damage taken
        }
    }

    public bool CalcFlee()
    {
        // TEMP FOR TESTING
        //return true;

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
        if (num < likelihood) return true;
        else return false;
    }

    public void StartFlee()
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
        DescriptionText.text = "Failed to flee";
        //fleeFailed = true;
        foreach (var hero in heroList)
            hero.CurrentAttackName = "";

        // remove heroes from turn order ui
        foreach(var turn in turnOrder)
        {
            if (turn.denigen is Hero)
                turn.Disable();
        }

        yield return new WaitForSeconds(1f);
        GoToAttackState();
    }

    IEnumerator WinBattle()
    {
        DescriptionText.text = "SUCCESS";

        // count winnings (gold) && experience
        var winnings = 0;
        var exp = 0;
        foreach(var e in enemyList)
        {
            winnings += e.Gold;
            exp += e.ExpGiven;
        }

        yield return new WaitForSeconds(2f);

        uiManager.PushMenu(uiManager.uiDatabase.VictoryMenu);
        var victoryMenu = uiManager.CurrentMenu.GetComponent<VictoryMenu>();
        victoryMenu.AddGold(winnings);
        victoryMenu.LevelUp(exp);
        
        //// add gold earnings
        //DescriptionText.text = "You gain " + winnings + " gold.";
        //GameControl.control.AddGold(winnings);
        //yield return new WaitForSeconds(2f);

        //// add exp
        //DescriptionText.text = "Your team members gain " + exp + " exp.";

        //foreach (var h in heroList)
        //{
        //    if (!h.IsDead)
        //        h.AddExp(exp);
        //}

        //yield return new WaitForSeconds(2f);

        //GameControl.control.ReturnFromBattle();
    }

    IEnumerator FailBattle()
    {
        DescriptionText.text = "FAILURE";
        // calculate loss gold
        var loss = (int)(GameControl.control.totalGold * 0.1f);
        yield return new WaitForSeconds(2f);

        DescriptionText.text = "You lose " + loss + " gold.";
        GameControl.control.AddGold(-loss);
        yield return new WaitForSeconds(2f);

        GameControl.control.LoadLastSavedStatue();
    }

    IEnumerator FleeBattle()
    {
        DescriptionText.text = "Flee successful";
        yield return new WaitForSeconds(2f);
        GameControl.control.ReturnFromBattle();
    }

    // STATS CARDS
    public void ShowCurrentFullCard()
    {
        ToggleDenigenStatCard(heroList[currentDenigen], true);
        //var card = heroList[currentDenigen].statsCard;
        //if(!card.gameObject.activeSelf)
        //    card.gameObject.SetActive(true);
        //card.ShowFullCard();
    }
    public void ShowCurrentShortCard()
    {
        ToggleDenigenStatCard(heroList[currentDenigen], false);
        //var card = heroList[currentDenigen].statsCard;
        //card.gameObject.SetActive(true);
        //card.ShowShortCard();
    }
    void ToggleAllCards(bool showFull)
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            //heroStatsList[i].gameObject.SetActive(show);
            ToggleDenigenStatCard(heroList[i], showFull);
        }
        for(int i = 0; i < enemyList.Count; i++)
        {
            ToggleDenigenStatCard(enemyList[i], showFull);
        }
    }
    void ToggleDenigenStatCard(Denigen denigen, bool show)
    {        
        if (show)
            denigen.statsCard.ShowFullCard();
        else
            denigen.statsCard.ShowShortCard();
    }

    public void ShowAllShortCards()
    {
        ToggleAllCards(showFull : false);
    }

    public void ShowAllShortCardsExceptCurrent()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            if (i == currentDenigen)
            {
                ToggleDenigenStatCard(heroList[i], true);
                continue;
            }

            ToggleDenigenStatCard(heroList[i], false);
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            ToggleDenigenStatCard(enemyList[i], false);
        }
    }

    // menu states
    public void SetMenuState(MenuState state)
    {
        menuState = state;
    }

}
// Target type
// for determining what kind and how many targets a denigen can affect depending on their chosen attack
public enum TargetType
{
    NULL,
    ENEMY_SINGLE,
    ENEMY_SPLASH,
    ENEMY_TEAM,
    HERO_SINGLE,
    HERO_SPLASH,
    HERO_TEAM,
    HERO_SELF
}

// Battle Menu state
// mainly to determine what "List Sub" should show
public enum MenuState
{
    SKILLS,
    SPELLS,
    ITEMS
}