using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DescriptionText : MonoBehaviour {

    public List<string> mainDesc;
    public List<string> attackDesc;

	// Use this for initialization
	void Start () {
        mainDesc = new List<string>();
        mainDesc.Add("Access to strike, spells, skills and summons.");
        mainDesc.Add("Command this hero to block, which reduces all damage recieved by half.");
        mainDesc.Add("Access items usable in battle, such as potions and healing items.");
        mainDesc.Add("Flee from battle.");
        attackDesc = new List<string>();
        attackDesc.Add("Attack that consumes no PM. Hits a single target with 30% power.");
        attackDesc.Add("Access all of this hero's learnt skills.");
        attackDesc.Add("Access all of this hero's learnt spells.");
        attackDesc.Add("Access all of this hero's summoning contracts.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
