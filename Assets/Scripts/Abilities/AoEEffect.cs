using System.Collections;
using System.Collections.Generic;
using Common;
using Enums;
using JetBrains.Annotations;
using Projectiles;
using UnityEngine;

namespace Abilities {
    public class AoEEffect {
        public ColliderParams ColliderParams;

        public List<ControlType> AffectedFactions;

        private bool ShouldActivate() => false;

        private IEnumerator Activate() {
            yield break;
        }
        
        // private void OnTriggerEnter(Collider other) {
        //     if (!ShouldActivate(other, out var rigidBody)) return;
        //
        //     Debug.Log($"{other.gameObject.name} will be {(Force < 0 ? "pushed" : "pulled")}!");
        //     // StartCoroutine(Strategy(other, rigidBody, Force, transform));
        // }
        //
        // private bool ShouldActivate(Collider other, [CanBeNull] out Rigidbody rigidBody) {
        //     rigidBody = null;
        //     var unit = other.transform.root.GetComponentInChildren<Unit>();
        //     if (unit == null) return false;
        //
        //     if (AffectedFactions.All(x => x != unit.Owner.ControlType)) {
        //         Debug.Log($"Unable to affect {unit.name} because their faction is {unit.Owner.ControlType}");
        //         return false;
        //     }
        //
        //     rigidBody = other.transform.root.GetComponent<Rigidbody>();
        //     if (rigidBody == null) {
        //         Debug.Log($"Unable to affect {unit.name} because they do not posses a rigidbody");
        //         return false;
        //     }
        //
        //     return true;
        // }
    }
}