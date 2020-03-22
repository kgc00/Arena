using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats", order = 0)]
    public class Stats : ScriptableObject
    {
        public float MovementSpeed;
        public float Health;
        public float Attack;
        public float Defense;
        public float Bounty;
    }
}