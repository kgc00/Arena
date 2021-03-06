﻿using System.Collections;
using System.Collections.Generic;
using Arena;
using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.Types;
using Modifiers.SpawnModifiers;
using UnityEngine;
using Utils;

namespace Spawner {
    public class WaveHandler {
        public HordeSpawnData hordeSpawnData { get; private set; }
        private WaveSpawnData currentWave;
        public int currentIndex { get; private set; }
        private Spawner owner;

        public WaveHandler(HordeSpawnData table, Spawner owner) {
            hordeSpawnData = table;
            currentIndex = 0;
            currentWave = table.Waves[currentIndex];
            this.owner = owner;
        }

        public IEnumerator Spawn(float spawnStartupTime, float delayBetweenUnits) {
            Vector3 spawnerPos = owner.transform.position;
            Vector3 extentNegative = spawnerPos - owner.Bounds / 2;
            Vector3 extentPositive = spawnerPos + owner.Bounds / 2;

            var wave = SpawnDataSmith.ModifyWaveData(currentWave);

            foreach (UnitSpawnData spawnData in wave.wave) {
                Debug.Log($"Spawning {spawnData.Amount} {spawnData.Unit}");
                for (int i = 0; i < spawnData.Amount; i++) {
                    var spawnPos = GetRandomSpawnPos(extentNegative, extentPositive);

                    var spawnVfx = MonoHelper.SpawnVfx(VfxType.EnemySpawnIndicator, spawnPos);
                    var unitData = DataHelper.DataFromUnitType(spawnData.Unit);

                    owner.StartSpawnCoroutine(spawnStartupTime, spawnVfx, () => owner.owningPlayer.InstantiateUnit(
                        SpawnHelper.PrefabFromUnitType(spawnData.Unit),
                         SpawnDataSmith.ModifyUnitData(unitData, spawnData.modifiers),
                        pos: spawnPos
                    ));

                    yield return new WaitForSeconds(delayBetweenUnits);
                }
            }
        }

        public void IncrementIndex() {
            currentIndex++;
            if (currentIndex <= hordeSpawnData.Waves.Count -1) {
                currentWave = hordeSpawnData.Waves[currentIndex];
            }
        }

        private static Vector3 GetRandomSpawnPos(Vector3 extentNegative, Vector3 extentPositive) {
            var x = Random.Range(extentNegative.x, extentPositive.x);
            var y = 1.0f;
            var z = Random.Range(extentNegative.z, extentPositive.z);
            var spawnPos = new Vector3(x, y, z);
            return spawnPos;
        }
    }
}