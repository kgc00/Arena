using Abilities;
using Controls;
using Enums;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils;

namespace State.PlayerStates
{
    public class StateSkillBehaviour
    {
        public Unit Owner { get; private set; }

        public StateSkillBehaviour(Unit owner)
        {
            Owner = owner;
        }

        public bool ShouldActivateSkill([CanBeNull] InputValues input, out UnitState unitState)
        {
            unitState = null;
            
            foreach (var kvp in input.ButtonValues)
            {
                var buttonVal = kvp.Value;
                var type = kvp.Key;
                bool notFiring = buttonVal.PressValue < 0.4f || !buttonVal.HasStartedPress;
                if (notFiring) continue;

                Ability ability = null;

                if (input.ActiveControl == ControllerType.Delta)
                    ability = HandleSkillActivation(MouseHelper.GetWorldPosition(),
                        type);
                else if (input.ActiveControl == ControllerType.GamePad)
                    ability = HandleSkillActivation(RotationHelper.GetUnitForward(Owner),
                        type);
                else
                    Debug.Log("updating for neither");

                if (ability == null || ability.Cooldown.IsOnCooldown) return false;
                
                unitState = new ActingUnitState(Owner, ability);
                return true;
            }

            return false;
        }
        private Ability HandleSkillActivation(Vector3 targetLocation, ButtonType buttonType)
        {
            Owner.AbilityComponent.equippedAbilities.TryGetValue(buttonType, out var ability);
            
            if (ability == null) return null;
            ability.Activate(targetLocation);

            return ability;
        }
    }
}