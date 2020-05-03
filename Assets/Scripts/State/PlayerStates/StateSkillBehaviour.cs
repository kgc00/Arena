using Abilities;
using Controls;
using Enums;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

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

                (Ability, Vector3) activationData = (null, Vector3.zero);

                if (input.ActiveControl == ControllerType.Delta)
                    activationData = HandleSkillActivation(MouseHelper.GetWorldPosition(),
                        type);
                else if (input.ActiveControl == ControllerType.GamePad)
                    activationData = HandleSkillActivation(RotationHelper.GetUnitForward(Owner),
                        type);
                else
                    Debug.Log("updating for neither");

                if (activationData.Item1 == null) return false;

                
                unitState = new ActingUnitState(Owner, activationData.Item1, activationData.Item2);
                return true;
            }

            return false;
        }
        private (Ability, Vector3) HandleSkillActivation(Vector3 targetLocation, ButtonType buttonType)
        {
            Owner.AbilityComponent.equippedAbilities.TryGetValue(buttonType, out var ability);
            
            if (ability == null || ability.Cooldown.IsOnCooldown) return (null, Vector3.zero);

            return (ability, targetLocation);
        }
    }
}