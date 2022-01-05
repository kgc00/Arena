using UnityEngine;

namespace Abilities.Modifiers.AbilityModifierShopData {
    public static class AbilityModifierShopDataExtensions {
        public static AbilityModifierShopData CreateInstance(this AbilityModifierShopData data) {
            var instance = ScriptableObject.CreateInstance<AbilityModifierShopData>();
            instance.Cost = data.Cost;
            instance.Description = data.Description;
            instance.Image = data.Image;
            instance.Title = data.Title;
            return instance;
        }
    }
}