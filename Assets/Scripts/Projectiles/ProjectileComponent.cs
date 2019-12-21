using Units;
using UnityEngine;

namespace Projectiles
{
    
    public class ProjectileComponent : MonoBehaviour
    {
        Vector3 start;
        System.Action<GameObject> onConnected;
        float speed = 1.5f;
        public Vector3 Direction { get; private set; }

        //optional param for setting projectile speed
        public void Initialize (Vector3 dir, System.Action<GameObject> callback, float speed = 1.5f)
        {
            start = transform.position;
            this.Direction = dir;
            this.onConnected = callback;
            this.speed = speed;
        }
        
        void Update () {
            MoveGameObject ();
        }

        
        private void MoveGameObject () {
            transform.Translate (new Vector3 (
                Direction.x * Time.deltaTime * speed,
                0,
                Direction.y * Time.deltaTime * speed 
            ));
        }

        private void OnTriggerEnter(Collider other)
        {
            var unit = other.GetComponent<Unit>();
            if (unit != null)
            {
                onConnected (unit.gameObject);
                Destroy (gameObject);
            }
        }
    }
}