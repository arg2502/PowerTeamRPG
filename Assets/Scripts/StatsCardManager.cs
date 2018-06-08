using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCardManager : MonoBehaviour {

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
    const float EDGE_BUFFER = 20f; // arbitrary
    const float MENU_BUFFER = 50f; // arbitrary

    float heroLeftBorder;
    float heroRightBorder;
    float heroLeftQuarter;
    float heroRightQuarter;

    float enemyLeftBorder;
    float enemyRightBorder;
    float enemyLeftQuarter;
    float enemyRightQuarter;

    BattleManager battleManager;

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
        CENTER_SCREEN = Screen.width / 2f;
        //HERO_CENTER = Screen.width / 4f;
        //ENEMY_CENTER = Screen.width / 4f * 3f;

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
        var heroes = heroList.Count;
        if (heroes == 1)
            OneHero();
        else if (heroes == 2)
            TwoHeroes();
        else if (heroes == 3)
            ThreeHeroes();
        else if (heroes == 4)
            FourHeroes();

        var enemies = enemyList.Count;
        //OneEnemy();

        if (enemies == 1)
            OneEnemy();
        else if (enemies == 2)
            TwoEnemies();
        else if (enemies == 3)
            ThreeEnemies();
        else if (enemies == 4)
            FourEnemies();
        else if (enemies == 5)
            FiveEnemies();

        CheckBorders();
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

        // reorder heroes from leftmost to rightmost 
        ReorderHeroes();
    }

    void TwoHeroes()
    {
        var pos1 = (HERO_CENTER + heroRightQuarter) / 2f;
        var pos2 = (HERO_CENTER + heroLeftQuarter) / 2f;

        SetHeroPosition(0, pos1);
        SetHeroPosition(1, pos2);

        // reorder heroes from leftmost to rightmost 
        ReorderHeroes();
    }

    void ThreeHeroes()
    {
        var pos1 = heroRightQuarter;
        var pos2 = heroLeftQuarter;
        var pos3 = HERO_CENTER;

        SetHeroPosition(0, pos1);
        SetHeroPosition(1, pos2);
        SetHeroPosition(2, pos3);

        // reorder heroes from leftmost to rightmost 
        ReorderHeroes();
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

        // reorder heroes from leftmost to rightmost 
        ReorderHeroes();
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

    void CheckBorders()
    {
        CheckHeroBorders();
        CheckEnemyBorders();
    }

    void CheckHeroBorders()
    {
        StartCoroutine(CheckLeftSide(HeroCards, heroLeftBorder, ActiveHeroes));
        StartCoroutine(CheckRightSide(HeroCards, heroRightBorder, ActiveHeroes));
    }

    void CheckEnemyBorders()
    {
        StartCoroutine(CheckLeftSide(EnemyCards, enemyLeftBorder, ActiveEnemies));
        StartCoroutine(CheckRightSide(EnemyCards, enemyRightBorder, ActiveEnemies));
    }

    IEnumerator CheckLeftSide(List<StatsCard> cards, float leftBorder, int activeNum)
    {
        // find first active hero
        var first = FindActiveCard(0, cards);

        // if we're past the border, move in
        if (first.FarLeft < leftBorder)
        {
            while (first.FarLeft < leftBorder)
            {
                var pos = first.GetComponent<RectTransform>().position;
                pos.x += 1f;
                first.GetComponent<RectTransform>().position = pos;

                if (activeNum > 3)
                {
                    var secondHero = FindActiveCard(1, cards);
                    var pos2 = secondHero.GetComponent<RectTransform>().position;
                    pos2.x += 0.5f;
                    secondHero.GetComponent<RectTransform>().position = pos2;
                }
                yield return null;
            }
        }
        // otherwise, we probably have room to stretch to the border
        else if (first.FarLeft > leftBorder)
        {
            while (first.FarLeft > leftBorder)
            {
                var pos = first.GetComponent<RectTransform>().position;
                pos.x -= 1f;
                first.GetComponent<RectTransform>().position = pos;

                if (activeNum > 3)
                {
                    var secondHero = FindActiveCard(1, cards);
                    var pos2 = secondHero.GetComponent<RectTransform>().position;
                    pos2.x -= 0.5f;
                    secondHero.GetComponent<RectTransform>().position = pos2;
                }
                yield return null;
            }
        }
    }

    IEnumerator CheckRightSide(List<StatsCard> cards, float rightBorder, int activeNum)
    {
        // find last active hero
        StatsCard last = FindActiveCard(cards.Count - 1, cards, false);

        // if we're past the border, move in
        if (last.FarRight > rightBorder)
        {
            while (last.FarRight > rightBorder)
            {
                var pos = last.GetComponent<RectTransform>().position;
                pos.x -= 1f;
                last.GetComponent<RectTransform>().position = pos;

                if (activeNum > 3)
                {
                    var secondHero = FindActiveCard(cards.Count - 2, cards, false);
                    var pos2 = secondHero.GetComponent<RectTransform>().position;
                    pos2.x -= 0.5f;
                    secondHero.GetComponent<RectTransform>().position = pos2;
                }
                yield return null;
            }
        }
        // otherwise, we probably have room to stretch to the border
        else if (last.FarRight < rightBorder)
        {
            while (last.FarRight < rightBorder)
            {
                var pos = last.GetComponent<RectTransform>().position;
                pos.x += 1f;
                last.GetComponent<RectTransform>().position = pos;

                if (activeNum > 3)
                {
                    var secondHero = FindActiveCard(cards.Count - 2, cards, false);
                    var pos2 = secondHero.GetComponent<RectTransform>().position;
                    pos2.x -= 0.5f;
                    secondHero.GetComponent<RectTransform>().position = pos2;
                }
                yield return null;
            }
        }
    }
}
