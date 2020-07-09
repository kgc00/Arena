using System;
using UnityEngine;

namespace Data.StatData
{
    [Serializable]
    public class StatsData 
    {
        [SerializeField] public float agility;
        [SerializeField] public float strength;
        [SerializeField] public float intelligence;
        [SerializeField] public float endurance;
        [SerializeField] public float movementSpeed;

        public StatsData(StatsData data) {
            agility = data.agility;
            strength = data.strength;
            intelligence = data.intelligence;
            endurance = data.endurance;
            movementSpeed = data.movementSpeed;
        }
    }
}