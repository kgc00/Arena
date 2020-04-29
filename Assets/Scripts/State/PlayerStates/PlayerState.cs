using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using JetBrains.Annotations;
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
        protected StateSkillBehaviour skillBehaviour;

        public PlayerState(Unit owner) : base(owner)
        {
            movementSpeed = 5f;
            skillBehaviour = new StateSkillBehaviour(owner);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (input.Look > 0 || input.Turn > 0)
            {
                var motion = GetMovementFromInput(input);
                UpdatePlayerRotation(input, motion);
            }

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