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
        private void Start() {
            _seq = DOTween.Sequence()
                .AppendCallback(() => MonoHelper.SpawnVfx(VfxType.Poof, Owner.transform.position.WithoutY()))
                .AppendInterval(0.1f)
                .AppendCallback(() => {
                    Owner.Renderers.ForEach(r => {
                        mat = r.materials[1];
                        r.material.SetFloat("_FresnelPower", 0.5f);
                        r.materials = new[] {r.materials[0]};
                    });
                }).SetAutoKill(false).Pause();
        }

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            Debug.Log("Handling activation of Conceal");
            Debug.Log("Concealed!");
            float timeLeft = Duration;
            brokenConcealment = false;
            _seq.Restart();

            this.AddObserver(BreakConcealment, NotificationType.AbilityDidActivate);

            Owner.StatusComponent.AddStatus(StatusType.Hidden);

            OnAbilityActivationFinished(Owner, this);

            var modifiers = Owner.AbilityComponent.Modifiers;
            var markModifier = new MarkOnHitModifier(null);
            modifiers.Add(markModifier);
            modifiers.Add(new DoubleDamageModifier(null));

            while (timeLeft > 0f && Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden)) {
                if (brokenConcealment) break;

                timeLeft -= Time.deltaTime;
                yield return null;
            }

            if (Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden))
                Owner.StatusComponent.RemoveStatus(StatusType.Hidden);

            if (modifiers.Contains(markModifier)) modifiers.Remove(markModifier);

            this.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate);
            Owner.Renderers.ForEach(r => {
                r.material.SetFloat("_FresnelPower", 0f);
                r.materials = new[] {r.materials[0], mat};
            });
            Debug.Log("Finished Concealment");
        }

        private void OnDestroy() {
            _seq?.Kill();
        }

        private bool brokenConcealment = false;
        private Material mat;
        private Sequence _seq;

        void BreakConcealment(object sender, object args) {
            if (ReferenceEquals(args, this)) return;
            brokenConcealment = true;
            Owner.Renderers.ForEach(r => {
                r.material.SetFloat("_FresnelPower", 0f);
                r.materials = new[] {r.materials[0], mat};
            });
        }
    }
}