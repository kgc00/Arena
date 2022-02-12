﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Pickups;
using Data.Types;
using Units;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Components {
    public class ItemDropComponent : MonoBehaviour {
        private List<DropData> _drops;
        public Unit Owner { get; private set; }

        public ItemDropComponent Initialize(Unit unit) {
            Owner = unit;
            _drops = Owner.unitType switch {
                UnitType.Melee => new List<DropData> {new DropData(DropType.HealthPickupSmall, 1)},
                UnitType.Charging => new List<DropData> {new DropData(DropType.HealthPickupLarge, 1)},
                UnitType.Ranged => new List<DropData> {new DropData(DropType.HealthPickupSmall, 3)},
                UnitType.Boss => new List<DropData> {new DropData(DropType.HealthPickupLarge, 3)},
                _ => new List<DropData>()
            };

            return this;
        }

        public IEnumerator SpawnDrops() {
            var typesToSpawn =
                (from drop in _drops
                    let roll = Random.Range(0, 100f)
                    where roll <= drop.dropRate
                    select drop.dropType).ToList();
            if (typesToSpawn.Count == 0) yield break;
            MonoHelper.SpawnVfx(VfxType.DropSpawn, transform.position);
            yield return new WaitForSeconds(1f); // hardcoded for now
            typesToSpawn.ForEach(type => MonoHelper.SpawnDrop(type, transform.position));
        }
    }
}