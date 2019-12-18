using System;
using Controls;
using State;
using State.PlayerStates;
using Stats;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour {
        public Player Owner { get; private set; }
        public BaseStats BaseStats { get; private set; } = new BaseStats();
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        UnitState state;
        [SerializeField] private UnitStateEnum initialUnitState;
        [SerializeField] Controller controller;
        public HealthComponent HealthComponent { get; private set; }

        public void Initialize (Player owner) {
            //Owner
            this.Owner = owner;

            //Controller
            if (controller == null) controller = GetComponentInChildren<Controller>();
            
            //RigidBody
            if (Rigidbody == null) Rigidbody = GetComponentInChildren<Rigidbody>();
            
        
            // Animator
            if (Animator == null) Animator = GetComponentInChildren<Animator>();
            
            
            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>();
            HealthComponent.Initialize(this);
            
            //State
            state = Utils.StateHelper.StateFromEnum(initialUnitState, this);
            state.Enter ();
        }

        void Update () {
            // handle state
            var newState = state?.HandleUpdate (controller.InputValues);
            if (newState == null) return;
            state = newState;
            state.Enter();
        
        }

        private void FixedUpdate()
        {
            state?.HandleFixedUpdate(controller.InputValues);
        }

        private void OnCollisionEnter(Collision other)
        {
            state?.HandleCollisionEnter(other);
        }
    }
}