using System;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Player Owner { get; private set; }
    public BaseStats BaseStats { get; private set; } = new BaseStats();
    UnitState state;
    [SerializeField] Controller controller;
    [SerializeField] public Rigidbody Rigidbody { get; private set; }
    [SerializeField] public Animator Animator { get; private set; }

    public void Initialize (Player owner) {
        //Owner
        this.Owner = owner;
        
        //State
        state = new IdleUnitState (this);
        state.Enter ();

        //Controller
        controller = GetComponentInChildren<Controller>();
        
        //RigidBody
        if (Rigidbody == null)
        {
            Rigidbody = transform.Find("Model").GetComponent<Rigidbody>();
        }
        
        // Animator
        if (Animator == null)
        {
            Animator = GetComponentInChildren<Animator>();
        }
    }

    void Update () {
        var newState = state?.HandleInput (controller.InputValues);
        if (newState == null) return;
        state = newState;
        state.Enter();
    }

    private void FixedUpdate()
    {
        state.HandleFixedUpdate(controller.InputValues);
    }
}