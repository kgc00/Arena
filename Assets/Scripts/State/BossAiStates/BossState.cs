using Units;
using UnityEngine;

namespace State.BossAiStates {
    public class BossState : UnitState {
        protected BossState(Unit owner) : base(owner) { }
        public override void HandleOnGUI() {
            // GUILayout.Box(GetType().ToString());
        }
    }
}