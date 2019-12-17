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
}