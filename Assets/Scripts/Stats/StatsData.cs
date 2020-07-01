using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class StatsData 
    {
        [SerializeField] public float Agility;
        [SerializeField] public float Strength;
        [SerializeField] public float Intelligence;
        [SerializeField] public float Endurance;
        [SerializeField] public float MovementSpeed;

        public StatsData(StatsData data) {
            Agility = data.Agility;
            Strength = data.Strength;
            Intelligence = data.Intelligence;
            Endurance = data.Endurance;
            MovementSpeed = data.MovementSpeed;
        }
    }
}