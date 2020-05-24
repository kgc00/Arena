﻿using System;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Ability Data", menuName = "ScriptableObjects/Abilities/Ability Data", order = 0), Serializable]
    public class AbilityData : ScriptableObject {
        [SerializeField] public string displayName; // shown to player via UI
        [SerializeField] public string description; // shown to player via UI
        [SerializeField] public float energyCost; // energy/mana cost per use
        [SerializeField] public float cooldown; // time between uses
        [SerializeField] public int range; // maximum targeting or travel range
        [SerializeField] public int areaOfEffectRadius; // 0 (none), 100f, 200f
        [SerializeField] public Types type; // used by factory to create ability
        [SerializeField] public float startupTime; // delays attack to render targeting graphics
        [SerializeField] public int indicatorType; // Used by shader/ 0=none, 1=circular, 2=linear
        [SerializeField] public float projectileSpeed; // projectile speed 10f is average
        [SerializeField] public float force;
        [SerializeField] public Sprite icon;
    }
}