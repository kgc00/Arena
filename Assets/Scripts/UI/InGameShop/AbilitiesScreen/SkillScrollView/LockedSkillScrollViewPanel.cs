using System;
using Abilities;
using Data.Types;
using TMPro;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class LockedSkillScrollViewPanel : SkillScrollViewPanel {
        [SerializeField] private TextMeshProUGUI skillName;

        public void InspectAbility() {
            this.PostNotification(NotificationType.DidClickShopButton);
            this.PostNotification(NotificationType.LockedSkillInspected,
                new LockedSkillInspectedEvent(AssociatedAbility.Model));
        }
        
        public override void InspectAbility(bool isSilent = false) {
            // send some event
            if (!isSilent) {
                this.PostNotification(NotificationType.DidClickShopButton);
            }

            this.PostNotification(NotificationType.LockedSkillInspected,
                new LockedSkillInspectedEvent(AssociatedAbility.Model));
        }

        public override void UpdateSkillScrollViewPanel(Ability ability) {
            AssociatedAbility = ability;
            skillName.SetText(AssociatedAbility.DisplayName);
        }
    }
}