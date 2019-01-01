namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class Description : MonoBehaviour
    {
        public bool noDescription;
        public Description parentButton;
        [TextArea]
        public string description;

        public string GetDescription()
        {
            //if (string.IsNullOrEmpty(description))
            if (parentButton)
                SetDescription(parentButton.GetDescription());
            //else
              //  SetDescription();
            //print("description: " + description);
            return description;
        }

        public virtual void SetDescription(string message = "") { description = message; }
    }
}