using System;
using Abilities;
using Data.Types;
using UnityEngine;

namespace State.PlayerStates {
    [Serializable]
    public class PlayerIntent {
        [SerializeField] public Ability ability;
        [SerializeField] public Vector3 targetLocation;
        [SerializeField] public ButtonType buttonType;
        public PlayerIntent(Ability ability, Vector3 targetLocation, ButtonType buttonType) {
            this.ability = ability;
            this.targetLocation = targetLocation;
            this.buttonType = buttonType;
        }
    }
}