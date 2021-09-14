using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Data.Types;
using JetBrains.Annotations;
using UI.InGameShop;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace State.PlayerStates {
    public class PlayerState : UnitState {
        protected readonly float movementThreshold = 0.1f;
        protected readonly StateSkillBehaviour skillBehaviour;

        protected PlayerState(Unit owner) : base(owner) {
            skillBehaviour = new StateSkillBehaviour(owner);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (input.ButtonValues[ButtonType.ShopMenu].HasPerformedPress) {
                InGameShopManager.Instance.ToggleVisibility();
            }
            
            return null;
        }

        // need to look into handling input only in update and storing values onto a field for fixed update
        public override void HandleFixedUpdate(InputValues input) {
            var movementSpeed = Owner.StatsComponent.Stats.MovementSpeed;
            var motion = GetMovementFromInput(input, movementSpeed.Value);


            UpdatePlayerRotation(input, motion, movementSpeed.Value);
            UpdatePlayerPositionForce(input, motion, movementSpeed.Value);
            UpdateAnimations(motion); // must occur after rotation has been updated
        }

        private Vector3 ObtainLookTarget(InputValues input, Vector3 motion) {
            if (input.ActiveControl == ControllerType.Delta)
                return LookTargetForKeyboard(input, motion);
            // TODO else if (input.ActiveControl == ControllerType.GamePad) 
            return default;
        }

        private void UpdateAnimations(Vector3 motion) {
            var forward = Owner.transform.forward;
            var right = Owner.transform.right;
            var dotForward = Vector3.Dot(forward, motion);
            var dotRight = Vector3.Dot(right, motion);
            
            Owner.Animator.SetFloat("VerticalMotion", dotForward);
            Owner.Animator.SetFloat("HorizontalMotion", dotRight);
        }

        private void UpdatePlayerPositionForce(InputValues input, Vector3 motion, float movementSpeed) {
            if (Owner.InputModifierComponent.InputModifier.HasFlag(InputModifier.CannotMove)) {
                // i don't think we need this
                // Owner.Rigidbody.velocity = Vector3.zero;
                return;
            }

            // east = (1, 0)
            // west = (-1, 0)
            // north = (0, 1)
            // south = (0, -1)

            // TODO: reduce input for diagonal movement

            Owner.Rigidbody.AddForce(motion.normalized * movementSpeed);
        }

        private void UpdatePlayerRotation(InputValues input, Vector3 motion, float movementSpeed) {
            if (Owner.InputModifierComponent.InputModifier.HasFlag(InputModifier.CannotRotate)) return;


            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion, movementSpeed);
        }

        private Vector3 LookTargetForKeyboard(InputValues input, Vector3 motion) {
            // get mouse pos
            var mousePos = MouseHelper.GetWorldPosition();

            // lock y to unit's current y
            return new Vector3(mousePos.x, Owner.transform.position.y, mousePos.z);
        }

        private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion) {
            // get mouse pos
            var mousePos = MouseHelper.GetWorldPosition();

            // lock y to unit's current y
            var lookTarget = new Vector3(mousePos.x, Owner.transform.position.y, mousePos.z);

            Owner.transform.LookAt(lookTarget);
        }

        private void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion, float movementSpeed) {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Look * movementSpeed * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(rotationVal),
                Time.deltaTime * 10f);
        }

        private Vector3 GetMovementFromInput(InputValues input, float movementSpeed) {
            var posX = input.Horizontal * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Forward * movementSpeed * Time.deltaTime;

            var motion = new Vector3(posX, posY, posZ);
            return motion;
        }


#if UNITY_EDITOR
        public override void HandleDrawGizmos() {
            
        }
#endif
    }
}