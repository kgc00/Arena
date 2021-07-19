using UnityEngine;
using System.Linq;
using Data.Types;
using Players;
using System.Collections.Generic;

namespace Units {
    public class DebugTools : MonoBehaviour {

        public void KillEnemies() {
            var units = FindObjectsOfType<Unit>();
            foreach (var u in units) {
                if (u.Owner.ControlType == ControlType.Ai)
                    u.HealthComponent.DamageOwner(99);
            }
        }

        private void OnGUI() {

            GUILayout.BeginArea(new Rect(new Vector2(250, 15), new Vector2(150, 300)));
            if (GUILayout.RepeatButton("kill")) {
                KillEnemies();
            };
            GUILayout.EndArea();

        }

    }
}

/*
Serialization depth limit 10 exceeded at 'Common::ScriptableObjectModifier`1.Model'. There may be an object composition cycle in one or more of your serialized classes.

Serialization hierarchy:
11: Common::ScriptableObjectModifier`1.Model
10: Common::ScriptableObjectModifier`1.Next
9: Common::ScriptableObjectModifier`1.Next
8: Common::ScriptableObjectModifier`1.Next
7: Common::ScriptableObjectModifier`1.Next
6: Common::ScriptableObjectModifier`1.Next
5: Common::ScriptableObjectModifier`1.Next
4: Common::ScriptableObjectModifier`1.Next
3: Common::ScriptableObjectModifier`1.Next
2: Common::ScriptableObjectModifier`1.Next
1: Common::ScriptableObjectModifier`1.Next
*/