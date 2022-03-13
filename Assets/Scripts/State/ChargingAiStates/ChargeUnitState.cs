using System.Collections;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Data.Types;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace State.ChargingAiStates {
    public class ChargeUnitState : AbilityUnitState<Charge> {
        private readonly Transform _playerTransform;
        private static readonly int Charging = Animator.StringToHash("Charging");
        OrcSlash orcSlash;
        private Vector3 _chargeTargetPosition;

        public ChargeUnitState(Unit owner, Transform targetTransform) : base(owner) {
            _playerTransform = targetTransform;
            orcSlash = Owner.AbilityComponent.GetEquippedAbility<OrcSlash>();
            this.AddObserver(HandleDidCollide, NotificationType.ChargeDidImpactWall);
        }
        ~ChargeUnitState() => this.RemoveObserver(HandleDidCollide, NotificationType.ChargeDidImpactWall);
        private void HandleDidCollide(object sender, object args) {
            if (!UnityEngine.Object.Equals(sender, Ability)) {
                return;
            }
            Owner.StatusComponent.AddStatus(StatusType.Stunned,2f, 1);
        }

        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator || _playerTransform == null) {
                AbilityFinished = true;
                yield break;
            }

            Owner.Animator.SetTrigger(Charging);
            _chargeTargetPosition = _playerTransform.position;
            yield return Owner.AbilityComponent.Activate(ref Ability, _chargeTargetPosition);
        }

        protected override void HandleAbilityFinished(Unit u, Ability a) {
            Owner.CoroutineHelper.StartCoroutine(ExecuteWaitForCooldown());
        }

        IEnumerator ExecuteWaitForCooldown() {
            if (Owner.Animator != null) {
                Owner.Animator.ResetTrigger(Charging);
            }
            
            yield return new WaitForSeconds(0.5f);
            base.HandleAbilityFinished(Owner, Ability);
        }

        public override UnitState HandleUpdate(InputValues input) {
            UnitState state = null;

            if (Owner.StatusComponent.IsStunned()) {
                if (Owner.Animator != null) {
                    Owner.Animator.ResetTrigger(Charging);
                }
                return new StunUnitState(Owner);
            }
            
            if (!AbilityFinished) return state;

            if (_playerTransform == null) return new IdleUnitState(Owner);
            
            var distanceToUnit = Vector3.Distance(_playerTransform.position, Owner.transform.position);
            var isWithinSlashRange = distanceToUnit <= orcSlash.Range;

            if (ShouldEnterAttack(ref state, isWithinSlashRange)) {
                return state;
            }

            if (ShouldEnterChase(ref state, isWithinSlashRange)) {
                return state;
            }

            if (ShouldEnterIdle(ref state)) {
                return state;
            }

            return null;
        }


        private bool ShouldEnterAttack([CanBeNull] ref UnitState unitState, bool isWithinAttackRange) {
            if (isWithinAttackRange && !orcSlash.Cooldown.IsOnCooldown) {
                unitState = new OrcSlashState(Owner, _playerTransform);
                return true;
            }

            return false;
        }

        private bool ShouldEnterIdle([CanBeNull] ref UnitState unitState) {
            var playerIsHidden = false;
            if (_playerTransform.TryGetComponent(out Unit unit)) {
                playerIsHidden = !unit.StatusComponent.IsVisible();
            }

            if (_playerTransform != null && !playerIsHidden) return false;
            unitState = new IdleUnitState(Owner);
            return true;
        }

        private bool ShouldEnterChase([CanBeNull] ref UnitState unitState, bool isWithinAttackRange) {
            if (isWithinAttackRange) return false;
            unitState = new ChaseUnitState(Owner, _playerTransform);
            return true;
        }


        public override void HandleFixedUpdate(InputValues input) {
            if (_playerTransform == null || Owner.transform == null || AbilityFinished) return;
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation() {
            var transform = Owner.transform;
            var heading = (_chargeTargetPosition - transform.position).normalized;
            Owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(heading),
                Time.deltaTime * 20f);
        }
    }
}