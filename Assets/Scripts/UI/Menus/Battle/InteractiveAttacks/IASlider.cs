using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IASlider : InteractiveAttack {

    //public GameObject track;
    public GameObject slider;
    public List<GameObject> targets;
    public GameObject targetPrefab;
    float startTime;
    float journeyLength;
    public float speed;
    //float speed = 1000f;
    Vector2 startTrack;
    Vector2 endTrack;
    float perfectRange, greatRange, goodRange, okayRange, poorRange;
    int currentTarget = 0;
    Denigen currentAttacker;

    public void Init(Denigen attacker, List<float> damage, int numOfAttacks = 1) {
        base.Init(damage);
        CreatePositionsOnLine(numOfAttacks);

        currentAttacker = attacker;

        var startTrackX = slider.transform.localPosition.x;
        var endTrackX = Mathf.Abs(startTrackX);

        startTrack = new Vector2(startTrackX, 0f);
        endTrack = new Vector2(endTrackX, 0f);

        startTime = Time.time;
        journeyLength = Vector2.Distance(startTrack, endTrack);

        CreateTargets(numOfAttacks);
        
        var halfWidth = targets[0].GetComponent<RectTransform>().rect.width / 2f;
        perfectRange = halfWidth / 8f;
        greatRange = halfWidth / 4f;
        goodRange = halfWidth / 2f;
        okayRange = halfWidth / 1.5f;
        poorRange = halfWidth;

	}
	void CreateTargets(int numOfAttacks)
    {        
        targets = new List<GameObject>();
        for (int i = 0; i < numOfAttacks; i++)
        {
            var newTarget = Instantiate(targetPrefab, parentRectTransform.transform);
            
            // determine size of target based on accuracy of the attack
            float acc = currentAttacker.CurrentAttack.Accuaracy;
            newTarget.GetComponent<RectTransform>().sizeDelta *= acc/100f;

            newTarget.GetComponent<RectTransform>().localPosition = new Vector2(xPosList[i], 0f);
            targets.Add(newTarget);
        }
        slider.transform.SetAsLastSibling();
    }
	// Update is called once per frame
	new void Update () {        
        // if dazed
        //speed += Random.Range(-50, 50);
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        slider.transform.localPosition = Vector2.Lerp(startTrack, endTrack, fractionOfJourney);

        if(Input.GetButtonDown("Submit") && currentTarget < targets.Count)
        {
            var dist = GetDistance(targets[currentTarget]);
            var textObj = targets[currentTarget].GetComponentInChildren<Text>();
            
			if (dist <= perfectRange) { quality = Quality.PERFECT; SetAttack(targets[currentTarget], true); currentTarget++; }
			else if (dist <= greatRange) { quality = Quality.GREAT; SetAttack(targets[currentTarget], true); currentTarget++; }
			else if (dist <= goodRange) { quality = Quality.GOOD; SetAttack(targets[currentTarget], true); currentTarget++; }
			else if (dist <= okayRange) { quality = Quality.OKAY; SetAttack(targets[currentTarget], true); currentTarget++; }
			else if (dist <= poorRange) { quality = Quality.POOR; SetAttack(targets[currentTarget], true); currentTarget++; }

        }

        // past the target
        if(currentTarget < targets.Count - 1
            && slider.transform.localPosition.x > targets[currentTarget].transform.localPosition.x + poorRange)
        {
            currentTarget++;
        }

        // end of the line
        if (fractionOfJourney >= 1)
        {
            battleManager.NextAttack();
            Destroy(gameObject);
            currentTarget = 0;
        }
	}

    float GetDistance(GameObject target)
    {
        return Mathf.Abs(slider.transform.localPosition.x - target.transform.localPosition.x);
    }

    
}
