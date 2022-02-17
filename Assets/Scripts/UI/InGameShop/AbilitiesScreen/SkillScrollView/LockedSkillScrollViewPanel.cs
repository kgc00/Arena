using System;
using Abilities;
using Data.Types;
using TMPro;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class LockedSkillScrollViewPanel : SkillScrollViewPanel {
        [SerializeField] private TextMeshProUGUI skillName;

        public override void InspectAbility() {
            // send some event
            this.PostNotification(NotificationType.DidClickShopButton);
            this.PostNotification(NotificationType.LockedSkillInspected,
                new LockedSkillInspectedEvent(AssociatedAbility.Model));
        }

        public override void UpdateSkillScrollViewPanel(Ability ability) {
            AssociatedAbility = ability;
            skillName.SetText(AssociatedAbility.DisplayName);
        }
    }
}