using UnityEngine;
using System.Linq;
using Data.Types;
using Players;
using System.Collections.Generic;
using UI;

namespace Units {
    public class DebugTools : MonoBehaviour {
#if UNITY_EDITOR
        
        public void KillEnemies() {
            var units = FindObjectsOfType<Unit>();
            foreach (var u in units) {
                if (u.Owner.ControlType == ControlType.Ai)
                    u.HealthComponent.DamageOwner(99);
            }
        }

        public void HealPlayer() {
            var units = FindObjectsOfType<Unit>();
            foreach (var u in units) {
                if (u.Owner.ControlType == ControlType.Local)
                    u.HealthComponent.Refill();
            }
        }

        public void AddExperience() {
            var units = FindObjectsOfType<Unit>();
            foreach (var u in units) {
                if (u.Owner.ControlType == ControlType.Local)
                    u.ExperienceComponent.AwardBounty(15);
            }
        }

        private void AddGold() {
            var units = FindObjectsOfType<Unit>();
            foreach (var u in units) {
                if (u.Owner.ControlType == ControlType.Local)
                    u.FundsComponent.AddFunds(300);
            }
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(new Vector2(Screen.width - 185, 15), new Vector2(150, 300)));
            if (GUILayout.RepeatButton("kill enemies")) {
                KillEnemies();
            }

            ;
            if (GUILayout.RepeatButton("heal player")) {
                HealPlayer();
            }

            ;
            if (GUILayout.RepeatButton("award xp")) {
                AddExperience();
            }

            ;
            if (GUILayout.RepeatButton("award money")) {
                AddGold();
            }

            ;
            GUILayout.EndArea();
        }
    }
#endif
}