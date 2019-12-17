using System;
using UnityEngine;
public class Player : MonoBehaviour {
    Unit unit;
    ControlType controlType;

    private void Awake()
    {
        if (unit == null)
        {
            unit = GetComponentInChildren<Unit>();
            unit.Initialize(this);
        }
    }
}