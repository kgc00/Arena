using System;
using System.Collections.Generic;
using Enums;
using Units;
using UnityEngine;
public class Player : MonoBehaviour {
   [SerializeField] List<Unit> units;
    public ControlType ControlType { get => controlType; }
    [SerializeField] private ControlType controlType;
    
    private void Awake()
    {
        if (units == null) throw new Exception("no unit has been assigned");
        foreach (var unit in units)
        {
            unit.Initialize(this);
        }
    }
}