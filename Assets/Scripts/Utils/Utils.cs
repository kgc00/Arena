using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.AttackAbilities;
using Abilities.Buffs;
using Abilities.Data;
using Controls;
using Enums;
using JetBrains.Annotations;
using Spawner;
using State;
using State.MeleeAiStates;
using State.RangedAiStates;
using Stats;
using Units;
using Units.Data;
using UnityEngine;
using Types = Abilities.Types;

namespace Utils
{
    public static class MathHelpers
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if(val.CompareTo(max) > 0) return max;
            else return val;
        }
    }

    public static class ForceStrategies {
        public enum Type {
            ForceAlongLocalX,
            ForceAlongHeading
        }

        public static Dictionary<Type, Func<Collider, Rigidbody, float, Transform, IEnumerator>> Strategies { get; } =
            new Dictionary<Type, Func<Collider, Rigidbody,float, Transform, IEnumerator>>() {
                {Type.ForceAlongLocalX, ForceAlongLocalX},
                {Type.ForceAlongHeading, ForceAlongHeading}
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
            
            Debug.Log("LOGGING forceComponentTransform");
            Debug.Log(forceComponentTransform.position);
            
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
                Debug.Log($"Applying {appliedForce} force");
                rigidBody.AddForce(appliedForce);
                yield return null;
            }
        }
    }
    
    public static class AbilityFactory
    {
        public static Dictionary<ButtonType, Ability> CreateAbilitiesFromData(List<AbilityData> data, Unit owner)
        {
            var retVal = new Dictionary<ButtonType, Ability>();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == null) throw new Exception("Skill SO was unassigned");
                
                Ability instance;
                ButtonType type = (ButtonType)Enum.ToObject(typeof(ButtonType) , i);
                switch (data[i].type)
                {
                    case Types.ShootCrossbow:
                        instance = owner.gameObject.AddComponent<ShootCrossbow>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.IceBolt:
                        instance = owner.gameObject.AddComponent<IceBolt>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.BodySlam:
                        instance = owner.gameObject.AddComponent<BodySlam>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.Mark:
                        instance = owner.gameObject.AddComponent<Mark>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.Prey:
                        instance = owner.gameObject.AddComponent<Prey>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.Conceal:
                        instance = owner.gameObject.AddComponent<Conceal>().Initialize((BuffAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.PierceAndPull:
                        instance = owner.gameObject.AddComponent<PierceAndPull>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                    case Types.Burst:
                        instance = owner.gameObject.AddComponent<Burst>().Initialize((AttackAbilityData)data[i], owner);
                        retVal.Add(type,instance);
                        break;
                }
            }

            return retVal;
        }
    }

    public static class SpawnHelper
    {
        public static GameObject PrefabFromUnitType(Units.Types unit)
        {
            string s = "";
            switch (unit)
            {
                // Enemies
                case Units.Types.Melee:
                    s = "Units/Enemies/Slime/Melee AI";
                    break;
                case Units.Types.Ranged:
                    s = "Units/Enemies/Lich/Ranged AI";
                    break;
                case Units.Types.TrainingDummy:
                    s = "Units/Enemies/Training Dummy/Training Dummy";
                    break;

                // Playables
                case Units.Types.Hunter:
                    s = "Units/Playable Characters/Hunter/Hunter";
                    break;
            }

            if (s == "") throw new Exception("Unable to locate Prefab");

            return Resources.Load<GameObject>(s);
        }

        public static UnitData DataFromUnitType(Units.Types unit)
        {
            var s = "";
            switch (unit)
            {
                // Enemies
                case Units.Types.Melee:
                    s = "Data/Beastiary/Melee Ai Data";
                    break;
                case Units.Types.Ranged:
                    s = "Data/Beastiary/Ranged Ai Data";
                    break;
                case Units.Types.TrainingDummy:
                    s = "Data/Beastiary/Training Dummy Data";
                    break;

                // Playable
                case Units.Types.Hunter:
                    s = "Data/Playable Characters/Hunter/Hunter Data";
                    break;
            }

            if (s == "") throw new Exception("Unable to locate Data");

            return Resources.Load<UnitData>(s);
        }

        public static Interval IntervalFromType(Intervals interval, GameObject go)
        {
            switch (interval)
            {
                case Intervals.Timer:
                    return go.AddComponent<TimerInterval>();
                case  Intervals.WaveLastEnemyAlive:
                    return go.AddComponent<ContinuousInterval>();
                default:
                    Debug.LogError($"NO TYPE FOUND!");
                    return null;
            }
        }
    }

    public static class PlayerHelper
    {
        private static HashSet<Player.Player> players = new HashSet<Player.Player>();

        public static void AddPlayer(Player.Player newPlayer)
        {
            if (players.Contains(newPlayer)) return;
            players.Add(newPlayer);
        }
    }

    public static class MouseHelper
    {
        static Plane plane = new Plane(Vector3.up, 0f);

        static Camera cam = Camera.main;

        public static Vector3 GetWorldPosition()
        {
            if (cam != null)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distanceToPlane))
                {
                    var mouseLocation = ray.GetPoint(distanceToPlane);
                    mouseLocation.y = 0;
                    return mouseLocation;
                }
            }

            return Vector3.zero;
        }
    }

    public static class RotationHelper
    {
        public static Vector3 GetUnitForward(Unit owner)
        {
            var transform = owner.transform;
            return transform.position + transform.forward * 25;
        }
    }

    public static class StateHelper
    {
        public static UnitState StateFromEnum(UnitStateEnum stateEnum, Unit owner)
        {
            switch (stateEnum)
            {
                case UnitStateEnum.Player:
                    return new State.PlayerStates.IdleUnitState(owner);
                case UnitStateEnum.MeleeAi:
                    return new State.MeleeAiStates.IdleUnitState(owner);
                case UnitStateEnum.TrainingDummy:
                    return new State.TrainingDummy.Idle(owner);
                case UnitStateEnum.RangedAi:
                    return new State.RangedAiStates.IdleUnitState(owner);
                default:
                    return null;
            }
        }
    }
    
    public static class StatusHelper 
    {
        public static void AddMark(GameObject go)
        {
            var unit = go.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) return;
            
            unit.StatusComponent.AddStatus(Status.Marked);
            Debug.Log(unit.StatusComponent.Status);
        }
        
        public static void AddMark(Unit unit)
        {
            unit.StatusComponent.AddStatus(Status.Marked);
            Debug.Log(unit.StatusComponent.Status);
        }
    }
}