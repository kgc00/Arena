using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace State.PlayerStates
{
    public class PlayerState : UnitState
    {
        protected readonly float movementSpeed;
        protected readonly float movementThreshold = 0.1f;
        protected StateSkillBehaviour skillBehaviour;

        public PlayerState(Unit owner) : base(owner)
        {
            movementSpeed = 100f;
            skillBehaviour = new StateSkillBehaviour(owner);
        }

        public override UnitState HandleUpdate(InputValues input) => null;

        // need to look into handling input only in update and storing values onto a field for fixed update
        public override void HandleFixedUpdate(InputValues input)
        {
            var motion = GetMovementFromInput(input);

            UpdatePlayerRotation(input, motion);
            UpdatePlayerPositionForce(input, motion);
        }

        private void UpdatePlayerPositionForce(InputValues input, Vector3 motion)
        {
            if (Owner.inputModifierComponent.InputModifier.HasFlag(InputModifier.CannotMove)) {
                Owner.Rigidbody.velocity = Vector3.zero;
                return;
            }

            // east = (1, 0)
            // west = (-1, 0)
            // north = (0, 1)
            // south = (0, -1)

            // TODO: reduce input for diagonal movement
            
            Owner.Rigidbody.AddForce(motion.normalized * movementSpeed);
        }

        private void UpdatePlayerRotation(InputValues input, Vector3 motion)
        {
            if (Owner.inputModifierComponent.InputModifier.HasFlag(InputModifier.CannotRotate)) return;
            
            
            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion);
        }


        private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
        {
            // get mouse pos
            var mousePos = MouseHelper.GetWorldPosition();
            
            // lock y to unit's current y
            var lookTarget = new Vector3(mousePos.x, Owner.transform.position.y, mousePos.z);
            
            Owner.transform.LookAt(lookTarget);
        }

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
        private Vector3 lookDir = new Vector3(0,0,0);
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