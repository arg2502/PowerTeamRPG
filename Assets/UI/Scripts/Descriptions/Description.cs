namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class Description : MonoBehaviour
    {
        public bool noDescription;
        public string description;

        public string GetDescription()
        {
            if (string.IsNullOrEmpty(description))
                SetDescription();

            return description;
        }

        protected virtual void SetDescription() { description = ""; }
    }
}