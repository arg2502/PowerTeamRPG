namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class HeroStatDescription : Description
    {
        public enum HeroStat { Jethro, Cole, Eleanor, Juliette }
        public HeroStat hero;
        DenigenData currentHero;

        private void Start()
        {
            SetDescription();
        }

        // override our GetDescription to reset the description text everytime we
        // ask for it, since it's possible to have changed
        public override string GetDescription()
        {
            SetDescription();
            return description;
        }

        public override void SetDescription(string message = "")
        {
            SetHero();
            if (currentHero == null) return;

            description = currentHero.denigenName +
                "\n\nLevel: " + currentHero.level +
                "\nStatus: " + currentHero.statusState +
                "\nHP: " + currentHero.hp + " / " + currentHero.hpMax +
                "\nPM: " + currentHero.pm + " / " + currentHero.pmMax +
                "\nAtk: " + currentHero.atk +
                "\nDef: " + currentHero.def +
                "\nMgk Atk: " + currentHero.mgkAtk +
                "\nMgk Def: " + currentHero.mgkDef +
                "\nLuck: " + currentHero.luck +
                "\nEvasion: " + currentHero.evasion +
                "\nSpeed: " + currentHero.spd;
        }

        void SetHero()
        {
            switch (hero)
            {
                case HeroStat.Jethro:
                    currentHero = GameControl.control.heroList[0];
                    break;
                case HeroStat.Cole:
                    if (GameControl.control.heroList.Count > 1)
                        currentHero = GameControl.control.heroList[1];
                    break;
                case HeroStat.Eleanor:
                    if (GameControl.control.heroList.Count > 2)
                        currentHero = GameControl.control.heroList[2];
                    break;
                case HeroStat.Juliette:
                    if (GameControl.control.heroList.Count > 3)
                        currentHero = GameControl.control.heroList[3];
                    break;

            }
        }


    }
}