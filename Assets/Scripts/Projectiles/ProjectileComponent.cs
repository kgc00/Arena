using Units;
using UnityEngine;

namespace Projectiles
{
    
    public class ProjectileComponent : MonoBehaviour
    {
        System.Action<GameObject> onConnected;
        float speed = 1.5f;
        private Vector3 Direction { get; set; }

        //optional param for setting projectile speed
        public void Initialize (Vector3 dir, System.Action<GameObject> callback, float speed = 1.5f)
        {
            this.Direction = dir;
            this.onConnected = callback;
            this.speed = speed;

            transform.rotation = Quaternion.LookRotation(Direction);
        }
        
        void Update () {
            MoveGameObject ();
        }

        
        private void MoveGameObject ()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            var unit = other.gameObject.GetComponent<Unit>();
            if (unit != null)
            {
                onConnected (unit.gameObject);
                Destroy (gameObject);
            }
        }
    }
}