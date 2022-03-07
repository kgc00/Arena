using System;
using UnityEngine;

namespace Data.StatData
{
    [Serializable]
    public class ExperienceData
    {
        [SerializeField] public int currentExp;
        [SerializeField] public int bounty;
        public ExperienceData(ExperienceData data) {
            currentExp = data.currentExp;
            bounty = data.bounty;
        }
    }
}