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
        var healthPercent = currentDenigen.Hp / (float) currentDenigen.HpMax;
        StartCoroutine(ChangeBarValue(hpBarFull, healthPercent));
        StartCoroutine(ChangeBarValue(hpBarShort, healthPercent));
    }

    void UpdatePowerMagicBars()
    {
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
        // only play the animation if the full group is not already active
        //if (!fullGroup.activeSelf)
            StartCoroutine(ToFull());
    }
    IEnumerator ToFull()
    {
        shortGroup.SetActive(false);
        bgAnimator.speed = 1f;
        bgAnimator.Play("Grow", -1, 0);
        yield return new WaitForSeconds(0.2f); // CHANGE THIS
        fullGroup.SetActive(true);
        shortGroup.SetActive(false); // double check
    }
    
    public void ShowShortCard()
    {
        // only play the animation if the short group is not already active
        //if (!shortGroup.activeSelf)
            StartCoroutine(ToShort());
    }
    IEnumerator ToShort()
    {
        fullGroup.SetActive(false);
        bgAnimator.speed = 1f;
        bgAnimator.Play("Shrink", -1, 0);
        yield return new WaitForSeconds(0.2f); // CHANGE THIS
        shortGroup.SetActive(true);
        fullGroup.SetActive(false); // double check
    }

    public void JumpToShort()
    {
        fullGroup.SetActive(false);
        shortGroup.SetActive(true);

        bgAnimator.speed = 0;
        bgAnimator.Play("Shrink", -1, 1);
    }
}
