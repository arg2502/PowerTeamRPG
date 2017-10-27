namespace UI
{
    using UnityEngine;
    using System.Collections;

    [CreateAssetMenu(fileName = "UIDatabase", menuName = "UI/Database", order = 1)]
    public class UIDatabase : ScriptableObject
    {
        public GameObject PauseMenu;
        public GameObject TeamInfoSub;
        public GameObject HeroInfoSub;
        public GameObject InventorySub;
    }
}