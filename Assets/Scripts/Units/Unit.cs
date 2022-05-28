using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Common;
using Components;
using Controls;
using CustomCamera;
using Data.Items;
using Data.Types;
using Data.UnitData;
using State;
using UnityEngine;
using Utils;
using Players;
using Pooling;
using Status;
using UI.HUD.EnemyHealthbar;
using UI.InGameShop;
using UI.Targeting;
using UnityEngine.Serialization;
using Utils.NotificationCenter;

namespace Units {
    public sealed class Unit : MonoBehaviour, IDamageable, IAbilityUser, IExperienceUser, IPoolable {
        public List<ItemType> PurchasedItems;
        public static Action<Unit> OnDeath = delegate { };
        public List<Renderer> Renderers { get; protected set; }
        public Player Owner { get; private set; }
        [FormerlySerializedAs("type")] public UnitType unitType;
        public Sprite Portrait { get; protected set; }
        public TargetingUIController UIController { get; protected set; }
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        public UnitState State { get; private set; }
        public Controller Controller { get; private set; }
        public InputModifierComponent InputModifierComponent { get; private set; }
        public AbilityComponent AbilityComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ExperienceComponent ExperienceComponent { get; private set; }
        public StatusComponent StatusComponent { get; private set; }
        public CoroutineHelper CoroutineHelper { get; private set; }
        public StatsComponent StatsComponent { get; private set; }
        public FundsComponent FundsComponent { get; private set; }
        public ItemDropComponent ItemDropComponent { get; private set; }
        public UnitData UnitData;
        private GameObject _owner;
        public InGameShopManager InGameShopManager { get; private set; }
        public bool Initialized { get; private set; } = false;
        public bool inUse { get; set; }
        GameObject IPoolable.Owner => gameObject;
        public string poolKey { get; set; }
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private EnemyHealthbar _enemyHealthbar;
        [field: SerializeField] public VCamFollowTarget _vcamFollowTarget; 

        public Unit Initialize(Player owner, UnitData data) {
            // properties & fields
            UnitData = data;
            Owner = owner;
            Portrait = data.visualAssets.portrait;
            Renderers = GetComponentsInChildren<Renderer>().ToList();
            PurchasedItems = new List<ItemType>();

            // Controller
            if (Controller == null) Controller = GetComponentInChildren<Controller>();
            Controller.Initialize(this);

            // Input Modifiers
            if (InputModifierComponent == null) InputModifierComponent = gameObject.AddComponent<InputModifierComponent>();
            InputModifierComponent  .Initialize(this);

            // RigidBody
            if (Rigidbody == null) Rigidbody = GetComponentInChildren<Rigidbody>();

            // Animator
            if (Animator == null) Animator = GetComponentInChildren<Animator>();

            // Stats -- must occur before abilities & health
            if (StatsComponent == null)  StatsComponent = gameObject.AddComponent<StatsComponent>();
            StatsComponent.Initialize(this, data.statsData);

            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>();
            HealthComponent.Initialize(this, data.health, StatsComponent);

            if (_enemyHealthbar != null) _enemyHealthbar.Initialize();

            // Abilities
            if (AbilityComponent == null) AbilityComponent = gameObject.AddComponent<AbilityComponent>();
            AbilityComponent.Initialize(this, data.abilities, StatsComponent);

            // Experience
            if (ExperienceComponent == null) ExperienceComponent = gameObject.AddComponent<ExperienceComponent>();
            ExperienceComponent.Initialize(this, data.experience);

            // CoroutineHelper
            if (CoroutineHelper == null) CoroutineHelper = gameObject.AddComponent<CoroutineHelper>();
            CoroutineHelper.Initialize(this);

            // Status 
            if (StatusComponent == null) StatusComponent = gameObject.AddComponent<StatusComponent>();
            StatusComponent.Initialize(this);

            // Funds
            if (FundsComponent == null) FundsComponent = gameObject.AddComponent<FundsComponent>();
            FundsComponent.Initialize(this, data.fundsData);

            // Targeting HUD 
            if (UIController == null) UIController = gameObject.AddComponent<TargetingUIController>();
            UIController.Initialize(this);

            // Item drops
            if (ItemDropComponent == null) ItemDropComponent = gameObject.AddComponent<ItemDropComponent>();
            ItemDropComponent.Initialize(this);

            // State
            State = StateHelper.StateFromEnum(data.state, this);

            if (InGameShopManager == null) InGameShopManager = FindObjectOfType<InGameShopManager>();

            Initialized = true;
            State.Enter();
            return this;
        }

        public void HandleExitFromPool() { }

        public void HandleReturnToPool() {
            Unsubscribe();
            if (StatsComponent != null) {
                StatusComponent.RemoveAllStatuses();
            }
            if (HealthComponent != null) {
                HealthComponent.ReinitializeHealth();
            }
            Initialized = false;
            Owner = null;
            UnitData = null;
        }

        private void Unsubscribe() {
            if (!Initialized) return;
            ExperienceComponent.Unsubscribe();
            StatusComponent.Unsubscribe();
            FundsComponent.Unsubscribe();
            UIController.Unsubscribe();
            ItemDropComponent.Unsubscribe();
            if (_enemyHealthbar != null) {
                _enemyHealthbar.Unsubscribe();
            }
        }

        private void OnDestroy() {
            if (!Initialized) return;
            ExperienceComponent.Unsubscribe();
            StatusComponent.Unsubscribe();
            FundsComponent.Unsubscribe();
            UIController.Unsubscribe();
            ItemDropComponent.Unsubscribe();
        }

        public Unit UpdateComponents() {
            Debug.Assert(Initialized);
            // stats component not updated to keep gains from level ups
            AbilityComponent.ReinitializeAbilities();
            HealthComponent.ReinitializeHealth();
            this.PostNotification(NotificationType.ComponentsDidUpdate);
            return this;
        }

        void Update() {
            if (!Initialized) return;

            // handle state
            var newState = State?.HandleUpdate(Controller.InputValues);
            if (newState == null) return;
            State.Exit();
            State = newState;
            State.Enter();
        }

        private void FixedUpdate() {
            if (!Initialized) return;
            State?.HandleFixedUpdate(Controller.InputValues);
        }

        private void OnCollisionEnter(Collision other) {
            if (!Initialized) return;
            State?.HandleCollisionEnter(other);
        }

        public void UnitDeath() {
            StartCoroutine(UnitDeathCrt());
        }

        private IEnumerator UnitDeathCrt() {
            OnDeath(this);
            Owner.RemoveUnit(this);
            var positionWithOffset = transform.position;
            positionWithOffset += Vector3.up*.5f;
            if (Owner.ControlType != ControlType.Ai) {
                MonoHelper.SpawnVfx(VfxType.PlayerDeath, positionWithOffset);
                var vfx = MonoHelper.SpawnVfx(VfxType.EnemyDeath, positionWithOffset);
                vfx.GetComponent<SetParticleData>().Initialize(_collider.radius);
            }
            else {
                this.PostNotification(NotificationType.DidKillEnemy);
                var vfx = MonoHelper.SpawnVfx(VfxType.EnemyDeath, positionWithOffset);
                vfx.GetComponent<SetParticleData>().Initialize(_collider.radius);
            }
            gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            ObjectPool.AddOrReturnInstanceToPool(poolKey,this);
        }

        public void OnLevelUp() {
            var spawnPos = new Vector3(transform.position.x, 0, transform.position.z);
            MonoHelper.SpawnVfx(VfxType.LevelUp, spawnPos).transform.SetParent(transform);
            this.PostNotification(NotificationType.DidLevelUp);
            StatsComponent.IncrementStat(StatType.Agility, 2);
            StatsComponent.IncrementStat(StatType.Endurance, 2);
            StatsComponent.IncrementStat(StatType.Intelligence, 2);
            StatsComponent.IncrementStat(StatType.Strength, 2);
            StatsComponent.IncrementStat(StatType.MovementSpeed, 2);
            UpdateComponents();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            State?.HandleDrawGizmos();
        }

        private void OnGUI() {
            if (!Initialized) return;

            if (Owner.ControlType == ControlType.Local) { }

            State?.HandleOnGUI();
        }
#endif
    }
}