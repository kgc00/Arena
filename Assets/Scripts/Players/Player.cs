using System;
using System.Collections.Generic;
using Data.Types;
using Data.UnitData;
using Units;
using UnityEngine;
using Utils;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] List<Unit> units;

        public List<Unit> Units
        {
            get => units;
            private set => units = value;
        }

        public ControlType ControlType => controlType;

        [SerializeField] private ControlType controlType;

        private void Awake()
        {
            PlayerHelper.AddPlayer(this);
            if (units == null) throw new Exception("no unit has been assigned");
            foreach (var unit in units)
            {
                unit.Initialize(this,
                    data: Data.DataHelper.DataFromUnitType(unit.unitType)
                );
            }
        }

        public Unit InstantiateUnit(GameObject instance, UnitData unitData, Vector3 pos)
        {
            var unit = Instantiate(instance, pos, Quaternion.identity)?.GetComponent<Unit>()?.Initialize(this, unitData);
            units.Add(unit);
            return unit;
        }

        public void RemoveUnit(Unit unit) => units.Remove(unit);
    }
}