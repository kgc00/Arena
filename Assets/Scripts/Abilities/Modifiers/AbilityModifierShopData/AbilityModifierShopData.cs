using Data.Modifiers;
using UnityEngine;

namespace Abilities.Modifiers.AbilityModifierShopData {
    [CreateAssetMenu(fileName = "AbilityModifierShopData", menuName = "ScriptableObjects/AbilityModifierShopData", order = 0)]
    public class AbilityModifierShopData : ScriptableObject {
        public string Title;
        public string Description;
        public int Cost;
        public Sprite Image;
        public AbilityModifierType Type;
    }
}