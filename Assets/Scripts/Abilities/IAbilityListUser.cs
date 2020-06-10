using System.Collections.Generic;
using Abilities.Data;
using UnityEngine;

namespace Abilities
{
    public interface IAbilityListUser
    {
        [SerializeField] List<AbilityData>  Abilities { get; }
    }
}