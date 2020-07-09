using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Spell : Technique {
    public Spell() { }
    public Spell(string[] list, Sprite icon = null)
        :base(list, icon)
    {
    }
}
