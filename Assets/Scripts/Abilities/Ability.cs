using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public float Range { get; protected set; }

        public float CooldownTime{ get; protected set; }
        public Unit Owner;
        public abstract void Activate ();
    }
}