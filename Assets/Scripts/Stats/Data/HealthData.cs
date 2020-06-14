using System;
using UnityEngine;

namespace Stats.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] public float maxHp;
        [SerializeField] public bool Invulnerable;
        [HideInInspector] public float currentHp;
        public HealthData(HealthData data) {
            this.maxHp = data.maxHp;
            Invulnerable = data.Invulnerable;
            this.currentHp = data.currentHp;
        }
    }
}