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
            print("description: " + description);
            return description;
        }

        public virtual void SetDescription(string message = "") { print("set to: " + message); description = message; }
    }
}