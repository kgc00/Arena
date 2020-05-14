using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using JetBrains.Annotations;
using Units;
using UnityEngine;

namespace Projectiles
{
    public class PullComponent : MonoBehaviour
    {
        #region Vars

        public float Force = 250f;

        [Header("Center")] 
        [Range(-20f, 20f), SerializeField] private float xPos = 0f;
        [Range(-20f, 20f), SerializeField] private float zPos = 0f;

        [Header("Size")] 
        [Range(-25f, 25f), SerializeField] private float xModifier = 0f;
        [Range(-25f, 25f), SerializeField] private float zModifier = 0f;
        
        public Vector3 Bounds { get; private set; }
        private Collider collider;

        private List<ControlType> AffectedFactions;
        public Vector3 LookTarget;
        #endregion

        public GameObject Initialize(float force, ColliderParams colliderParams, Vector3 center, Vector3 lookTarget,
            List<ControlType> affectedFactions) {
            Force = force;
            AffectedFactions = affectedFactions;

            transform.position = center;
            
            InitializeCollider(colliderParams);

            // lock y to unit's current y
            LookTarget = new Vector3(lookTarget.x, gameObject.transform.position.y, lookTarget.z);
            
            gameObject.transform.LookAt(LookTarget);
            return gameObject;
        }

        private void InitializeCollider(ColliderParams colliderParams) {
            if (collider != null) return;
            
            if (colliderParams is BoxParams) {
                var p = (BoxParams) colliderParams;
                var c = gameObject.AddComponent<BoxCollider>();
                Bounds = p.Bounds;
                c.size = Bounds;
                collider = c;
            }

            if (colliderParams is SphereParams) {
                var p = (SphereParams) colliderParams;
                var c = gameObject.AddComponent<SphereCollider>();
                c.radius = p.Radius;
                collider = c;
            }

            collider.isTrigger = true;
        }

        // private void OnEnable() {
        //     Initialize(250f, 20, Vector3.zero);
        // }

        private void OnTriggerEnter(Collider other) {
            if (!ShouldActivate(other, out var rigidBody)) return;

            StartCoroutine(ApplyForce(other, rigidBody));
        }
        
        private IEnumerator ApplyForce(Collider other, Rigidbody rigidBody) {
            Debug.Log($"{other.gameObject.name} will be pulled!");
            
            Vector3 left = transform.TransformDirection(Vector3.left);
            Vector3 heading = other.transform.position - transform.position;
            heading.y = 0f;
            
            // dot scales the value of force to make it stronger as the unit
            // is further away. The unit will always end near center of bounds
            var dot = Vector3.Dot(left, heading.normalized);

            // Force is required to be a negative value because we are pulling
            var scaledForce = (-Math.Abs(Force) * dot); 
            var appliedForce = left * scaledForce;
            
            // Apply force over several frames for a smoother acceleration
            var frames = 10;
            for (int j = 0; j < frames; j++) {
                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }

        private bool ShouldActivate(Collider other, [CanBeNull] out Rigidbody rigidBody) {
            rigidBody = null;
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) return false;

            if (AffectedFactions.All(x => x != unit.Owner.ControlType)) {
                Debug.Log($"Unable to affect {unit.name} because thier faction is {unit.Owner.ControlType}");
                return false;
            }

            rigidBody = other.transform.root.GetComponent<Rigidbody>();
            if (rigidBody == null) {
                Debug.Log($"Unable to affect {unit.name} because they do not posses a rigidbody");
                return false;
            }

            return true;
        }
        
        #region Debug

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = gameObject.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(
                Vector3.zero, 
                Bounds
            );
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawSphere(
                LookTarget, 1f
            );
        }
#endif

        #endregion
    }
}