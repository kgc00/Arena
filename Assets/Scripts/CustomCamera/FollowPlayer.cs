using System.Linq;
using Enums;
using Units;
using UnityEngine;

namespace CustomCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform unitTransform;
        private Unit unit;
        readonly Vector3 offset = new Vector3(0,10,-10);
    
        void Start()
        {
            AssignUnitTransform();
        }

        private void AssignUnitTransform()
        {
            unitTransform = FindObjectsOfType<Unit>()
                .FirstOrDefault(element => element.Owner.ControlType == ControlType.Local)
                ?.transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (unitTransform == null) return;

            var target = new Vector3(
                unitTransform.position.x + offset.x,
                offset.y,
                unitTransform.position.z + offset.z
            );

            // transform.position = Vector3.Slerp(transform.position, target, 3f);
            transform.position = target;
        }
    }
}
