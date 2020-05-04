﻿using System;
using System.Collections.Generic;
using Abilities.Data;
using State;
using Stats.Data;
using UnityEngine;

namespace Units.Data
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "ScriptableObjects/Units/UnitData", order = 0),  Serializable]
    public class UnitData : ScriptableObject
    {
        [SerializeField] public List<AbilityData> abilities;
        // health
        [SerializeField] public HealthData health;
        // exp
        [SerializeField] public ExperienceData experience;
        // States
        [SerializeField] public UnitStateEnum state;
    }
}