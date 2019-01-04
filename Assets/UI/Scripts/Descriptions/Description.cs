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
        
        public virtual string GetDescription()
        {
            // if this description does not have it's own text and is based
            // off a "parent" button, get that description instead
            if (parentButton)
                SetDescription(parentButton.GetDescription());
            return description;
        }

        public virtual void SetDescription(string message = "") { description = message; }
    }
}