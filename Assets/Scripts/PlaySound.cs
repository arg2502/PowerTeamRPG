using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class gets attached to sprites with animations 
/// that need to play a sound on a certain frame
/// </summary>
public class PlaySound : MonoBehaviour {

	public void Play(AudioClip clip)
    {
        GameControl.audioManager.PlaySFX(clip);
    }
}
