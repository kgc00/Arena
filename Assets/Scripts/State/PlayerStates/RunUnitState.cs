using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace State.PlayerStates
{
    public class RunUnitState : UnitState
    {
        private Rigidbody body;
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private float threshold = 0.1f;

        public RunUnitState(Unit owner) : base(owner)
        {
            body = owner.Rigidbody;
            movementSpeed = 5 * owner.BaseStats.MovementSpeed.Value;
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            CheckShouldFire(input);
            
            var playerIsStationary = Math.Abs(input.Forward) <= threshold && Math.Abs(input.Horizontal) <= threshold;
            if (playerIsStationary) return new IdleUnitState(Owner);
        
            return null;
        }

        private void CheckShouldFire(InputValues input)
        {
            // Debug.Log($"input.HasStartedFire {input.HasStartedFire}");
            bool notFiring = input.Fire < 0.4f || !input.HasStartedFire;
            if (notFiring) return;
            
            if (input.ActiveControl == ControllerType.Delta)
                HandleFire(MouseHelper.GetWorldPosition());
            else if (input.ActiveControl == ControllerType.GamePad)
                HandleFire(RotationHelper.GetUnitForward(Owner));
            else
                Debug.Log("updating for neither");
        }

        private void HandleFire(Vector3 targetLocation)
        {
            var abil = Owner.AbilityComponent.equippedAbilities.FirstOrDefault(ability => ability is AttackAbility);
            if (abil != null) abil.Activate(targetLocation);
        }

        public override void HandleFixedUpdate(InputValues input)
        {
            var motion = GetMovementFromInput(input);

            UpdatePlayerRotation(input, motion);
            UpdatePlayerPositionTr(input, motion);
            Owner.Animator.SetBool(Moving, true);
        }

        private void UpdatePlayerRotation(InputValues input, Vector3 motion)
        {
            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion);
            else if (input.ActiveControl ==  ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion);
            else
                Debug.Log("updating for neither");
        }
    
        private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
        {
            // Debug.Log("updating for keyboard");
            var mousePos = Utils.MouseHelper.GetWorldPosition();
        
            var difference = mousePos - Owner.transform.position;
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }
    
    
        private void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion)
        {        
            // Debug.Log("updating for gamepad");
    
            var posX = input.Turn * movementSpeed * Time.deltaTime ;
            var posY = 0;
            var posZ = input.Look * movementSpeed * Time.deltaTime ;    
            var rotationVal = new Vector3(posX,posY,posZ);
        
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

        private void UpdatePlayerPositionTr(InputValues input, Vector3 motion)
        {
            // east = (1, 0)
            // west = (-1, 0)
            // north = (0, 1)
            // south = (0, -1)

            // TODO: reduce input for diagonal movement

            Owner.transform.position += motion;
        }
    }
}