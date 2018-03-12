namespace UI
{
    using UnityEngine;
    using System.Collections;

    [CreateAssetMenu(fileName = "UIDatabase", menuName = "Database/UI Menus", order = 1)]
    public class UIDatabase : ScriptableObject
    {
        [Header("Misc.")]
        public GameObject ConfirmationMenu;

        [Header("Pause Menu")]
        public GameObject PauseMenu;
        public GameObject TeamInfoSub;
        public GameObject HeroInfoSub;
        public GameObject InventorySub;

        [Header("Inventory")]
        public GameObject InventoryMenu;
        public GameObject ConfirmUseMenu;
        public GameObject UseItemMenu;

        [Header("Skill Tree")]
        public GameObject SkillTreeMenu;

        [Header("Battle")]
        public GameObject BattleMenu;
        public GameObject AttackSub;
        public GameObject ListSub;
        public GameObject TargetMenu;
    }
}