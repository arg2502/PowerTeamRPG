using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsCard : MonoBehaviour {

    public Image background;
    Animator bgAnimator;
    public AnimationClip bgAnimation;
    Denigen currentDenigen;

    [Header("Full Group")]
    public GameObject fullGroup;
    public Image portrait;
    public Text denigenName;
    public Text level;
    public Text status;
    public Text hpCurrent;
    public Text hpMax;
    public Text pmCurrent;
    public Text pmMax;
    public Image hpBarFull;
    public Image pmBarFull;

    [Header("Short Group")]
    public GameObject shortGroup;
    public Image portraitShort;
    public Text hpShort;
    public Text pmShort;
    public Image hpBarShort;
    public Image pmBarShort;

    public enum CardState { SHORT, GROW, FULL, SHRINK }
    public CardState cardState;

    public float Center { get { return GetComponent<RectTransform>().position.x; } }
    public float FarLeft
    {
        get
        {         
            Vector3[] v = new Vector3[4];
            background.rectTransform.GetWorldCorners(v);
            return v[0].x;
        }
    }
    public float FarRight
    {
        get
        {
            Vector3[] v = new Vector3[4];
            background.rectTransform.GetWorldCorners(v);
            return v[3].x;
        }
    }

    void Awake()
    {
        bgAnimator = background.GetComponent<Animator>();
    }

    public void SetInitStats(Denigen denigen)
    {
        currentDenigen = denigen;
        UpdateStats();
        JumpToShort();
    }
    
    public void UpdateStats()
    {
        denigenName.text = currentDenigen.DenigenName;
        level.text = "Lvl " + currentDenigen.Level;

        // get status state -- but capitalize the first letter
        var statusText = currentDenigen.StatusState.ToString();
        statusText = char.ToUpper(statusText[0]) + statusText.Substring(1);
        status.text = statusText;

        if (currentDenigen is Hero)
        {
            hpCurrent.text = currentDenigen.Hp.ToString();
            hpMax.text = currentDenigen.HpMax.ToString();
            pmCurrent.text = currentDenigen.Pm.ToString();
            pmMax.text = currentDenigen.PmMax.ToString();

            hpShort.text = currentDenigen.Hp.ToString();
            pmShort.text = currentDenigen.Pm.ToString();
            portrait.sprite = currentDenigen.Portrait;
            portraitShort.sprite = currentDenigen.Portrait;
        }

        UpdateHealthBars();
        UpdatePowerMagicBars();
    }

    void UpdateHealthBars()
    {
        if (!gameObject.activeSelf) return;

        var healthPercent = currentDenigen.Hp / (float) currentDenigen.HpMax;
        StartCoroutine(ChangeBarValue(hpBarFull, healthPercent));
        StartCoroutine(ChangeBarValue(hpBarShort, healthPercent));
    }

    void UpdatePowerMagicBars()
    {
        if (!gameObject.activeSelf) return;

        var pmPercent = currentDenigen.Pm / (float) currentDenigen.PmMax;
        StartCoroutine(ChangeBarValue(pmBarFull, pmPercent));
        StartCoroutine(ChangeBarValue(pmBarShort, pmPercent));
    }

    IEnumerator ChangeBarValue(Image bar, float desiredAmount)
    {
        // decrease bar value
        if (bar.fillAmount > desiredAmount)
        {
            while (bar.fillAmount > desiredAmount)
            {
                bar.fillAmount -= Time.deltaTime;
                yield return null;
            }
        }
        // increase bar value
        else
        {
            while (bar.fillAmount < desiredAmount)
            {
                bar.fillAmount += Time.deltaTime;
                yield return null;
            }
        }
        bar.fillAmount = desiredAmount;
    }

    public void ShowFullCard()
    {
        //print("show full -- state: " + cardState);
        //// only play the animation if the full group is not already active
        //if (cardState == CardState.SHORT
        //    || cardState == CardState.SHRINK)
        //{
        //    print("inside full group is not active");
        //    StopAllCoroutines();
        //    StartCoroutine(ToFull());
        //}
    }
    IEnumerator ToFull()
    {
        shortGroup.SetActive(false);
        bgAnimator.speed = 1f;
        bgAnimator.Play("Grow", -1, 0);
        cardState = CardState.GROW;
        yield return new WaitForSeconds(0.2f); // CHANGE THIS
        fullGroup.SetActive(true);
        cardState = CardState.FULL;
        shortGroup.SetActive(false); // double check
    }
    
    public void ShowShortCard()
    {
        //print("SHRINK: " + gameObject.name);
        //// only play the animation if the short group is not already active
        //if (cardState == CardState.FULL
        //     || cardState == CardState.GROW)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(ToShort());
        //}
    }
    IEnumerator ToShort()
    {
        fullGroup.SetActive(false);
        bgAnimator.speed = 1f;
        bgAnimator.Play("Shrink", -1, 0);
        cardState = CardState.SHRINK;
        yield return new WaitForSeconds(0.2f); // CHANGE THIS
        shortGroup.SetActive(true);
        cardState = CardState.SHORT;
        fullGroup.SetActive(false); // double check
    }

    public void JumpToShort()
    {
        //fullGroup.SetActive(false);
        //shortGroup.SetActive(true);

        //if (bgAnimator == null) return;

        //bgAnimator.speed = 0;
        //bgAnimator.Play("Shrink", -1, 1);
    }

    public void SetBGSize(int numOfCards)
    {
        print("num: " + numOfCards);
        var delta = background.rectTransform.sizeDelta;

        if (numOfCards == 1)
            delta.x = 100;
        else if (numOfCards == 2)
            delta.x = 100;
        else if (numOfCards == 3)
            delta.x = 30;
        else if (numOfCards == 4)
            delta.x = 6;
        else if (numOfCards == 5)
            delta.x = -20;

        background.rectTransform.sizeDelta = delta;

    }
}
