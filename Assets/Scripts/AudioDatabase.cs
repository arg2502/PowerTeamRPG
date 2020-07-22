using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Database/AudioDatabase", order = 1)]
public class AudioDatabase : ScriptableObject
{
    public List<AudioItem> battleMusic;

    public string[] GetBattleKeys()
    {
        List<string> keys = new List<string>();

        for(int i = 0; i < battleMusic.Count; i++)
        {
            keys.Add(battleMusic[i].key);
        }

        return keys.ToArray();
    }
}

[System.Serializable]
public class AudioItem
{
    public string key;
    public List<AudioClip> clips;
}
