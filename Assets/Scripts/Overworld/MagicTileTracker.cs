using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTileTracker : MonoBehaviour {

    public List<MagicTile> tiles;
    
    private void Start()
    {
        foreach(var t in tiles)
        {
            t.Tracker = this;
        }
    }

    public void CheckTiles()
    {
        foreach(var t in tiles)
        {
            if (!t.IsOn)
                return;
        }

        SolveTiles();
    }

    void SolveTiles()
    {
        foreach(var t in tiles)
        {
            t.SolveTile();
        }
    }
}
