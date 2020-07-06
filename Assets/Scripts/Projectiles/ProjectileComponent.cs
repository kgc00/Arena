using System;
using System.Collections.Generic;
using Enums;
using Units;
using UnityEngine;

namespace Projectiles {
    public class ProjectileComponent : MonoBehaviour {
        List<Action<GameObject, GameObject>> onConnected;
        private float speed;
        private Vector3 Direction { get; set; }
        // private Rigidbody rigidbody;

        //optional param for setting projectile speed
        public void Initialize(Vector3 dir, List<Action<GameObject, GameObject>> callbacks, float projectileSpeed = default) {
            var forward = gameObject.transform.forward;
            Direction = new Vector3(forward.x, 0, forward.z);
            onConnected = callbacks;
            speed = projectileSpeed;
            // rigidbody = GetComponent<Rigidbody>() ?? throw new Exception($"No rigidbody found on {name}");
        }

        void FixedUpdate() => MoveGameObject();
        private void MoveGameObject() =>  transform.position += Direction * (speed * Time.deltaTime);

        private void OnTriggerEnter(Collider other) {
            // Apply all callbacks in order from lowest to highest
            foreach (var cb in onConnected)
                cb(other.gameObject, gameObject);
        }
    }
}