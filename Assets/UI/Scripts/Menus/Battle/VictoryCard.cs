using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCard : MonoBehaviour {

    public Image portrait;
    public Text title;
    public Text level;
    public Image expValue;
    public Text currentExp;
    public Text maxExp;
    Hero currentHero;

    float fillSpeed = 0.5f;

    bool isDone = false;
    public bool IsDone { get { return isDone; } }

    bool leveledUp = false;
    public bool LeveledUp { get { return leveledUp; } }

    public void Init(Hero hero)
    {
        currentHero = hero;
        portrait.sprite = currentHero.Portrait;
        title.text = currentHero.DenigenName;
        level.text = currentHero.Level.ToString();
        currentExp.text = currentHero.ExpCurLevel.ToString(); //currentHero.Exp.ToString();
        //maxExp.text = currentHero.ExpToLevelUp.ToString();
        //maxExp.text = currentHero.ExpCurLevelMax.ToString();
        maxExp.text = currentHero.Data.MaxExpOfLevel(currentHero.Level).ToString();
        leveledUp = false;
        UpdateBar();
    }

    void UpdateBar()
    {
        var currentNum = int.Parse(currentExp.text);
        var maxNum = int.Parse(maxExp.text);
        expValue.fillAmount = (float)currentNum / maxNum;
    }

    IEnumerator BarChange()
    {
        var end = int.Parse(currentExp.text);
        var desiredFill = (float)end / int.Parse(maxExp.text);

        while(expValue.fillAmount < desiredFill)
        {
            expValue.fillAmount += Time.deltaTime * fillSpeed;
            yield return null;
        }
        expValue.fillAmount = desiredFill;
    }

    public void LevelUp(int exp)
    {
        // don't level up if the hero is dead
        if(currentHero.IsDead)
        {
            isDone = true;
            return;
        }

        leveledUp = true;
        currentHero.AddExp(exp);
        StartCoroutine(IncreaseBar(exp));
    }

    IEnumerator IncreaseBar(int remainingExp)
    {
        isDone = false;

        var startExp = int.Parse(currentExp.text);

        for (int i = 0; i < remainingExp; i++)
        {
            startExp++;
            //if (currentHero.DenigenName == "Jethro")
            //    print("start: " + startExp);
            currentExp.text = startExp.ToString();

            yield return StartCoroutine(BarChange());

            // check if leveled up
            if (BarFull())
            {
                startExp = 0;
                currentExp.text = startExp.ToString();

                var levelNum = int.Parse(level.text);
                levelNum++;
                level.text = levelNum.ToString();

                maxExp.text = currentHero.Data.MaxExpOfLevel(levelNum).ToString();

                yield return StartCoroutine(BarChange());
            }
            
        }

        isDone = true;
    }

    bool BarFull()
    {
        if (expValue.fillAmount >= 1)
            return true;
        else return false;
    }
}
