using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Enums;
using JetBrains.Annotations;
using Units;
using UnityEngine;

namespace Projectiles
{
    public class AoEComponent : MonoBehaviour
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
        public float Radius { get; private set; }
        private Collider collider;

        private List<ControlType> AffectedFactions;
        public Vector3 LookTarget;
        public Func<Collider, Rigidbody, float, Transform, IEnumerator> Strategy;
        #endregion

        public AoEComponent Initialize(ColliderParams colliderParams, 
            Vector3 center, 
            Vector3 lookTarget,
            Func<Collider, Rigidbody, float, Transform, IEnumerator> Strategy,
            List<ControlType> affectedFactions,
            float force = default) {
            Force = force;
            AffectedFactions = affectedFactions;
            this.Strategy = Strategy;

            transform.position = center;
            
            InitializeCollider(colliderParams);

            // lock y to unit's current y
            LookTarget = new Vector3(lookTarget.x, gameObject.transform.position.y, lookTarget.z);
            gameObject.transform.LookAt(LookTarget);
            
            return this;
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
                Radius = p.Radius;
                c.radius = p.Radius;
                collider = c;
            }

            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (!ShouldActivate(other, out var rigidBody)) return;

            Debug.Log($"{other.gameObject.name} will be {(Force < 0 ? "pushed" : "pulled")}!");
            StartCoroutine(Strategy(other, rigidBody, Force, transform));
        }

        private bool ShouldActivate(Collider other, [CanBeNull] out Rigidbody rigidBody) {
            rigidBody = null;
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) return false;

            if (AffectedFactions.All(x => x != unit.Owner.ControlType)) {
                Debug.Log($"Unable to affect {unit.name} because their faction is {unit.Owner.ControlType}");
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
            
            Gizmos.DrawWireSphere(Vector3.zero, Radius);
            
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