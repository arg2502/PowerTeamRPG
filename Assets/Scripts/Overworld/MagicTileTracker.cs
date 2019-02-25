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
        bool wasPuzzleSolved = true;

        foreach(var t in tiles)
        {
            if (!t.IsOn)
            {
                wasPuzzleSolved = false;
                break;
            }                
        }

        SolveTiles(wasPuzzleSolved);
    }

    void SolveTiles(bool wasPuzzleSolved)
    {
        foreach(var t in tiles)
        {
            t.SolveTile(wasPuzzleSolved);
        }
    }
}
