using Data.AbilityData;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public class LockedSkillInspectedEvent {
        public readonly AbilityData Model;

        public LockedSkillInspectedEvent(AbilityData associatedAbilityModel) {
            Model = associatedAbilityModel;
        }
    }
}