using System;
using UnityEngine;

namespace Data.StatData
{
    [Serializable]
    public class StatsData 
    {
        [SerializeField] public int agility;
        [SerializeField] public int strength;
        [SerializeField] public int intelligence;
        [SerializeField] public int endurance;
        [SerializeField] public int movementSpeed;

        public StatsData(StatsData data) {
            agility = data.agility;
            strength = data.strength;
            intelligence = data.intelligence;
            endurance = data.endurance;
            movementSpeed = data.movementSpeed;
        }
    }
}