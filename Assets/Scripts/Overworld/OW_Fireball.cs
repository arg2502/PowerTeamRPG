using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_Fireball : MonoBehaviour {

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<OW_Flammable>())
        {
            collision.GetComponent<OW_Flammable>().Ignite();
        }
    }

}
