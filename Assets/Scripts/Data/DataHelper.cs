using System;
using Data.Types;
using Data.UnitData;
using UnityEngine;

namespace Data {
    public static class DataHelper {
        public static UnitData.UnitData DataFromUnitType(UnitType unit)
        {
            var s = "";
            switch (unit)
            {
                // Enemies
                case UnitType.Melee:
                    s = "Data/Beastiary/Melee Ai Data";
                    break;
                case UnitType.Ranged:
                    s = "Data/Beastiary/Ranged Ai Data";
                    break;
                case UnitType.TrainingDummy:
                    s = "Data/Beastiary/Training Dummy Data";
                    break;
                case UnitType.Charging:
                    s = "Data/Beastiary/Charging Ai Data";
                    break;
                case UnitType.Boss:
                    s = "Data/Beastiary/Boss Ai Data";
                    break;

                

                // Playable
                case UnitType.Hunter:
                    s = "Data/Playable Characters/Hunter/Hunter Data";
                    break;
            }

            if (s == "") throw new Exception("Unable to locate Data");

            return Resources.Load<UnitData.UnitData>(s).CreateInstance();
        }
    }
}