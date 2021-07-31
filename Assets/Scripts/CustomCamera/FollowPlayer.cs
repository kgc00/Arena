using System.Collections;
using System.Linq;
using Data.Types;
using Units;
using UnityEngine;

namespace CustomCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        public enum FollowType {
            Over_Shoulder,
            Above_Head
        }

        private Transform unitTransform;
        private Unit unit;
        [SerializeField] public FollowType _followType = FollowType.Above_Head;
        [SerializeField, Range(0,30)] float _distance = 7.5f;
        [SerializeField, Range(0,1)] float _angle = 0.5f;
        [SerializeField, Range(-5,5)] private float _verticalOffset = 1.25f;
        [SerializeField, Range(-5,5)] private float _forwardOffset = 0;
        Vector3 _camOffset => new Vector3(0, _distance, -_distance);
        private Vector3 _camAngle => new Vector3(_angle * 90f, 0, 0);
    
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
                _camOffset.y * _angle + _verticalOffset,
                unitTransform.position.z + _camOffset.z * (1 - _angle) + _forwardOffset
            );

            // transform.position = Vector3.Slerp(transform.position, target, 3f);
            transform.position = target;
            transform.localRotation = Quaternion.Euler(_camAngle);
        }
    }
}
