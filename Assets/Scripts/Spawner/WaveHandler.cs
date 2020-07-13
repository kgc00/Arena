using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.Types;
using Modifiers.SpawnModifiers;
using UnityEngine;
using Utils;

namespace Spawner {
    public class WaveHandler {
        private HordeSpawnData hordeSpawnData;
        private WaveSpawnData currentWave;
        private int currentIndex;
        private Spawner owner;
        public List<WaveModifier> waveModifications;
        public List<UnitModifier> unitModifications;

        public WaveHandler(HordeSpawnData table, Spawner owner) {
            hordeSpawnData = table;
            currentIndex = 0;
            currentWave = table.Waves[currentIndex];
            waveModifications = new List<WaveModifier>();
            unitModifications = new List<UnitModifier>();
            this.owner = owner;
        }

        public IEnumerator Spawn(float spawnStartupTime, float delayBetweenUnits) {
            Vector3 spawnerPos = owner.transform.position;
            Vector3 extentNegative = spawnerPos - owner.Bounds / 2;
            Vector3 extentPositive = spawnerPos + owner.Bounds / 2;

            var wave = SpawnDataSmith.ModifyWaveData(currentWave.CreateInstance(), waveModifications);

            foreach (UnitSpawnData table in wave.wave) {
                Debug.Log($"Spawning {table.Amount} {table.Unit}");
                for (int i = 0; i < table.Amount; i++) {
                    var spawnPos = GetRandomSpawnPos(extentNegative, extentPositive);

                    var spawnVfx = MonoHelper.SpawnVfx(VfxType.EnemySpawnIndicator, spawnPos);

                    owner.StartSpawnCoroutine(spawnStartupTime, spawnVfx, () => owner.OwningPlayer.InstantiateUnit(
                        SpawnHelper.PrefabFromUnitType(table.Unit),
                        SpawnDataSmith.ModifyUnitData(DataHelper.DataFromUnitType(table.Unit), unitModifications),
                        pos: spawnPos
                    ));

                    yield return new WaitForSeconds(delayBetweenUnits);
                }
            }

            // no waves left ? break
            if (hordeSpawnData.Waves.Count - 1 <= currentIndex) yield break;

            currentIndex++;
            currentWave = hordeSpawnData.Waves[currentIndex];
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