using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;

namespace UI.InGameShop {
    public class SkillScrollViewToggleEvent {
        public readonly AbilityData AbilityModel;
        public readonly AbilityModifierShopData AbilityModifierShopData;
        public SkillScrollViewToggleEvent(AbilityData abilityModel, AbilityModifierShopData abilityModifierShopData) {
            AbilityModel = abilityModel;
            AbilityModifierShopData = abilityModifierShopData;
        }
    }
}