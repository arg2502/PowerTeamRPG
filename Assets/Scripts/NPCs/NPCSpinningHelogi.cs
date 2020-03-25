using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NPCSpinningHelogi : StationaryNPCControl {

    public PlayableDirector director;

	public void Spin()
    {
        director.Play();
    }
}
