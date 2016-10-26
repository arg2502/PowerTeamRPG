using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DescriptionText : MonoBehaviour {

    public List<string> mainDesc = new List<string>();   
    public List<string> attackDesc = new List<string>();

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 9899;
    }
}
