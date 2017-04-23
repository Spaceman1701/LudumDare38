using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace src
{
    public class MultilineInputField : InputField
    {
        bool shouldMoveToEnd = false;
        bool shouldInsertBackup = false;
        string backupText;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            shouldMoveToEnd = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            shouldMoveToEnd = false;
        }

        private void Update()
        {
            
        }
        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (shouldMoveToEnd && EventSystem.current != null)
            {
                if (shouldInsertBackup)
                {
                    text = backupText;
                    shouldInsertBackup = false;
                }
                if (gameObject.Equals(EventSystem.current.currentSelectedGameObject))
                {
                    MoveTextEnd(false);
                    shouldMoveToEnd = false;
                }
            }

        }

        public void FixBadDesign(string backupText)
        {
            shouldMoveToEnd = true;
            shouldInsertBackup = true;
            this.backupText = backupText;
        }
 
    }
}
