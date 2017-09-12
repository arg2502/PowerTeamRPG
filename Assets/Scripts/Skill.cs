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
    public Skill(string[] list)
        :base(list)
    {
        for (int i = 1; i < list.Length; i++)
        {
            switch (i)
            {
                case 1:
                    name = list[i];
                    break;

                case 2:
                    description = list[i];
                    break;

                case 3:
                    cost = int.Parse(list[i]);
                    break;

                case 4:
                    pm = int.Parse(list[i]);
                    break;

                case 5:
                    damage = int.Parse(list[i]);
                    break;

                case 6:
                    critical = int.Parse(list[i]);
                    break;

                case 7:
                    accuracy = int.Parse(list[i]);
                    break;

                case 8:
                    colPos = int.Parse(list[i]);
                    break;

                case 9:
                    rowPos = int.Parse(list[i]);
                    break;

                case 10:
                    int.TryParse(list[i], out level);
                    break;

            }
        }
    }
}
