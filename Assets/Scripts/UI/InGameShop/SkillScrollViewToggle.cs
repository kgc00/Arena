using System.Collections.Generic;
using Abilities;
using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;
using Data.Modifiers;
using Data.Types;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop {
    public class SkillScrollViewToggle : MonoBehaviour {
        public AbilityData AbilityModel { get; private set; }
        public AbilityModifierShopData ModifierShopData { get; private set; }
        [SerializeField] private Toggle _toggle;

        public void HandleToggle(bool toggleValue) {
            if (toggleValue) {
                this.PostNotification(NotificationType.SkillScrollViewToggleToggledOn, 
                    new SkillScrollViewToggleEvent(AbilityModel, ModifierShopData));
            }
        }

        public SkillScrollViewToggle Initialize(AbilityData abilityModel,
            AbilityModifierType modifierShopDataType) {
            AbilityModel = abilityModel;
            ModifierShopData = Utils.AbilityFactory.AbilityModifierShopDataFromType(modifierShopDataType); 
            return this;
        }

    }
}