using Controls;
using Enums;
using State;
using State.AiStates;
using Units;
using UnityEngine;

namespace Utils
{
    public static class MouseHelper
    {
        //Method 4
        private static Plane plane = new Plane(Vector3.up, 0f);

        private static readonly Camera cam = Camera.main;

        public static Vector3 GetWorldPosition()
        {
            if (cam != null)
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out var distanceToPlane)) return ray.GetPoint(distanceToPlane);
            }

            return Vector3.zero;
        }
    }

    public static class RotationHelper
    {
        public static void UpdatePlayerRotation(InputValues input, Unit owner, Stat movementSpeed)
        {
            var motion = GetMovementFromInput(input, movementSpeed);

            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion, owner);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion, owner, movementSpeed);
            else
                Debug.Log("updating for neither");
        }

        private static void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion, Unit owner,
            Stat movementSpeed)
        {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed.Value * Time.deltaTime;
            var posY = 0;
            var posZ = input.Look * movementSpeed.Value * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(rotationVal),
                Time.deltaTime * 10f);
        }

        private static void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion, Unit owner)
        {
            // Debug.Log("updating for keyboard");
            var mousePos = MouseHelper.GetWorldPosition();

            var transform = owner.transform;

            var difference = mousePos - transform.position;
            owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }

        public static Vector3 GetMovementFromInput(InputValues input, Stat movementSpeedStat)
        {
            var movementSpeed = movementSpeedStat.Value;
            var posX = input.Horizontal * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Forward * movementSpeed * Time.deltaTime;

            var motion = new Vector3(posX, posY, posZ);
            return motion;
        }
    }

    public static class StateHelper
    {
        public static UnitState StateFromEnum(UnitStateEnum stateEnum, Unit owner)
        {
            switch (stateEnum)
            {
                case UnitStateEnum.AiIdle:
                    return new IdleUnitState(owner);
                case UnitStateEnum.PlayerActive:
                    return new State.PlayerStates.IdleUnitState(owner);
                default:
                    return null;
            }
        }
    }

    public static class ControlTypeHelper
    {
        public static ControllerType GetControlType(string displayName)
        {
            if (displayName == "Delta")
                return ControllerType.Delta;
            if (displayName == "Right Stick")
                return ControllerType.GamePad;
            return ControllerType.None;
        }
    }
}