using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Ability Data", menuName = "ScriptableObjects/Abilities/Ability Data", order = 0)]
    public class AbilityData : ScriptableObject {
        [SerializeField] public string DisplayName;
        [SerializeField] public int Range;
        [SerializeField] public float EnergyCost;
        [SerializeField] public int AreaOfEffectRadius; // 0 (none), 100f, 200f
        [SerializeField] public string Description;
        [SerializeField] public Types type;
    }
}