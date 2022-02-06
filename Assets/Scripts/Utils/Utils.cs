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
using Data.Types;
using State;
using Units;
using UnityEngine;
using Players;

namespace Utils {
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
                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }

        private static IEnumerator ForceAlongHeading(Collider other, Rigidbody rigidBody, float Force,
            Transform forceComponentTransform) {
            // Debug.Log("LOGGING forceComponentTransform");
            // Debug.Log(forceComponentTransform.position);

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
                        abilityInstance = owner.gameObject.AddComponent<Roar>().Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.ChainFlame:
                        abilityInstance = owner.gameObject.AddComponent<ChainFlame>()
                            .Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    // hunter
                    case AbilityType.Mark:
                        abilityInstance = owner.gameObject.AddComponent<Mark>().Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
                        equippedAbilities.Add(type, abilityInstance);
                        break;
                    case AbilityType.Prey:
                        abilityInstance = owner.gameObject.AddComponent<Prey>().Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
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
                        abilityInstance = owner.gameObject.AddComponent<Rain>().Initialize((data[i] as AttackAbilityData).CreateInstance(), owner, statsComponent);
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

            if (cam != null) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distanceToPlane)) {
                    var mouseLocation = ray.GetPoint(distanceToPlane);
                    mouseLocation.y = 0;
                    return mouseLocation;
                }
            }

            return Vector3.zero;
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
                case UnitStateType.BossAi:
                    return new State.BossAiStates.IdleUnitState(owner);
                default:
                    return null;
            }
        }
    }
}