using System;
using Abilities;
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
        public UnitState State { get; protected set; }
        [SerializeField] private UnitStateEnum initialUnitState;
        [SerializeField] Controller controller;
        public AbilityComponent AbilityComponent { get; private set; }
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

            // Abilities
            if (AbilityComponent == null)AbilityComponent= gameObject.AddComponent<AbilityComponent>();
            AbilityComponent.Initialize(this);
            
            //State
            State = Utils.StateHelper.StateFromEnum(initialUnitState, this);
            State.Enter ();
        }

        void Update () {
            // handle state
            var newState = State?.HandleUpdate ();
            if (newState == null) return;
            State = newState;
            State.Enter();
        }

        private void FixedUpdate()
        {
            State?.HandleFixedUpdate();
        }

        private void OnCollisionEnter(Collision other)
        {
            State?.HandleCollisionEnter(other);
        }
    }
}