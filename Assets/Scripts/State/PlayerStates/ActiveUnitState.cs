using System;
using System.Linq;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;
using static Utils.ControlTypeHelper;

namespace State.PlayerStates
{
    public class IdleUnitState : UnitState
    {
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private InputValues input = new InputValues();

        public IdleUnitState(Unit owner) : base(owner)
        {
            movementSpeed = owner.BaseStats.MovementSpeed.Value;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, false);
        }

        public override UnitState HandleUpdate()
        {
                // Debug.Log();
                // Owner.Animator.SetBool(Moving, true);
                var motion = GetMovementFromInput(input);

                UpdatePlayerPositionTr(motion);


                //
            // if (input.Look > 0 || input.Turn > 0)
            // {
            //     var motion = GetMovementFromInput(input);
            //     UpdatePlayerRotation(input, motion);
            // }
            //
            // Debug.Log(input.FirePhase);
            // if (input.Fire == 1 && input.FirePhase == InputActionPhase.Performed)
            // {
            //     HandleFire();
            // }

            return null;
        }

        public override void HandleOnMove(InputAction.CallbackContext context)
        {
            Debug.Log("called");
            input.Horizontal = context.ReadValue<Vector2> ().x;
            input.Forward = context.ReadValue<Vector2> ().y;
            // var motion = GetMovementFromInput(context);

            // UpdatePlayerPositionTr(motion);
        }
        
        public override void HandleOnLook(InputAction.CallbackContext context)
        {
            var motion = GetMovementFromInput(context);

            UpdatePlayerRotation(context, motion);
        }

        public override void HandleOnFire(InputAction.CallbackContext context)
        {
            Debug.Log(context.phase);
        }
        
        private void UpdatePlayerPositionTr(Vector3 motion)
        {
            // east = (1, 0)
            // west = (-1, 0)
            // north = (0, 1)
            // south = (0, -1)

            // TODO: reduce input for diagonal movement

            Owner.transform.position += motion;
        }
        private void UpdatePlayerRotation(InputAction.CallbackContext context, Vector3 motion)
        {
            if (GetControlType(context.action.activeControl.displayName) == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard();
            else if (GetControlType(context.action.activeControl.displayName) == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(context, motion);
            else
                Debug.Log("updating for neither");
        }

        private void UpdatePlayerRotationForKeyboard()
        {
            // Debug.Log("updating for keyboard");
            var mousePos = Utils.MouseHelper.GetWorldPosition();

            var transform = Owner.transform;
            var difference = mousePos - transform.position;
            Owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(difference), Time.deltaTime * 10f);
        }

        private void UpdatePlayerRotationForGamepad(InputAction.CallbackContext context, Vector3 motion)
        {
            // Debug.Log("updating for gamepad");

            var posX = context.ReadValue<Vector2> ().x * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = context.ReadValue<Vector2> ().x * movementSpeed * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, Quaternion.LookRotation(rotationVal),
                Time.deltaTime * 10f);
        }

        private Vector3 GetMovementFromInput(InputAction.CallbackContext context)
        {
            var posX = context.ReadValue<Vector2> ().x * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = context.ReadValue<Vector2> ().y * movementSpeed * Time.deltaTime;

            var motion = new Vector3(posX, posY, posZ);
            return motion;
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