using Controls;
using Enums;
using UnityEngine;

namespace Utils
{
    public static class MouseHelper
    {

        //Method 4
        static Plane plane = new Plane(Vector3.up, 0f);

        static Camera cam = Camera.main;

        public static Vector3 GetWorldPosition()
        {
            if (cam != null)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distanceToPlane))
                {
                    return ray.GetPoint(distanceToPlane);
                }
            }

            return Vector3.zero;
        }
    }

    public static class UnitRotation
    {
        public static void UpdatePlayerRotation(InputValues input, Unit owner, Stat movementSpeed)
        {
            var motion = GetMovementFromInput(input, movementSpeed);
            
            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion, owner);
            else if (input.ActiveControl ==  ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion, owner, movementSpeed);
            else
                Debug.Log("updating for neither");
        }

        private static void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion, Unit owner, Stat movementSpeed)
        {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed.Value * Time.deltaTime ;
            var posY = 0;
            var posZ = input.Look * movementSpeed.Value * Time.deltaTime ;    
            var rotationVal = new Vector3(posX,posY,posZ);
        
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(rotationVal),
                Time.deltaTime * 10f);
        }

        private static void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion, Unit owner)
        {
            // Debug.Log("updating for keyboard");
            var mousePos = Utils.MouseHelper.GetWorldPosition();

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
}