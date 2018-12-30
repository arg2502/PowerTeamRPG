namespace UI
{
    using UnityEngine;
    using System.Collections;

    [CreateAssetMenu(fileName = "UIDatabase", menuName = "Database/UI Menus", order = 1)]
    public class UIDatabase : ScriptableObject
    {
        [Header("Overworld")]
        public GameObject DialogueMenu;
        public GameObject DialogueResponseMenu;
        public GameObject ShopkeeperBuyMenu;
        public GameObject ShopkeeperSellMenu;
        public GameObject ItemQuantityMenu;

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

        [Header("Stat Points")]
        public GameObject StatPointsMenu;

        [Header("Battle")]
        public GameObject BattleMenu;
        public GameObject AttackSub;
        public GameObject ListSub;
        public GameObject TargetMenu;
        public GameObject VictoryMenu;
    }
}