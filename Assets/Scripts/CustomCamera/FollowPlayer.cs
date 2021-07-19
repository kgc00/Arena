using System.Collections;
using System.Linq;
using Data.Types;
using Units;
using UnityEngine;

namespace CustomCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform unitTransform;
        private Unit unit;
        [SerializeField, Range(5,10)] float _offset;
        Vector3 _camOffset => new Vector3(0, _offset, -_offset);
    
        void Start()
        {
            StartCoroutine(AssignUnitTransform());
        }

        private IEnumerator AssignUnitTransform()
        {
            yield return new WaitUntil(() => {
                return FindObjectsOfType<Unit>()
                    ?.FirstOrDefault(element => element.Owner?.ControlType == ControlType.Local)
                    ?.transform != null;
            });
            
            unitTransform = FindObjectsOfType<Unit>()
                ?.FirstOrDefault(element => element.Owner?.ControlType == ControlType.Local)
                ?.transform;
        }

        void LateUpdate()
        {
            if (unitTransform == null) return;

            var target = new Vector3(
                unitTransform.position.x + _camOffset.x,
                _camOffset.y,
                unitTransform.position.z + _camOffset.z
            );

            // transform.position = Vector3.Slerp(transform.position, target, 3f);
            transform.position = target;
        }
    }
}
