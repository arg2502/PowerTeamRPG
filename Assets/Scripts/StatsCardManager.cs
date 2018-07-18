using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCardManager : MonoBehaviour {

    public GameObject HeroStats;
    public GameObject EnemyStats;
    public List<StatsCard> HeroCards;
    public List<StatsCard> EnemyCards;

    /*  
      
    |   |   |   |   |
    |   |   |   |   |
    |   |   |   |   |
    |   |   |   |   |

    */
    float CENTER_SCREEN;
    float HERO_CENTER;
    float ENEMY_CENTER;
    float EDGE_BUFFER = 2f * Screen.width / 100f; // arbitrary
    float MENU_BUFFER = 8f * Screen.width / 100f; // arbitrary

    float heroLeftBorder;
    float heroRightBorder;
    float heroLeftQuarter;
    float heroRightQuarter;

    float enemyLeftBorder;
    float enemyRightBorder;
    float enemyLeftQuarter;
    float enemyRightQuarter;

    BattleManager battleManager;

    List<StatsCard> battleCardHolder; // stores the cards that were active before battle, so we only turn those back on after battle

    int ActiveHeroes
    {
        get
        {
            int num = 0;
            foreach(var card in HeroCards)
            {
                if (card.gameObject.activeSelf)
                    num++;
            }
            return num;
        }
    }
    int ActiveEnemies
    {
        get
        {
            int num = 0;
            foreach (var card in EnemyCards)
            {
                if (card.gameObject.activeSelf)
                    num++;
            }
            return num;
        }
    }

    void Awake()
    {
        print("Awake screen width: " + Screen.width);
        CENTER_SCREEN = Screen.width / 2f;
         
        heroLeftBorder = EDGE_BUFFER;
        heroRightBorder = CENTER_SCREEN - MENU_BUFFER;
        HERO_CENTER = (heroLeftBorder + heroRightBorder) / 2f;

        heroLeftQuarter = (HERO_CENTER + heroLeftBorder) / 2f;
        heroRightQuarter = (HERO_CENTER + heroRightBorder) / 2f;

        enemyLeftBorder = CENTER_SCREEN + MENU_BUFFER;
        enemyRightBorder = Screen.width - EDGE_BUFFER;
        ENEMY_CENTER = (enemyLeftBorder + enemyRightBorder) / 2f;

        enemyLeftQuarter = (ENEMY_CENTER + enemyLeftBorder) / 2f;
        enemyRightQuarter = (ENEMY_CENTER + enemyRightBorder) / 2f;

        battleManager = GetComponent<BattleManager>();
    }

    public void DetermineCardPositions(List<Hero> heroList, List<Enemy> enemyList)
    {
        print("CardPos screen width: " + Screen.width);
        var heroesCount = ActiveHeroes;
        if (heroesCount == 1)
            OneHero();
        else if (heroesCount == 2)
            TwoHeroes();
        else if (heroesCount == 3)
            ThreeHeroes();
        else if (heroesCount == 4)
            FourHeroes();
        
        // reorder heroes from leftmost to rightmost 
        ReorderHeroes();

        var enemyCount = ActiveEnemies;
        if (enemyCount == 1)
            OneEnemy();
        else if (enemyCount == 2)
            TwoEnemies();
        else if (enemyCount == 3)
            ThreeEnemies();
        else if (enemyCount == 4)
            FourEnemies();
        else if (enemyCount == 5)
            FiveEnemies();

        SetCardSizes();
        CheckBorders();

        battleManager.SetDenigenPositionsToCards();
    }

    void SetHeroPosition(int index, float newPos)
    {
        SetPosition(HeroCards, index, newPos);
    }

    void SetEnemyPosition(int index, float newPos)
    {
        SetPosition(EnemyCards, index, newPos);
    }

    void SetPosition(List<StatsCard> cardList, int index, float newPos)
    {
        var pos = cardList[index].GetComponent<RectTransform>().position;
        pos.x = newPos;
        cardList[index].GetComponent<RectTransform>().position = pos;
    }

    void TestOneHero()
    {
        // test everyone
        for (int i = 0; i < HeroCards.Count; i++)
        {
            var pos = HeroCards[i].GetComponent<RectTransform>().position;
            pos.x = HERO_CENTER;
            HeroCards[i].GetComponent<RectTransform>().position = pos;
        }
    }

    void OneHero()
    {
        var newPos = HERO_CENTER;
        SetHeroPosition(0, newPos);        
    }

    void TwoHeroes()
    {
        var pos1 = (HERO_CENTER + heroRightQuarter) / 2f;
        var pos2 = (HERO_CENTER + heroLeftQuarter) / 2f;

        SetHeroPosition(0, pos1);
        SetHeroPosition(1, pos2);
    }

    void ThreeHeroes()
    {
        var pos1 = heroRightQuarter;
        var pos2 = heroLeftQuarter;
        var pos3 = HERO_CENTER;

        SetHeroPosition(0, pos1);
        SetHeroPosition(1, pos2);
        SetHeroPosition(2, pos3);
    }

    void FourHeroes()
    {
        var pos1 = (HERO_CENTER + heroRightQuarter) / 2f;
        var pos2 = (heroLeftBorder + heroLeftQuarter) / 2f;
        var pos3 = (HERO_CENTER + heroLeftQuarter) / 2f;
        var pos4 = (heroRightBorder + heroRightQuarter) / 2f;

        SetHeroPosition(0, pos1);
        SetHeroPosition(1, pos2);
        SetHeroPosition(2, pos3);
        SetHeroPosition(3, pos4);
    }

    void ReorderHeroes()
    {
        StatsCard temp;
        for (int j = 0; j < HeroCards.Count; j++)
        {
            for (int i = 0; i < HeroCards.Count - 1; i++)
            {
                if (HeroCards[i].Center > HeroCards[i + 1].Center)
                {
                    temp = HeroCards[i + 1];
                    HeroCards[i + 1] = HeroCards[i];
                    HeroCards[i] = temp;
                }
            }
        }
    }

    void OneEnemy()
    {
        var newPos = ENEMY_CENTER;
        SetEnemyPosition(0, newPos);
    }
    void TwoEnemies()
    {
        var pos1 = (ENEMY_CENTER + enemyLeftQuarter) / 2f;
        var pos2 = (ENEMY_CENTER + enemyRightQuarter) / 2f;

        SetEnemyPosition(0, pos1);
        SetEnemyPosition(1, pos2);
    }
    void ThreeEnemies()
    {
        var pos1 = enemyLeftQuarter;
        var pos2 = ENEMY_CENTER;
        var pos3 = enemyRightQuarter;

        SetEnemyPosition(0, pos1);
        SetEnemyPosition(1, pos2);
        SetEnemyPosition(2, pos3);
    }
    void FourEnemies()
    {
        var pos1 = (enemyLeftBorder + enemyLeftQuarter) / 2f;
        var pos2 = (ENEMY_CENTER + enemyLeftQuarter) / 2f;
        var pos3 = (ENEMY_CENTER + enemyRightQuarter) / 2f;
        var pos4 = (enemyRightBorder + enemyRightQuarter) / 2f;

        SetEnemyPosition(0, pos1);
        SetEnemyPosition(1, pos2);
        SetEnemyPosition(2, pos3);
        SetEnemyPosition(3, pos4);
    }
    void FiveEnemies()
    {
        var pos1 = enemyLeftBorder;
        var pos2 = enemyLeftQuarter;
        var pos3 = ENEMY_CENTER;
        var pos4 = enemyRightQuarter;
        var pos5 = enemyRightBorder;

        SetEnemyPosition(0, pos1);
        SetEnemyPosition(1, pos2);
        SetEnemyPosition(2, pos3);
        SetEnemyPosition(3, pos4);
        SetEnemyPosition(4, pos5);
    }

    StatsCard FindActiveCard(int startingIndex, List<StatsCard> cards, bool goingUp = true)
    {
        StatsCard card = null;
        if (goingUp)
        {
            for (int i = startingIndex; i < cards.Count; i++)
            {
                if (cards[i].gameObject.activeSelf)
                {
                    card = cards[i];
                    break;
                }
            }
        }
        else
        {
            for(int i = startingIndex; i >= 0; i--)
            {
                if (cards[i].gameObject.activeSelf)
                {
                    card = cards[i];
                    break;
                }
            }
        }
        return card;
    }

    void SetCardSizes()
    {
        foreach(var hero in HeroCards)
        {
            hero.SetBGSize(ActiveHeroes);
        }
        foreach(var enemy in EnemyCards)
        {
            enemy.SetBGSize(ActiveEnemies);
        }
    }

    void CheckBorders()
    {
        CheckHeroBorders();
        CheckEnemyBorders();
        SetMiddleCards(HeroCards, ActiveHeroes);
        SetMiddleCards(EnemyCards, ActiveEnemies);
    }

    void CheckHeroBorders()
    {
        CheckLeftSide(HeroCards, heroLeftBorder, ActiveHeroes);
        CheckRightSide(HeroCards, heroRightBorder, ActiveHeroes);
    }

    void CheckEnemyBorders()
    {
        CheckLeftSide(EnemyCards, enemyLeftBorder, ActiveEnemies);
        CheckRightSide(EnemyCards, enemyRightBorder, ActiveEnemies);
    }

    void CheckLeftSide(List<StatsCard> cards, float leftBorder, int activeNum)
    {
        // find first active hero
        var first = FindActiveCard(0, cards);

        // find distance between far point and border
        var distance = leftBorder - first.FarLeft;
        
        var pos = first.GetComponent<RectTransform>().position;
        pos.x += distance;
        first.GetComponent<RectTransform>().position = pos;

        //if (activeNum > 3)
        //{
        //    var secondHero = FindActiveCard(1, cards);
        //    var pos2 = secondHero.GetComponent<RectTransform>().position;

        //    // half the distance of first card
        //    var half = distance / 2f;
        //    pos2.x += half;

        //    secondHero.GetComponent<RectTransform>().position = pos2;
        //}
    }

    void CheckRightSide(List<StatsCard> cards, float rightBorder, int activeNum)
    {
        // find last active hero
        StatsCard last = FindActiveCard(cards.Count - 1, cards, false);

        // var distance = last.FarRight - rightBorder;
        var distance = rightBorder - last.FarRight;

        var pos = last.GetComponent<RectTransform>().position;
                pos.x += distance;
                last.GetComponent<RectTransform>().position = pos;        
    }

    StatsCard FindLastCard(List<StatsCard> cards)
    {
        for(int i = cards.Count - 1; i >= 0; i--)
        {
            if (cards[i].gameObject.activeSelf)
                return cards[i];
        }
        return null;
    }

    void SetMiddleCards(List<StatsCard> cards, int activeNum)
    {
        if (activeNum <= 3) return;

        var card1 = cards[0].GetComponent<RectTransform>().position.x;
        var lastActiveCard = FindLastCard(cards);
        var lastCard = lastActiveCard.GetComponent<RectTransform>().position.x;
        var distance = lastCard - card1;

        if (activeNum == 4)
        {
            var card2 = card1 + distance / 3f;
            var card3 = card2 + distance / 3f;

            SetPosition(cards, 1, card2);
            SetPosition(cards, 2, card3);
        }
        else if (activeNum == 5)
        {
            var card2 = card1 + distance / 4f;
            var card3 = card1 + distance / 2f;
            var card4 = lastCard - distance / 4f;

            SetPosition(cards, 1, card2);
            SetPosition(cards, 2, card3);
            SetPosition(cards, 3, card4);
        }
    }

    public void HideCards()
    {
        HeroStats.GetComponent<Animator>().Play("TurnOff");
        EnemyStats.GetComponent<Animator>().Play("TurnOff");       
    }

    public void ShowCards()
    {
        HeroStats.GetComponent<Animator>().Play("TurnOn");
        EnemyStats.GetComponent<Animator>().Play("TurnOn");
    }

}
