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
                    int.TryParse(list[i], out cost);
                    break;

                case 4:
                    int.TryParse(list[i], out pm);
                    break;

                case 5:
                    int.TryParse(list[i], out damage);
                    break;

                case 6:
                    int.TryParse(list[i], out critical);
                    break;

                case 7:
                    int.TryParse(list[i], out accuracy);
                    break;

                case 8:
                    int.TryParse(list[i], out colPos);
                    break;

                case 9:
                    int.TryParse(list[i], out rowPos);
                    break;

                case 10:
                    int.TryParse(list[i], out level);
                    break;

            }
        }
    }
}
