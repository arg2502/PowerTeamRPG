using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	public void Play(AudioClip clip)
    {
        AudioManager.instance.Play(clip);
    }
}
