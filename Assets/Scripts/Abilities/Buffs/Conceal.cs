using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Data.AbilityData;
using Data.Types;
using DG.Tweening;
using Extensions;
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
        private float _startingMoveSpeed;      
        private bool _concealed;

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
        }

        private void EnableDoubleMovementSpeed(object sender, object args) {
            _doubleMovementSpeed = true;
        }

        private void OnDisable() {this.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate); }

        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            if (_concealed) yield break;
            _concealed = true;
            Debug.Log("Handling activation of Conceal");
            Debug.Log("Concealed!");
            float timeLeft = Duration;
            _brokenConcealment = false;
            _seq.Restart();

            this.AddObserver(BreakConcealment, NotificationType.AbilityDidActivate);

            Owner.StatusComponent.AddStatus(StatusType.Hidden);

            OnAbilityActivationFinished(Owner, this);

            // var modifiers = Owner.AbilityComponent.GlobalAbilityModifiers;
            // var markModifier = new MarkOnHitModifier(null);
            // modifiers.Add(markModifier);
            
            if (_doubleMovementSpeed)
            {
                _startingMoveSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
                Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, _startingMoveSpeed);
            }

            while (timeLeft > 0f && Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden)) {
                if (_brokenConcealment) break;

                timeLeft -= Time.deltaTime;
                yield return null;
            }

            if (Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden))
                Owner.StatusComponent.RemoveStatus(StatusType.Hidden);

            // if (modifiers.Contains(markModifier)) modifiers.Remove(markModifier);

            this.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate);
            Owner.Renderers.ForEach(r => {
                r.material.SetFloat(FresnelPower, 0f);
                r.materials = new[] {r.materials[0], _mat};
            });
            Debug.Log("Finished Concealment");
            foreach (var cb in OnAbilityFinished) cb(Owner, this);
            if (_doubleMovementSpeed) {
                Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, Owner.StatsComponent.Stats.MovementSpeed.Value - _startingMoveSpeed);
            }

            _concealed = false;
        }

        private void OnDestroy() {
            _seq?.Kill();
        }

        void BreakConcealment(object sender, object args) {
            if (this != null && ReferenceEquals(args, this)) return;
            _brokenConcealment = true;
            Owner.Renderers.ForEach(r => {
                r.material.SetFloat(FresnelPower, 0f);
                r.materials = new[] {r.materials[0], _mat};
            });
            if (_doubleMovementSpeed) {
                Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, Owner.StatsComponent.Stats.MovementSpeed.Value - _startingMoveSpeed);
            }
            _concealed = false;
        }
    }
}