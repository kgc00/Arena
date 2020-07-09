using System.Collections.Generic;
using Data.AbilityData;
using UnityEngine;

namespace Abilities
{
    public interface IAbilityListUser
    {
        [SerializeField] List<AbilityData>  Abilities { get; }
    }
}