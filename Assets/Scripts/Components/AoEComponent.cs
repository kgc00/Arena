using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Types;
using JetBrains.Annotations;
using Projectiles;
using UnityEngine;
using Utils;

namespace Common
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
        private new Collider collider;

        private List<ControlType> affectedFactions;
        public Vector3 lookTarget;
        public Func<Collider, Rigidbody, float, Transform, IEnumerator> EnterStrategy;
        public Func<Collider, Rigidbody, float, Transform, IEnumerator> StayStrategy;
        public Func<Collider, Rigidbody, float, Transform, IEnumerator> ExitStrategy;
        public float Duration { get; private set; }
        #endregion

        public AoEComponent Initialize(ColliderParams colliderParams,
            Vector3 center,
            Vector3 lookTarget,
            Func<Collider, Rigidbody, float, Transform, IEnumerator> enterStrategy,
            Func<Collider, Rigidbody, float, Transform, IEnumerator> stayStrategy,
            Func<Collider, Rigidbody, float, Transform, IEnumerator> exitStrategy,
            List<ControlType> affectedFactions,
            float force = default,
            float duration = -1) {
            Duration = duration;
            Force = force;
            this.affectedFactions = affectedFactions;
            EnterStrategy = enterStrategy;
            StayStrategy = stayStrategy;
            ExitStrategy = exitStrategy;

            transform.position = center;
            
            InitializeCollider(colliderParams);

            // lock y to unit's current y
            this.lookTarget = new Vector3(lookTarget.x, gameObject.transform.position.y, lookTarget.z);
            gameObject.transform.LookAt(this.lookTarget);
            
            return this;
        }
        
        private void InitializeCollider(ColliderParams colliderParams) {
            if (collider != null) return;
            
            if (colliderParams is BoxParams boxParams) {
                var c = gameObject.AddComponent<BoxCollider>();
                Bounds = boxParams.Bounds;
                c.size = Bounds;
                collider = c;
            }

            if (colliderParams is SphereParams sphereParams) {
                var c = gameObject.AddComponent<SphereCollider>();
                Radius = sphereParams.Radius;
                c.radius = sphereParams.Radius;
                collider = c;
            }

            collider.isTrigger = true;
        }

        private void OnTriggerStay(Collider other) {
            if (StayStrategy == null) return;
            
            if (!ShouldActivate(other, out var rigidBody)) return;
            
            StartCoroutine(StayStrategy(other, rigidBody, Force, transform));
        }

        private void OnTriggerEnter(Collider other) {
            if (EnterStrategy == null) return;

            if (!ShouldActivate(other, out var rigidBody)) return;
            
            StartCoroutine(EnterStrategy(other, rigidBody, Force, transform));
        }

        private void OnTriggerExit(Collider other) {
            if (ExitStrategy == null) return;

            if (!ShouldActivate(other, out var rigidBody)) return;
            
            StartCoroutine(ExitStrategy(other, rigidBody, Force, transform));
        }

        private bool ShouldActivate(Collider other, [CanBeNull] out Rigidbody rigidBody) {
            rigidBody = null;
            var unit = other.gameObject.GetUnitComponent();
            if (unit == null) return false;

            if (affectedFactions.All(x => x != unit.Owner.ControlType))
                return false;

            rigidBody = other.transform.root.GetComponent<Rigidbody>();
            if (rigidBody == null) {
                Debug.Log($"Unable to affect {unit.name} because they do not posses a rigidbody");
                return false;
            }

            return true;
        }

        private void Update() {
            // -1 means we aren't using the duration value
            if (Duration == -1) return;
            
            if (Duration <= 0) Destroy(gameObject);
            else Duration -= Time.deltaTime;
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
                lookTarget, 1f
            );
        }
#endif

        #endregion
    }
}