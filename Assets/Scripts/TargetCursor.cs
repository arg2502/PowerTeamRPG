using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetCursor : MonoBehaviour {

    public List<Sprite> sprites;
    public bool isSplash;

    public void ChangeSprite()
    {
        if (isSplash) { gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1]; }
        if (!isSplash) { gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0]; }
    }
}
