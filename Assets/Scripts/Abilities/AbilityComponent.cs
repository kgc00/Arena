using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour
    {
        public Unit Owner { get; private set; }
        [SerializeField] public List<Ability> equippedAbilities;
        public void Initialize(Unit owner)
        {
            Owner = owner;
            equippedAbilities = GetComponents<Ability>().ToList();

            foreach (var ability in equippedAbilities)
            {
                ability.Owner = owner;
            }
        }
    }
}