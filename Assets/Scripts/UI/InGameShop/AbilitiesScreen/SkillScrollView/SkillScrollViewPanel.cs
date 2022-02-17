using Abilities;
using UnityEngine;

namespace UI.InGameShop.AbilitiesScreen.SkillScrollView {
    public abstract class SkillScrollViewPanel : MonoBehaviour {
        public Ability AssociatedAbility { get; protected set; }
        public abstract void InspectAbility();
        public abstract void UpdateSkillScrollViewPanel(Ability ability);
    }
}