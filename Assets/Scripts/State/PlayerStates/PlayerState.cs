using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Utils;

namespace State.PlayerStates
{
    public class PlayerState : UnitState
    {
        protected readonly float movementSpeed;
        protected readonly float movementThreshold = 0.1f;

        public PlayerState(Unit owner) : base(owner)
        {
            movementSpeed = 5f;
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (input.Look > 0 || input.Turn > 0)
            {
                var motion = GetMovementFromInput(input);
                UpdatePlayerRotation(input, motion);
            }

            CheckShouldActivateSkill(input);

            return null;
        }
        
        public override void HandleFixedUpdate(InputValues input)
        {
            var motion = GetMovementFromInput(input);

            UpdatePlayerRotation(input, motion);
            UpdatePlayerPositionTr(input, motion);
        }
        
        private void UpdatePlayerPositionTr(InputValues input, Vector3 motion)
        {
            // east = (1, 0)
            // west = (-1, 0)
            // north = (0, 1)
            // south = (0, -1)

            // TODO: reduce input for diagonal movement

            Owner.transform.position += motion;
        }

        private void CheckShouldActivateSkill(InputValues input)
        {
            foreach (var kvp in input.ButtonValues)
            {
                var buttonVal = kvp.Value;
                var type = kvp.Key;
                bool notFiring = buttonVal.PressValue < 0.4f || !buttonVal.HasStartedPress;
                if (notFiring) continue;
            
                if (input.ActiveControl == ControllerType.Delta)
                    HandleSkillActivation(MouseHelper.GetWorldPosition(), type);
                else if (input.ActiveControl == ControllerType.GamePad)
                    HandleSkillActivation(RotationHelper.GetUnitForward(Owner), type);
                else
                    Debug.Log("updating for neither");
            }
        }

        private void HandleSkillActivation(Vector3 targetLocation, ButtonType buttonType)
        {
            Owner.AbilityComponent.equippedAbilities.TryGetValue(buttonType, out var ability);
            
            if (ability == null) return;
            ability.Activate(targetLocation);
        }

        private void UpdatePlayerRotation(InputValues input, Vector3 motion)
        {
            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion);
        }

        private Vector3 lookDir = new Vector3(0,0,0);

        private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
        {
            // get mouse pos
            var mousePos = MouseHelper.GetWorldPosition();

            #region Vars
            var transform = Owner.transform;
            var position = transform.position;
            var rotation = transform.rotation;
            #endregion
            
            // 1.) Grab the normalized euler angles for the look direction
            // 2.) Clamp the values to use the existing x and z, only alter the y
            var difEuler = Quaternion.LookRotation((mousePos - position).normalized).eulerAngles;
            var difEulerAdjusted = new Vector3(rotation.x, difEuler.y, rotation.z);
            
            // for debugging
            // lookDir = difEulerAdjusted;
            
            // Set the player's rotation to the new angle
            var updatedRot = Quaternion.Euler(difEulerAdjusted);
            Owner.transform.rotation = updatedRot;
        }

        #region LegacyRotation
        // private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
        // {
        //     // non functional but almost works... player needs to be 'locked' to x and z and only
        //     // rotate on the y axis.  otherwise this is very compact code to acheive that effect.
        //     var mousePos = MouseHelper.GetWorldPosition();
        //
        //     var difference = mousePos - Owner.transform.position;
        //     Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
        //                                                 Quaternion.LookRotation(difference),
        //                                                 Time.deltaTime * 10f);
        // }
        #endregion

        private void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion)
        {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Look * movementSpeed * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, 
                                                        Quaternion.LookRotation(rotationVal),
                                                        Time.deltaTime * 10f);
        }

        private Vector3 GetMovementFromInput(InputValues input)
        {
            var posX = input.Horizontal * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Forward * movementSpeed * Time.deltaTime;

            var motion = new Vector3(posX, posY, posZ);
            return motion;
        }
        
#if UNITY_EDITOR
        public override void HandleDrawGizmos()
        {
            return;
            
            Gizmos.color = Color.red;
            Debug.Log(lookDir);
            Gizmos.DrawRay(
                Owner.gameObject.transform.position,
                lookDir
            );
        }
#endif
    }
}