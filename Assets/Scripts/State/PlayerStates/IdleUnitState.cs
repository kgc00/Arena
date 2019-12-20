using System;
using System.Linq;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State.PlayerStates
{
    public class IdleUnitState : UnitState
    {
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");

        public IdleUnitState(Unit owner) : base(owner)
        {
            movementSpeed = owner.BaseStats.MovementSpeed.Value;
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

            Debug.Log(input.FirePhase);
            if (input.Fire == 1 && input.FirePhase == InputActionPhase.Performed)
            {
                HandleFire();
            }

            return null;
        }

        private void HandleFire()
        {
            // var abil = Owner.AbilityComponent.equippedAbilities.FirstOrDefault(ability => ability is ShootCrossbow);
            // Debug.Log(abil);
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
            var mousePos = Utils.MouseHelper.GetWorldPosition();

            var transform = Owner.transform;
            var difference = mousePos - transform.position;
            Owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(difference), Time.deltaTime * 10f);
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