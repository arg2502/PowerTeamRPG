using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsCard : MonoBehaviour {

    public Image background;
    Animator bgAnimator;
    public AnimationClip bgAnimation;

    [Header("Full Group")]
    public GameObject fullGroup;
    public Image portrait;
    public Text denigenName;
    public Text level;
    public Text hpCurrent;
    public Text hpMax;
    public Text pmCurrent;
    public Text pmMax;

    [Header("Short Group")]
    public GameObject shortGroup;
    public Text hpShort;
    public Text pmShort;
    
    void Start()
    {
        bgAnimator = background.GetComponent<Animator>();
    }

    public void SetInitStats(Denigen denigen)
    {
        denigenName.text = denigen.DenigenName;
        level.text = "Lvl " + denigen.Level;
        hpCurrent.text = denigen.Hp.ToString();
        hpMax.text = denigen.HpMax.ToString();
        pmCurrent.text = denigen.Pm.ToString();
        pmMax.text = denigen.PmMax.ToString();

        hpShort.text = denigen.Hp.ToString();
        pmShort.text = denigen.Pm.ToString();
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void ShowFullCard()
    {
        StartCoroutine(ToFull());
    }
    IEnumerator ToFull()
    {
        shortGroup.SetActive(false);
        bgAnimator.Play("Grow", -1, 0);
        yield return new WaitForSeconds(bgAnimator.GetCurrentAnimatorClipInfo(0).Length); // CHANGE THIS WHEN UPDATING TO NEW UNITY
        fullGroup.SetActive(true);
    }
    
    public void ShowShortCard()
    {
        StartCoroutine(ToShort());
    }
    IEnumerator ToShort()
    {
        fullGroup.SetActive(false);
        bgAnimator.Play("Shrink", -1, 0);
        yield return new WaitForSeconds(bgAnimator.GetCurrentAnimatorClipInfo(0).Length); // CHANGE THIS WHEN UPDATING TO NEW UNITY
        shortGroup.SetActive(true);
    }
}
