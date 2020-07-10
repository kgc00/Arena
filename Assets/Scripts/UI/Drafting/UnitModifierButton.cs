using System;
using Common;
using Data.SpawnData;
using Data.UnitData;
using JetBrains.Annotations;
using Modifiers.SpawnModifiers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Drafting {
    public sealed class UnitModifierButton : MonoBehaviour {
        public WaveVisualizerWrapper Owner { get; private set; }
        public UnitData Model { get; private set; }
        public UnitModifier Modifier { get; private set; }
        public bool Initialized { get; private set; }
        public Image image;

        private void Awake() {
            image = gameObject.GetComponentInChildren<Image>() ??
                     throw new Exception($"Unable find Image component on {name}");
        }

        public UnitModifierButton Initialize(UnitData model, UnitModifier modifier, WaveVisualizerWrapper o) {
            Owner = o;
            Model = model;
            Modifier = modifier;
            image.sprite = Resources.Load<Sprite>(Modifier.IconAssetPath);
            Initialized = true;
            return this;
        }
    }
}