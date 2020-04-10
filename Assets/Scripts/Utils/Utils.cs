using System;
using System.Collections.Generic;
using Abilities;
using Abilities.AttackAbilities;
using Abilities.Data;
using Controls;
using Enums;
using JetBrains.Annotations;
using Spawner;
using State;
using State.MeleeAiStates;
using State.RangedAiStates;
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
    public static class AbilityFactory
    {
        public static List<Ability> CreateAbilitiesFromData(List<AbilityData> data, Unit owner)
        {
            List<Ability> retVal = new List<Ability>();
            foreach (var a in data)
            {
                if (a == null) throw new Exception("Skill SO was unassigned");
                
                Ability instance;
                switch (a.type)
                {
                    case Types.ShootCrossbow:
                        instance = owner.gameObject.AddComponent<ShootCrossbow>().Initialize((AttackAbilityData)a, owner);
                        retVal.Add(instance);
                        break;
                    case Types.BodySlam:
                        instance = owner.gameObject.AddComponent<BodySlam>().Initialize((AttackAbilityData)a, owner);
                        retVal.Add(instance);
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

                // Playables
                case Units.Types.Hunter:
                    s = "Units/Playable Characters/Hunter";
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

                // Playable
                case Units.Types.Hunter:
                    s = "Data/Playable Characters/Hunter Data";
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
        private static HashSet<Player> players = new HashSet<Player>();

        public static void AddPlayer(Player newPlayer)
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
                    return ray.GetPoint(distanceToPlane);
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

        public static void UpdatePlayerRotation(InputValues input, Unit owner, Stat movementSpeed)
        {
            var motion = GetMovementFromInput(input, movementSpeed);

            if (input.ActiveControl == ControllerType.Delta)
                UpdatePlayerRotationForKeyboard(input, motion, owner);
            else if (input.ActiveControl == ControllerType.GamePad)
                UpdatePlayerRotationForGamepad(input, motion, owner, movementSpeed);
            else
                Debug.Log("updating for neither");
        }

        private static void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion, Unit owner,
            Stat movementSpeed)
        {
            // Debug.Log("updating for gamepad");

            var posX = input.Turn * movementSpeed.Value * Time.deltaTime;
            var posY = 0;
            var posZ = input.Look * movementSpeed.Value * Time.deltaTime;
            var rotationVal = new Vector3(posX, posY, posZ);

            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(rotationVal),
                Time.deltaTime * 10f);
        }

        private static void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion, Unit owner)
        {
            // Debug.Log("updating for keyboard");
            var mousePos = Utils.MouseHelper.GetWorldPosition();

            var transform = owner.transform;

            var difference = mousePos - transform.position;
            owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }

        public static Vector3 GetMovementFromInput(InputValues input, Stat movementSpeedStat)
        {
            var movementSpeed = movementSpeedStat.Value;
            var posX = input.Horizontal * movementSpeed * Time.deltaTime;
            var posY = 0;
            var posZ = input.Forward * movementSpeed * Time.deltaTime;

            var motion = new Vector3(posX, posY, posZ);
            return motion;
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
                case UnitStateEnum.RangedAi:
                    return new State.RangedAiStates.IdleUnitState(owner);
                default:
                    return null;
            }
        }
    }
}