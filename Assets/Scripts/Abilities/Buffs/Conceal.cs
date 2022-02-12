using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Data.AbilityData;
using Data.Types;
using DG.Tweening;
using Extensions;
using State;
using State.PlayerStates;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.Buffs {
    public class Conceal : BuffAbility {
        private bool _brokenConcealment = false;
        private Material _mat;
        private Sequence _seq;
        private static readonly int FresnelPower = Shader.PropertyToID("_FresnelPower");
        private bool _doubleMovementSpeed;
        private int _startingMoveSpeed;      
        private bool _concealed;
        private bool _persistentAddMarkOnHitModifier;
        private MarkOnHitModifier _currentMarkModifier;
        private List<AbilityModifier> _globalAbilityModifiers;
        
        private void Start() {
            _seq = DOTween.Sequence()
                .AppendCallback(() => MonoHelper.SpawnVfx(VfxType.Poof, Owner.transform.position.WithoutY()))
                .AppendInterval(0.1f)
                .AppendCallback(() => {
                    Owner.Renderers.ForEach(r => {
                        _mat = r.materials[1];
                        r.material.SetFloat(FresnelPower, 0.5f);
                        r.materials = new[] {r.materials[0]};
                    });
                }).SetAutoKill(false).Pause();
            NotificationCenter.instance.AddObserver(EnableDoubleMovementSpeed, NotificationType.EnableDoubleMovementSpeed);
            NotificationCenter.instance.AddObserver(EnablePersistentAddMarkOnHit, NotificationType.EnableConcealPersistentAddMarkOnHit);
        }

        private void OnDisable() {
            NotificationCenter.instance.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate);
            NotificationCenter.instance.RemoveObserver(EnableDoubleMovementSpeed, NotificationType.EnableDoubleMovementSpeed);
            NotificationCenter.instance.RemoveObserver(EnablePersistentAddMarkOnHit, NotificationType.EnableConcealPersistentAddMarkOnHit);
        }
        private void EnableDoubleMovementSpeed(object sender, object args) {
            _doubleMovementSpeed = true;
        }
        
        private void EnablePersistentAddMarkOnHit(object sender, object args) {
            _persistentAddMarkOnHitModifier = true;
        }

        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            if (_concealed) yield break;
            _concealed = true;
            Debug.Log("Handling activation of Conceal");
            Debug.Log("Concealed!");
            float timeLeft = Duration;
            _brokenConcealment = false;
            _seq.Restart();

            NotificationCenter.instance.AddObserver(BreakConcealment, NotificationType.AbilityDidActivate);

            Owner.StatusComponent.AddStatus(StatusType.Hidden, Duration, 1);

            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
            Cooldown.Freeze();

            if (_persistentAddMarkOnHitModifier) {
                _globalAbilityModifiers = Owner.AbilityComponent.GlobalAbilityModifiers;
                _currentMarkModifier = new MarkOnHitModifier(null);
                _globalAbilityModifiers.Insert(0, _currentMarkModifier);
            }   
            
            if (_doubleMovementSpeed) {
                _startingMoveSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
                Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, _startingMoveSpeed);
            }

            while (timeLeft > 0f && Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden)) {
                if (_brokenConcealment) {
                    this.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate);
                    yield break;
                }
                timeLeft -= Time.deltaTime;
                yield return null;
            }

            // must be here so that the modifier is not removed before the ability finished completion
            // can move this into HandleBreakConcealment when the notification type is AbilityCompleted... not sure it's hooked up
            if (_persistentAddMarkOnHitModifier && _globalAbilityModifiers.Contains(_currentMarkModifier)) {
                _globalAbilityModifiers.Remove(_currentMarkModifier);
            }
            
            HandleBreakConcealment();
        }

        void BreakConcealment(object sender, object args) {
            if (!(args is UnitIntent playerIntent)) return;
            var isPlayerActivatedAbility = UnityEngine.Object.Equals(playerIntent.ability.Owner, Owner);
            if (this == null || !isPlayerActivatedAbility) return;
            
            HandleBreakConcealment();
        }

        private void HandleBreakConcealment() {
            Owner.StatusComponent.RemoveStatus(StatusType.Hidden);
            
            Owner.Renderers.ForEach(r => {
                r.material.SetFloat(FresnelPower, 0f);
                r.materials = new[] {r.materials[0], _mat};
            });
            
            Cooldown.UnFreeze();

            if (_doubleMovementSpeed) {
                Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, Owner.StatsComponent.Stats.MovementSpeed.Value - _startingMoveSpeed);
            }

            _concealed = false;
            _brokenConcealment = true;
        }

        private void OnDestroy() {
            _seq?.Kill();
        }
    }
}