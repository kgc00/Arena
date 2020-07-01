using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Data;
using State;
using Stats;
using Stats.Data;
using UnityEngine;

namespace Units.Data
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "ScriptableObjects/Units/UnitData", order = 0),  Serializable]
    public class UnitData : ScriptableObject
    {
        // Abilities
        [SerializeField] public List<AbilityData> abilities;
        // Health
        [SerializeField] public HealthData health;
        // Exp
        [SerializeField] public ExperienceData experience;
        // States
        [SerializeField] public UnitStateEnum state;
        // Visuals
        [SerializeField] public VisualAssets visualAssets;
        // Stats
        [SerializeField] public StatsData statsData;


    }
}