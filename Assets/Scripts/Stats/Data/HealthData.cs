using System;
using UnityEngine;

namespace Stats.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] public float maxHp;
        [HideInInspector] public float currentHp;
    }
}