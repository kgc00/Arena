using System;
using Enums;
using Units;
using UnityEngine;

namespace Projectiles
{
    
    public class ProjectileComponent : MonoBehaviour
    {
        Action<GameObject, GameObject> onConnected;
        float speed = 1.5f;
        private Vector3 Direction { get; set; }

        //optional param for setting projectile speed
        public void Initialize (Vector3 dir, Action<GameObject, GameObject> callback, float speed = 1.5f)
        {
            var forward = gameObject.transform.forward;
            Direction = new Vector3(forward.x, 0, forward.z);
            onConnected = callback;
            this.speed = speed;
        }
        
        void FixedUpdate () => MoveGameObject ();
        private void MoveGameObject () => transform.position +=  Direction * (speed * Time.deltaTime);

        private void OnTriggerEnter(Collider other)  => onConnected (other.gameObject, gameObject);
    }
}