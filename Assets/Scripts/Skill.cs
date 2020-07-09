using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Skill : Technique
{
    public Skill() {}
    public Skill(string[] list, Sprite icon = null)
        :base(list, icon)
    {
    }
}
