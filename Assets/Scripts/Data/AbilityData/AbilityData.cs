using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.Types;
using UnityEngine;

namespace Data.AbilityData {
    [CreateAssetMenu(fileName = "Ability Data", menuName = "ScriptableObjects/Abilities/Ability Data", order = 0),
     Serializable]
    public class AbilityData : ScriptableObject {
        [SerializeField] public int unlockCost; // price of unlocking the ability in the shop
        [SerializeField] public bool unlocked; // if toggled off, ability is unusable
        [SerializeField] public int areaOfEffectRadius; // 0 (none), 100f, 200f
        [SerializeField] public float cooldown; // time between uses
        [SerializeField] public float minimumCooldown; // time between uses
        [SerializeField] public string description; // shown to player via UI
        [SerializeField] public string displayName; // shown to player via UI
        [SerializeField] public float duration;
        [SerializeField] public float energyCost; // energy/mana cost per use
        [SerializeField] public float force;
        [SerializeField] public Sprite icon;
        [SerializeField] public IndicatorType indicatorType; 
        [SerializeField] public float projectileSpeed; // projectile speed 10f is average
        [SerializeField] public float range; // maximum travel range
        [SerializeField] public float startupTime; // delays attack to render targeting graphics
        [SerializeField] public AbilityType type; // used by factory to create ability
        [SerializeField] public List<AbilityModifierType> modifiers; // currently active modifiers
        [SerializeField] public List<AbilityModifierType> equipableModifiers; // any potential modifiers
    }
}