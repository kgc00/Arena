using System;
using System.Collections.Generic;
using Enums;
using Units;
using Units.Data;
using UnityEngine;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] List<Unit> units;

    public ControlType ControlType
    {
        get => controlType;
    }

    [SerializeField] private ControlType controlType;

    private void Awake()
    {
        PlayerHelper.AddPlayer(this);
        if (units == null) throw new Exception("no unit has been assigned");
        foreach (var unit in units)
        {
            // Debug.Log(unit);
            // Debug.Log(this);
            unit.Initialize(this,
                data: SpawnHelper.DataFromUnitType(unit.type)
            );
        }
    }

    public void InstantiateUnit(GameObject instance, UnitData unitData, Vector3 pos) =>
        Instantiate(instance, pos, Quaternion.identity)?.GetComponent<Unit>()?.Initialize(this, unitData);
}