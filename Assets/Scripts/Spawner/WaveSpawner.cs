using System;
using System.Collections;
using Data;
using Data.SpawnData;
using Data.Types;
using Modifiers.SpawnModifiers;
using Units;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Spawner {
    public class WaveSpawner {
        public readonly SpawnManager Owner;

        public WaveSpawner(SpawnManager owner) {
            Owner = owner;
        }

        public IEnumerator Spawn(WaveSpawnData waveToSpawn, float spawnStartupTime, float delayBetweenUnits,
            Action<float, GameObject, Func<Unit>> startSpawnCoroutine) {
            Vector3 spawnerPos = Owner.transform.position;
            Vector3 extentNegative = spawnerPos - Owner.Bounds / 2;
            Vector3 extentPositive = spawnerPos + Owner.Bounds / 2;

            var wave = SpawnDataSmith.ModifyWaveData(waveToSpawn);

            // spawn all units in wave at once
            foreach (UnitSpawnData spawnData in wave.wave) {
                for (int i = 0; i < spawnData.Amount; i++) {
                    var spawnPos = GetRandomSpawnPos(extentNegative, extentPositive);
                    var vfxGameObject = MonoHelper.SpawnVfx(VfxType.EnemySpawnIndicator, spawnPos);
                    var unitData = DataHelper.DataFromUnitType(spawnData.Unit);
                    Unit UnitSpawnFunction() => Owner.owningPlayer.InstantiateUnit(
                        SpawnHelper.PrefabFromUnitType(spawnData.Unit),
                        SpawnDataSmith.ModifyUnitData(unitData, spawnData.modifiers), spawnPos);
                    
                    // (float delay, GameObject vfx, Func<Unit> unitPrefab)
                    startSpawnCoroutine(spawnStartupTime, vfxGameObject, UnitSpawnFunction);
                    yield return new WaitForSeconds(delayBetweenUnits);
                }
            }
        }

        private static Vector3 GetRandomSpawnPos(Vector3 extentNegative, Vector3 extentPositive) {
            var x = Random.Range(extentNegative.x, extentPositive.x);
            var y = 0f;
            var z = Random.Range(extentNegative.z, extentPositive.z);
            var spawnPos = new Vector3(x, y, z);
            return spawnPos;
        }
    }
}