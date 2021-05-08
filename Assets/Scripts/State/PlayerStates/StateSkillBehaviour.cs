using Abilities;
using Controls;
using Data.Types;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace State.PlayerStates {
    public class StateSkillBehaviour {
        public Unit Owner { get; private set; }

        public StateSkillBehaviour(Unit owner) {
            Owner = owner;
        }

        public bool ShouldActivateSkill(InputValues input, bool quickCast, out UnitState unitState) {
            unitState = null;

            if (Owner.InputModifierComponent.InputModifier.HasFlag(InputModifier.CannotAct)) return false;
            
            foreach (var kvp in input.ButtonValues) {
                var press = kvp.Value;
                var type = kvp.Key;
                Debug.Log(press.HasReleasedPress);
                var notPressing = quickCast ? !press.HasReleasedPress : !press.HasPerformedPress;
                if (notPressing) continue;
                
                Vector3 targetLocation = Vector3.zero;
                switch (input.ActiveControl) {
                    case ControllerType.Delta:
                        targetLocation = MouseHelper.GetWorldPosition();
                        break;
                    case ControllerType.GamePad:
                        targetLocation = RotationHelper.GetUnitForward(Owner);
                        break;
                    default: 
                        Debug.Log("updating for neither");
                        break;
                }

                var intent = HandleSkillActivation(targetLocation, type);

                if (intent.ability == null) return false;

                unitState = new ActingUnitState(Owner, intent.ability, intent.targetLocation);
                return true;
            }

            return false;
        }

        private (Ability ability, Vector3 targetLocation) HandleSkillActivation(Vector3 targetLocation,
            ButtonType buttonType) {
            Owner.AbilityComponent.equippedAbilities.TryGetValue(buttonType, out var ability);

            if (ability == null || ability.Cooldown.IsOnCooldown) return (null, Vector3.zero);

            return (ability, targetLocation);
        }
    }
}