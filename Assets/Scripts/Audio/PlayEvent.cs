using UnityEngine;
using System.Collections;

public class PlayEvent : MonoBehaviour
{
    public string eventName;

    void OnTriggerEnter2D(Collider2D other)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
}
