using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniHP : MonoBehaviour {

    Denigen currentDenigen;
    public Text hpValue;
    public Text hpMax;
    public Image hpBar;

    private float fadeRate = 10f;

	public void Init(Denigen denigen)
    {
        currentDenigen = denigen;

        currentDenigen.hpBar = this;
        var hp = currentDenigen.Hp;
        var max = currentDenigen.HpMax;
        hpValue.text = hp.ToString();
        hpMax.text = max.ToString();
        hpBar.fillAmount = (float) hp / max;
        SetPosition();
        TurnOffInstant();
    }

    public void UpdateHP()
    {
        // if there hasn't been a change in HP, don't even do anything
        var prevHp = int.Parse(hpValue.text);
        if (prevHp - currentDenigen.Hp == 0)
            return;

        TurnOn();

        // update text
        hpMax.text = currentDenigen.HpMax.ToString();
        hpValue.text = currentDenigen.Hp.ToString();

        var percentage = (float)currentDenigen.Hp / currentDenigen.HpMax;

        // cap the min/max values
        if (percentage <= 0) percentage = 0;
        if (percentage >= 1) percentage = 1;

        // show the update
        StartCoroutine(ChangeBar(percentage));
    }

    IEnumerator ChangeBar(float desiredAmount)
    {
        // decrease bar value
        if (hpBar.fillAmount > desiredAmount)
        {
            while (hpBar.fillAmount > desiredAmount)
            {
                hpBar.fillAmount -= Time.deltaTime;
                yield return null;
            }
        }
        // increase bar value
        else
        {
            while (hpBar.fillAmount < desiredAmount)
            {
                hpBar.fillAmount += Time.deltaTime;
                yield return null;
            }
        }
        hpBar.fillAmount = desiredAmount;
        print("DONE");
        yield return new WaitForSeconds(1f);
        TurnOff();
    }

    //void Update()
    //{
    //    SetPosition();
    //}

    void SetPosition()
    {
        //var pos = Camera.main.WorldToScreenPoint(currentDenigen.transform.position);
        var pos = currentDenigen.transform.position;
        pos.y = currentDenigen.spriteHolder.GetComponent<SpriteRenderer>().bounds.min.y * 1.25f;
        GetComponent<RectTransform>().position = pos;
    }

    void TurnOn()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeRate;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    void TurnOff()
    {
        StartCoroutine(FadeOut());
    }
    void TurnOffInstant()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
    }
    IEnumerator FadeOut()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeRate;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
