using UnityEngine;
using System.Collections;

public class Drawbridge : OverworldObject {

    int height = 500;
    float initialPos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)-transform.position.y - 200;
        initialPos = transform.position.y;
    }
    public override void Activate()
    {
        StartCoroutine(LowerBridge());
    }
    public IEnumerator LowerBridge()
    {
        while (transform.position.y > initialPos - height)
        {
            transform.Translate(new Vector3(0.0f, -1.0f, 0.0f));
            yield return new WaitForSeconds(0.01f);

        }
    }
}
