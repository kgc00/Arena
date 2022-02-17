using Abilities.Modifiers.AbilityModifierShopData;
using Data.AbilityData;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class SkillScrollViewToggleEvent {
        public readonly AbilityData AbilityModel;
        public readonly AbilityModifierShopData AbilityModifierShopData;
        public readonly bool IsPurchased;

        public SkillScrollViewToggleEvent(AbilityData abilityModel, AbilityModifierShopData abilityModifierShopData,
            bool isPurchased) {
            AbilityModel = abilityModel;
            AbilityModifierShopData = abilityModifierShopData;
            IsPurchased = isPurchased;
        }
    }
}