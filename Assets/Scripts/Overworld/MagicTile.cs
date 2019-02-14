using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTile : MonoBehaviour {

    SpriteRenderer sr;

    bool isOn = false;    
    bool isSolved = false;
    MagicTileTracker tracker;

    public bool IsOn { get { return isOn; } }
    public MagicTileTracker Tracker { get { return tracker; } set { tracker = value; } }
    

    void Start () {
        sr = GetComponent<SpriteRenderer>();
        UpdateSpriteState();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isSolved)
        {
            isOn = !isOn;
            tracker.CheckTiles();
            UpdateSpriteState();
        }
    }

    public void SolveTile()
    {
        isSolved = true;
        UpdateSpriteState();
    }

    private void UpdateSpriteState()
    {
        if (isSolved)
            sr.color = Color.green;

        else if (isOn)
            sr.color = Color.blue;

        else
            sr.color = Color.white;
    } 
}
