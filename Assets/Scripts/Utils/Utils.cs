using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.AttackAbilities;
using Abilities.Buffs;
using Abilities.Modifiers;
using Abilities.Modifiers.AbilityModifierShopData;
using Common;
using Components;
using Data.AbilityData;
using Data.Modifiers;
using Data.Stats;
using Data.Types;
using State;
using Units;
using UnityEngine;
using Players;

namespace Utils {
    public static class StatHelpers {
        public static int CapForStat(StatType type) {
            switch (type) {
                case StatType.Strength:
                case StatType.Endurance:
                case StatType.Agility:
                    return 100;
                case StatType.MovementSpeed:
                case StatType.Intelligence:
                    return 150;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        } 
        
        public static string GetDescription(StatType type, int newValue) {
            switch (type) {
                case StatType.Agility:
                    return $"TBD";
                case StatType.Endurance:
                    return $"Increases max health by {Math.Round(GetHealthIncreaseModifier(newValue) * 100, 2)}%";
                case StatType.Intelligence:
                    return $"Increases AoE radius by {Math.Round(GetAoERadiusModifier(newValue) * 100, 2)}% and reduces ability cooldowns by {Math.Round((1 - GetAbilityCooldownModifier(newValue)) * 100, 2)}%";
                case StatType.Strength:
                    return $"Increases global damage by {Math.Round(GetDamageIncreaseModifier(newValue) * 100, 2)}%";
                case StatType.MovementSpeed:
                    return $"Sets movement speed to {newValue} units / second";
                default:
                    return "Unable to find stat";
            }
        }

        public static float GetAbilityCooldown(float baseCooldown, Stats stats, float minimumAbilityCooldown = 0f) {
            var cooldownModifier = Mathf.Max(stats.Intelligence.Value, 1) - 1;
            var cooldownReduction = GetAbilityCooldownModifier(cooldownModifier);
            return Mathf.Max(minimumAbilityCooldown, baseCooldown * cooldownReduction);
        }

        private static float GetAbilityCooldownModifier(float baseValue) {
            var aoeRadiusIncrease =  1 - baseValue / CapForStat(StatType.Intelligence); // intel of 150 = no cooldown
            return aoeRadiusIncrease;
        }
        
        public static int GetMaxHealth(float baseMaxHp, Stats stats) {
            var healthModifier = Mathf.Max(stats.Endurance.Value, 1) - 1;
            var healthIncrease = GetHealthIncreaseModifier(healthModifier);
            return (int) (baseMaxHp + baseMaxHp  * healthIncrease);
        }

        private static float GetHealthIncreaseModifier(float baseValue) {
            var healthIncrease = baseValue / CapForStat(StatType.Endurance); // Endurance of 100 = double hp
            return healthIncrease;
        }

        public static float GetAoERadius(int baseAreaOfEffectRadius, Stats stats) {
            var aoeRadiusModifier = Mathf.Max(stats.Intelligence.Value, 1) - 1;
            var aoeRadiusIncrease = GetAoERadiusModifier(aoeRadiusModifier);
            return baseAreaOfEffectRadius + baseAreaOfEffectRadius  * aoeRadiusIncrease;
        }

        private static float GetAoERadiusModifier(float baseValue) {
            var aoeRadiusIncrease = baseValue / CapForStat(StatType.Intelligence); // intel of 150 = 150% radius
            return aoeRadiusIncrease;
        }

        public static float GetDamage(float baseDamage, Stats stats) {
            var damageModifier = Mathf.Max(stats.Strength.Value, 1) - 1;
            var damageIncrease = GetDamageIncreaseModifier(damageModifier);
            return baseDamage + baseDamage  * damageIncrease;
        }

        private static float GetDamageIncreaseModifier(float baseValue) {
            var damageIncrease = baseValue / CapForStat(StatType.Strength); // strength of 100 = double damage
            return damageIncrease;
        }
    }
    
    public static class MathHelpers {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }

    public static class ForceStrategies {
        public static Dictionary<ForceStrategyType, Func<Collider, Rigidbody, float, Transform, IEnumerator>>
            Strategies { get; } =
            new Dictionary<ForceStrategyType, Func<Collider, Rigidbody, float, Transform, IEnumerator>>() {
                {ForceStrategyType.ForceAlongLocalX, ForceAlongLocalX},
                {ForceStrategyType.ForceAlongHeading, ForceAlongHeading}
            };

        private static IEnumerator ForceAlongLocalX(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            Vector3 left = forceComponentTransform.TransformDirection(Vector3.left);
            Vector3 heading = other.transform.position - forceComponentTransform.position;
            heading.y = 0f;

            // dot scales the value of force to make it stronger as the unit
            // is further away. The unit will always end near center of bounds
            var dot = Vector3.Dot(left, heading.normalized);

            // Force is required to be a negative value because we are pulling
            var scaledForce = Force * dot;
            var appliedForce = left * scaledForce;

            // Apply force over several frames for a smoother acceleration
            var frames = 10;
            for (int j = 0; j < frames; j++) {
                if (rigidBody == null) yield break;
                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }

        private static IEnumerator ForceAlongHeading(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            // Debug.Log("LOGGING forceComponentTransform");
            // Debug.Log(forceComponentTransform.position);

            if (other == null || forceComponentTransform == null) yield break;
            Vector3 heading = other.transform.position - forceComponentTransform.position;
            heading.y = 0f;
            heading = heading.normalized;
            heading = heading == Vector3.zero ? forceComponentTransform.forward : heading;
            var appliedForce = heading * Force;

            // dot scales the value of force to make it stronger as the unit
            // is further away. The unit will always end near center of bounds
            // var dot = Vector3.Dot(left, heading.normalized);

            // Force is required to be a negative value because we are pulling
            // var scaledForce = Force * dot;
            // var appliedForce = left * scaledForce;

            // Apply force over several frames for a smoother acceleration
            var frames = 10;
            for (int j = 0; j < frames; j++) {
                if (rigidBody == null) break;
                // Debug.Log($"Applying {appliedForce} force");
                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }
    }

    public static class AbilityFactory {
        public static AbilityModifierShopData AbilityModifierShopDataFromType(AbilityModifierType modifierType) {
            var path = $"{Constants.AbilityModifierShopDataPath}" + modifierType switch {
                AbilityModifierType.DoubleDamage => "DoubleDamageAbilityModifierShopData",
                AbilityModifierType.DamageOnCollision =>
                    "DamageOnCollisionAbilityModifierShopData",
                AbilityModifierType.DoubleMovementSpeed =>
                    "DoubleMovementSpeedAbilityModifierShopData",
                AbilityModifierType.ExplosionAroundCaster =>
                    "ExplosionAroundCasterAbilityModifierShopData",
                AbilityModifierType.PersistentAddMarkOnHit =>
                    "PersistentMarkOnHitAbilityModifierShopData",
                AbilityModifierType.ConcealPersistentAddMarkOnHit =>
                    "ConcealMarkOnHitAbilityModifierShopData",
                _ => throw new Exception("Unable to locate Data... Unhandled type - " + modifierType)
            };

            // if these are ever modifier we'll want to return unique instances or something
            // -- .CreateInstance()
            return Resources.Load<AbilityModifierShopData>(path);
        }

        public static Dictionary<ButtonType, Ability> CreateAbilitiesFromData(List<AbilityData> data, Unit owner,
            StatsComponent statsComponent) {
            var equippedAbilities = new Dictionary<ButtonType, Ability>();
            for (int i = 0; i < data.Count; i++) {
                if (data[i] == null) throw new Exception("Skill SO was unassigned");

                Ability abilityInstance;
                var type = (ButtonType) Enum.ToObject(typeof(ButtonType), i);
                switch (data[i].type) {
                    // enemies
                    case AbilityType.IceBolt:
                        abilityInstance = owner.gameObject.AddComponent<IceBolt>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.BodySlam:
                        abilityInstance = owner.gameObject.AddComponent<BodySlam>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.OrcSlash:
                        abilityInstance = owner.gameObject.AddComponent<OrcSlash>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Charge:
                        abilityInstance = owner.gameObject.AddComponent<Charge>()
                            .Initialize((data[i] as MovementAttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.MagicShield:
                        abilityInstance = owner.gameObject.AddComponent<MagicShield>()
                            .Initialize((data[i] as BuffAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Roar:
                        abilityInstance = owner.gameObject.AddComponent<Roar>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.ChainFlame:
                        abilityInstance = owner.gameObject.AddComponent<ChainFlame>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Disrupt:
                        abilityInstance = owner.gameObject.AddComponent<Disrupt>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.MissileStorm:
                        abilityInstance = owner.gameObject.AddComponent<MissileStorm>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    // hunter
                    case AbilityType.Mark:
                        abilityInstance = owner.gameObject.AddComponent<Mark>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Prey:
                        abilityInstance = owner.gameObject.AddComponent<Prey>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Conceal:
                        abilityInstance = owner.gameObject.AddComponent<Conceal>()
                            .Initialize((data[i] as BuffAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.PierceAndPull:
                        abilityInstance = owner.gameObject.AddComponent<PierceAndPull>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Burst:
                        abilityInstance = owner.gameObject.AddComponent<Burst>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Rain:
                        abilityInstance = owner.gameObject.AddComponent<Rain>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    default:
                        Debug.LogWarning("Skill Instance was not assigned");
                        break;
                }
            }

            return equippedAbilities;
        }

        public static AbilityModifier AbilityModifierFromEnum(Ability ability, AbilityModifierType type) {
            AbilityModifier instance = type switch {
                AbilityModifierType.AddMarkOnHit => new MarkOnHitModifier(ability),
                AbilityModifierType.PersistentAddMarkOnHit => new PersistentMarkOnHitAttackModifier(ability),
                AbilityModifierType.DoubleDamage => new DoubleDamageModifier(ability),
                AbilityModifierType.BaseAbilityModifier => new AbilityModifier(ability),
                AbilityModifierType.ExplosionAroundCaster => new ExplosionAroundCasterModifier(ability),
                AbilityModifierType.DamageOnCollision => new DamageOnCollision(ability),
                AbilityModifierType.DoubleMovementSpeed => new DoubleMovementSpeedModifier(ability),
                AbilityModifierType.ConcealPersistentAddMarkOnHit => new ConcealPersistentMarkOnHitModifier(ability),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return instance;
        }
    }

    public static class PlayerHelper {
        private static HashSet<Player> players = new HashSet<Player>();

        public static void AddPlayer(Player newPlayer) {
            if (players.Contains(newPlayer)) return;
            players.Add(newPlayer);
        }
    }

    public static class MouseHelper {
        static Plane plane = new Plane(Vector3.up, 0f);

        static Camera cam = Camera.main;

        public static Vector3 GetWorldPosition() {
            if (cam == null) cam = Camera.main;

            if (cam == null) return Vector3.zero;
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!plane.Raycast(ray, out var distanceToPlane)) return Vector3.zero;
            var mouseLocation = ray.GetPoint(distanceToPlane);
            mouseLocation.y = 0;
            return mouseLocation;
        }
    }

    public static class RotationHelper {
        public static Vector3 GetUnitForward(Unit owner) {
            var transform = owner.transform;
            return transform.position + transform.forward * 25;
        }
    }

    public static class TransformExtensions {
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName) {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0) {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }

            return null;
        }
    }

    public static class StateHelper {
        public static UnitState StateFromEnum(UnitStateType stateType, Unit owner) {
            switch (stateType) {
                case UnitStateType.Player:
                    return new State.PlayerStates.IdleUnitState(owner);
                case UnitStateType.MeleeAi:
                    return new State.MeleeAiStates.IdleUnitState(owner);
                case UnitStateType.TrainingDummy:
                    return new State.TrainingDummy.Idle(owner);
                case UnitStateType.RangedAi:
                    return new State.RangedAiStates.IdleUnitState(owner);
                case UnitStateType.ChargingAi:
                    return new State.ChargingAiStates.IdleUnitState(owner);
                case UnitStateType.BombThrowingAi:
                    return new State.BombThrowingAiStates.IdleUnitState(owner);
                case UnitStateType.BossAi:
                    return new State.BossAiStates.IdleUnitState(owner);
                default:
                    return null;
            }
        }
    }
}