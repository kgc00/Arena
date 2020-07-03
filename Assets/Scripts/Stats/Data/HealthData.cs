﻿using System;
using UnityEngine;

namespace Stats.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] public float maxHp;
        [SerializeField] public bool invulnerable;
        [HideInInspector] public float currentHp;
        public HealthData(HealthData data) {
            maxHp = data.maxHp;
            invulnerable = data.invulnerable;
            currentHp = data.currentHp;
        }
    }
}