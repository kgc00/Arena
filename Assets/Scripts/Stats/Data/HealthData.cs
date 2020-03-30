using System;
using UnityEngine;

namespace Stats.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] public float maxHp;
        [SerializeField] public float currentHp;
    }
}