namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class SubMenu : Menu
    {
        public override void Init()
        {
            descriptionText = FindParentDescription(transform.parent.gameObject);
            base.Init();
        }
        Text FindParentDescription(GameObject parent)
        {
            if (parent.GetComponent<Menu>() is SubMenu)
                return FindParentDescription(parent.transform.parent.gameObject);
            else
                return parent.GetComponentInChildren<Text>();
        }
    }
}
