using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsCard : MonoBehaviour {

    public Image background;
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
            fullGroup.GetComponent<RectTransform>().GetWorldCorners(v);
            return v[0].x;
        }
    }
    public float FarRight
    {
        get
        {
            Vector3[] v = new Vector3[4];
            fullGroup.GetComponent<RectTransform>().GetWorldCorners(v);
            return v[3].x;
        }
    }

    public void SetInitStats(Denigen denigen)
    {
        currentDenigen = denigen;
        UpdateStats();
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
            portraitShort.sprite = currentDenigen.Portrait;
        }

        UpdateHealthBars();
        UpdatePowerMagicBars();
    }

    void UpdateHealthBars()
    {
        var healthPercent = currentDenigen.Hp / (float) currentDenigen.HpMax;
        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeBarValueCoroutine(hpBarFull, healthPercent));
            StartCoroutine(ChangeBarValueCoroutine(hpBarShort, healthPercent));
        }
        else
        {
            ChangeBarValue(hpBarFull, healthPercent);
            ChangeBarValue(hpBarShort, healthPercent);
        }
    }

    void UpdatePowerMagicBars()
    {
        var pmPercent = currentDenigen.Pm / (float) currentDenigen.PmMax;
        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeBarValueCoroutine(pmBarFull, pmPercent));
            StartCoroutine(ChangeBarValueCoroutine(pmBarShort, pmPercent));
        }
        else
        {
            ChangeBarValue(pmBarFull, pmPercent);
            ChangeBarValue(pmBarShort, pmPercent);
        }
    }

    void ChangeBarValue(Image bar, float desiredAmount)
    {
        bar.fillAmount = desiredAmount;
    }

    IEnumerator ChangeBarValueCoroutine(Image bar, float desiredAmount)
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
    
    public void SetBGSize(int numOfCards)
    {
        var delta = fullGroup.GetComponent<RectTransform>().sizeDelta;
        if (numOfCards == 1)
            delta.x = 225;
        else if (numOfCards == 2)
            delta.x = 225;
        else if (numOfCards == 3)
            delta.x = 210;
        else if (numOfCards == 4)
            delta.x = 200;
        else if (numOfCards == 5)
            delta.x = 190;

        fullGroup.GetComponent<RectTransform>().sizeDelta = delta;

    }
}
