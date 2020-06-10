﻿using System;
using System.Collections.Generic;
using Enums;
using Units;
using UnityEngine;

namespace Projectiles {
    public class ProjectileComponent : MonoBehaviour {
        List<Action<GameObject, GameObject>> onConnected;
        float speed = 1.5f;
        private Vector3 Direction { get; set; }

        //optional param for setting projectile speed
        public void Initialize(Vector3 dir, List<Action<GameObject, GameObject>> callbacks, float speed = 1.5f) {
            var forward = gameObject.transform.forward;
            Direction = new Vector3(forward.x, 0, forward.z);
            onConnected = callbacks;
            this.speed = speed;
        }

        void FixedUpdate() => MoveGameObject();
        private void MoveGameObject() => transform.position += Direction * (speed * Time.deltaTime);

        private void OnTriggerEnter(Collider other) {
            // Debug.Log("LOGGING PROJECTILE LOCATION");
            // Debug.Log(transform.position);
            // Apply all callbacks in order from lowest to highest
            for (int i = 0; i < onConnected.Count; i++)
                onConnected[i](other.gameObject, gameObject);
        }
    }
}