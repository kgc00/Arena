using System;
using Controls;
using Enums;
using UnityEngine;
using Utils;

public class RunUnitState : UnitState
{
    private Rigidbody body;
    private readonly float movementSpeed;
    private static readonly int Moving = Animator.StringToHash("Moving");

    public RunUnitState(Unit Owner) : base(Owner)
    {
        body = Owner.Rigidbody;
        movementSpeed = 5 * Owner.BaseStats.MovementSpeed.Value;
    }

    public override UnitState HandleInput(InputValues input)
    {
        var playerIsStationary = Math.Abs(input.Forward) <= 0 && Math.Abs(input.Horizontal) <= 0;
        if (playerIsStationary) return new IdleUnitState(Owner);
        
        return null;
    }

    public override void HandleFixedUpdate(InputValues input)
    {
        var motion = GetMovementFromInput(input);

        UpdatePlayerRotation(input, motion);
        UpdatePlayerPositionTr(input, motion);
        Owner.Animator.SetBool(Moving, true);
    }

    private void UpdatePlayerRotation(InputValues input, Vector3 motion)
    {
        if (input.ActiveControl == ControllerType.Delta)
            UpdatePlayerRotationForKeyboard(input, motion);
        else if (input.ActiveControl ==  ControllerType.GamePad)
            UpdatePlayerRotationForGamepad(input, motion);
        else
            Debug.Log("updating for neither");
    }
    
    private void UpdatePlayerRotationForKeyboard(InputValues input, Vector3 motion)
    {
        // Debug.Log("updating for keyboard");
        var mousePos = Utils.MouseHelper.GetWorldPosition();
        
        var difference = mousePos - Owner.transform.position;
        Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
            Quaternion.LookRotation(difference),
            Time.deltaTime * 10f);
    }
    
    
    private void UpdatePlayerRotationForGamepad(InputValues input, Vector3 motion)
    {        
        // Debug.Log("updating for gamepad");
    
        var posX = input.Turn * movementSpeed * Time.deltaTime ;
        var posY = 0;
        var posZ = input.Look * movementSpeed * Time.deltaTime ;    
        var rotationVal = new Vector3(posX,posY,posZ);
        
        Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
            Quaternion.LookRotation(rotationVal),
            Time.deltaTime * 10f);
    }

    private Vector3 GetMovementFromInput(InputValues input)
    {
        var posX = input.Horizontal * movementSpeed * Time.deltaTime;
        var posY = 0;
        var posZ = input.Forward * movementSpeed * Time.deltaTime;

        var motion = new Vector3(posX, posY, posZ);
        return motion;
    }

    private void UpdatePlayerPositionTr(InputValues input, Vector3 motion)
    {
        //reduce input for diagonal movement
        motion *= Mathf.Abs(input.Horizontal) == 1 && Mathf.Abs(input.Forward) == 1 ? 0.0f : 1;

        Owner.transform.position += motion;
    }
}