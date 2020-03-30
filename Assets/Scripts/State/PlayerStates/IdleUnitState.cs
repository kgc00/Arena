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
    public class IdleUnitState : UnitState
    {
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");

        public IdleUnitState(Unit owner) : base(owner)
        {
            movementSpeed = 5f;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, false);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0) return new RunUnitState(Owner);

            if (input.Look > 0 || input.Turn > 0)
            {
                var motion = GetMovementFromInput(input);
                UpdatePlayerRotation(input, motion);
            }


            CheckShouldFire(input);

            return null;
        }

        private void CheckShouldFire(InputValues input)
        {
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

        private void UpdatePlayerRotation(InputValues input, Vector3 motion)
        {
            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion);
            else
                Debug.Log("updating for neither");
        }

        private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
        {
            // Debug.Log("updating for keyboard");
            var mousePos = MouseHelper.GetWorldPosition();

            var transform = Owner.transform;
            var difference = mousePos - transform.position;
            var difQuat = Quaternion.LookRotation(difference);
            var difEuler = difQuat.eulerAngles;
            var difEulerAdjusted = new Vector3(transform.rotation.x, difEuler.y, transform.rotation.z);
            var final = Quaternion.Euler(difEulerAdjusted);
            Owner.transform.rotation = final;
            // Quaternion.Slerp(transform.rotation,
            // final, Time.deltaTime);
        }

        private void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion)
        {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Look * movementSpeed * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, Quaternion.LookRotation(rotationVal),
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
    }
}