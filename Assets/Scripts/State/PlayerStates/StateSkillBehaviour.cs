using Abilities;
using Controls;
using Data;
using Data.Types;
using JetBrains.Annotations;
using UI.InGameShop;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace State.PlayerStates {
    public class StateSkillBehaviour {
        public Unit Owner { get; private set; }
        private bool isShopVisible;

        public StateSkillBehaviour(Unit owner) {
            Owner = owner;
            InGameShopManager.OnShopVisibilityToggled += HandleVisibilityToggled;
        }

        ~StateSkillBehaviour() {
            InGameShopManager.OnShopVisibilityToggled -= HandleVisibilityToggled;
        }

        private void HandleVisibilityToggled(bool isVisible, Unit purchasingUnit) {
            isShopVisible = isVisible;
        }

        public bool ShouldActivateSkill(InputValues input, out UnitState unitState) {
            unitState = null;

            if (Owner.InputModifierComponent.InputModifier.HasFlag(InputModifier.CannotAct)) return false;

            if (Owner.Controller.PreviousPress.HasValue) {
                var press = Owner.Controller.PreviousPress.Value.Value;
                var type = Owner.Controller.PreviousPress.Value.Key;

                if (press.HasPerformedRelease) {
                    var intent = FormIntent(input, type);

                    if (intent.ability == null) return false;

                    unitState = new ActingUnitState(Owner, intent.ability, intent.targetingData._location);
                    this.PostNotification(NotificationType.AbilityDidActivate, intent);
                    Owner.Controller.PreviousPress = null;
                    return true;
                }
            }
            else {
                foreach (var kvp in input.ButtonValues) {
                    var press = kvp.Value;
                    var type = kvp.Key;
                    
                    if (press.HasPerformedPress) {
                        var intent = FormIntent(input, type);

                        if (intent.ability == null) continue;

                        this.PostNotification(NotificationType.AbilityWillActivate, intent);
                        Owner.Controller.PreviousPress = kvp;
                    }
                }
            }

            return false;
        }

        private UnitIntent FormIntent(InputValues input, ButtonType type) {
            Vector3 targetLocation = Vector3.zero;
            switch (input.ActiveControl) {
                case ControllerType.Delta:
                    targetLocation = MouseHelper.GetWorldPosition();
                    break;
                case ControllerType.GamePad:
                    targetLocation = RotationHelper.GetUnitForward(Owner);
                    break;
                default:
                    break;
            }

            return HandleSkillActivation(targetLocation, type);
        }

        private UnitIntent HandleSkillActivation(Vector3 targetLocation, ButtonType buttonType) {
            Owner.AbilityComponent.equippedAbilitiesByButton.TryGetValue(buttonType, out var ability);

            if (ability == null || ability.Cooldown.IsOnCooldown || !ability.Unlocked)
                return new UnitIntent(null, new TargetingData(TargetingBehavior.CursorLocation, Vector3.zero), Owner);

            return new UnitIntent(ability, new TargetingData(TargetingBehavior.CursorLocation, targetLocation), Owner);
        }
    }
}