using System;
using Data.Types;
using Data.UnitData;
using Spawner;
using UnityEngine;

namespace Utils {
    public static class SpawnHelper
    {
        public static GameObject PrefabFromUnitType(UnitType unit)
        {
            string s = "";
            switch (unit)
            {
                // Enemies
                case UnitType.Melee:
                    s = "Units/Enemies/Slime/Melee AI";
                    break;
                case UnitType.Ranged:
                    s = "Units/Enemies/Lich/Ranged AI";
                    break;
                case UnitType.TrainingDummy:
                    s = "Units/Enemies/Training Dummy/Training Dummy";
                    break;
                case UnitType.Charging:
                    s = "Units/Enemies/Grunt/Charging AI";
                    break;
                case UnitType.BombThrowing:
                    s = "Units/Enemies/Beholder/Bomb Throwing AI";
                    break;
                case UnitType.Boss:
                    s = "Units/Enemies/Dragon/Boss AI";
                    break;

                // Playables
                case UnitType.Hunter:
                    s = "Units/Playable Characters/Hunter/Hunter";
                    break;
            }

            if (s == "") throw new Exception("Unable to locate Prefab");

            return Resources.Load<GameObject>(s);
        }
    }
}