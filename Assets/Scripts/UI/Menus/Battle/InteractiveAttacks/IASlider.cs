using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASlider : InteractiveAttack {

    public GameObject track;
    public GameObject slider;
    public List<GameObject> targets;
    float startTime;
    float journeyLength;
    public float speed;
    //float speed = 1000f;
    Vector2 startTrack;
    Vector2 endTrack;
    float perfectRange, greatRange, goodRange, okayRange, poorRange;

    // START() IS ONLY TEMP RIGHT NOW FOR TESTING
    private void Start()
    {
        Init();
    }

    void Init () {
        var startTrackX = slider.transform.localPosition.x;
        var endTrackX = Mathf.Abs(startTrackX);

        startTrack = new Vector2(startTrackX, 0f);
        endTrack = new Vector2(endTrackX, 0f);

        startTime = Time.time;
        journeyLength = Vector2.Distance(startTrack, endTrack);

        // temp
        targets[0].transform.position = track.transform.position;
        var width = targets[0].GetComponent<RectTransform>().rect.width;
        perfectRange = width / 10f;
        greatRange = width / 4f;
        goodRange = width / 2f;
        okayRange = width / 1.5f;
        poorRange = width;

	}
	
	// Update is called once per frame
	new void Update () {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        slider.transform.localPosition = Vector2.Lerp(startTrack, endTrack, fractionOfJourney);

        if(Input.GetButtonDown("Submit"))
        {
            var dist = GetDistance(targets[0]);
            if (dist <= perfectRange) { print("Perfect!"); Attack(Quality.PERFECT, true); }
            else if (dist <= greatRange) { print("Great!"); Attack(Quality.GREAT, true); }
            else if (dist <= goodRange) { print("Good!"); Attack(Quality.GOOD, true); }
            else if (dist <= okayRange) { print("Okay"); Attack(Quality.OKAY, true); }
            else if (dist <= poorRange) { print("Poor.."); Attack(Quality.POOR, true); }
        }
        if (fractionOfJourney >= 1)
        {
            battleManager.NextAttack();
            Destroy(gameObject);
        }
	}

    float GetDistance(GameObject target)
    {
        return Mathf.Abs(slider.transform.localPosition.x - target.transform.localPosition.x);
    }
}
