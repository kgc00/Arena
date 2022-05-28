using System;
using Common;
using Controls;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace State.PlayerStates {
    public class PlayerState : UnitState {
        protected readonly float movementThreshold = 0.1f;
        protected readonly StateSkillBehaviour skillBehaviour;
        private PlayerController _ownerPlayerController;
        private Vector3 _lookTarget;

        protected PlayerState(Unit owner) : base(owner) {
            skillBehaviour = new StateSkillBehaviour(owner);
            _ownerPlayerController = Owner.Controller as PlayerController
                                     ?? throw new Exception("Cannot cast controller to player controller");
        }

        public override UnitState HandleUpdate(InputValues input) {
            var isVisibleOrDebug = !Constants.IsDebug && !Owner.InGameShopManager.isShopVisible;
            if (!input.ButtonValues[ButtonType.ShopMenu].HasPerformedPress) return null;
            if (!Owner.InGameShopManager.IsPurchasingUnitWithinProximity || isVisibleOrDebug) return null;

            Owner.InGameShopManager.ToggleVisibility();
            if (Owner.InGameShopManager.ShopUI.gameObject.activeInHierarchy) {
                _ownerPlayerController.InputValues.ResetButtonValues();
                Owner.Controller.PreviousPress = null;
                Owner.UIController.DisableTargetingUI();
                _ownerPlayerController.EnableUISchema();
            }
            else {
                _ownerPlayerController.EnablePlayerSchema();
            }

            return null;
        }

        // need to look into handling input only in update and storing values onto a field for fixed update
        public override void HandleFixedUpdate(InputValues input) {
            var movementSpeed = Owner.StatsComponent.Stats.MovementSpeed;
            var motion = GetMovementFromInput(input, movementSpeed.Value);


            if (input.ControlSchema != ControlSchemeEnum.Player) return;
            UpdatePlayerRotation(input, motion, movementSpeed.Value);
            UpdatePlayerPositionForce(input, motion, movementSpeed.Value);
            UpdateAnimations(motion); // must occur after rotation has been updated
            UpdateCameraPosition();
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
            _lookTarget = new Vector3(mousePos.x, Owner.transform.position.y, mousePos.z);

            Owner.transform.LookAt(_lookTarget);
        }

        // todo - make look target an explicit arg
        private void UpdateCameraPosition() {
            if (Owner._vcamFollowTarget == null) return;
            var position = Owner.transform.position;
            var heading = (_lookTarget - position) / 2;
            var cameraMaxFollowDistance = Owner._vcamFollowTarget.cameraMaxFollowDistance;
            var cameraX = Mathf.Clamp(heading.x + position.x, position.x - cameraMaxFollowDistance,
                position.x + cameraMaxFollowDistance);
            var cameraZ = Mathf.Clamp(heading.z + position.z, position.z - cameraMaxFollowDistance,
                position.z + cameraMaxFollowDistance);

            var shouldLerp = false;
            Owner._vcamFollowTarget.transform.position = shouldLerp
                ? Vector3.Lerp(Owner._vcamFollowTarget.transform.position,
                    new Vector3(cameraX, position.y, cameraZ), Time.fixedDeltaTime + 1f)
                : new Vector3(cameraX, position.y, cameraZ);
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

            // todo - assign vcam look target position
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
            // Gizmos.color = Color.green;
            // Gizmos.DrawSphere(_lookTarget, 1f);
        }
#endif
    }
}