﻿using System;
using System.Collections.Generic;
using Data.StatData;
using Data.Types;
using UnityEngine;

namespace Data.UnitData
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "ScriptableObjects/Units/UnitData", order = 0),  Serializable]
    public class UnitData : ScriptableObject
    {
        // Abilities
        [SerializeField] public List<AbilityData.AbilityData> abilities;
        // Health
        [SerializeField] public HealthData health;
        // Exp
        [SerializeField] public ExperienceData experience;
        // States
        [SerializeField] public UnitStateType state;
        // Visuals
        [SerializeField] public VisualAssets visualAssets;
        // Stats
        [SerializeField] public StatsData statsData;
    }
}