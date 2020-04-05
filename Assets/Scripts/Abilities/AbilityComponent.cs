using System.Collections.Generic;
using System.Linq;
using Abilities.Data;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour
    {
        public Unit Owner { get; private set; }
        [SerializeField] public List<Ability> equippedAbilities;
        public Ability longestRangeAbility;
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            longestRangeAbility = equippedAbilities.Where(a => a is AttackAbility).OrderByDescending(a => a.Range).First();
            return this;
        }
    }
}