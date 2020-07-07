using System.Collections;
using System.Collections.Generic;
using Spawner.Data;
using Units.Data;
using Units.Modifiers;
using UnityEngine;
using Utils;
using Types = VFX.Types;

namespace Spawner {
    public class WaveHandler {
        private SpawnTable spawnTable;
        private WaveTable currentWave;
        private int currentIndex;
        private Spawner owner;

        public WaveHandler(SpawnTable table, Spawner owner) {
            spawnTable = table;
            currentIndex = 0;
            currentWave = table.Waves[currentIndex];
            this.owner = owner;
        }

        public IEnumerator Spawn(float spawnStartupTime, float delayBetweenUnits) {
            Vector3 spawnerPos = owner.transform.position;
            Vector3 extentNegative = spawnerPos - owner.Bounds / 2;
            Vector3 extentPositive = spawnerPos + owner.Bounds / 2;

            var wave = ModifyWaveData(currentWave.CreateInstance());

            foreach (UnitTable table in wave.Wave) {
                Debug.Log($"Spawning {table.Amount} {table.Unit}");
                for (int i = 0; i < table.Amount; i++) {
                    var spawnPos = GetRandomSpawnPos(extentNegative, extentPositive);

                    var spawnVfx = MonoHelper.SpawnVfx(Types.EnemySpawnIndicator, spawnPos);

                    owner.StartSpawnCoroutine(spawnStartupTime, spawnVfx, () => owner.OwningPlayer.InstantiateUnit(
                        SpawnHelper.PrefabFromUnitType(table.Unit),
                        ModifyUnitData(SpawnHelper.DataFromUnitType(table.Unit)),
                        pos: spawnPos
                    ));

                    yield return new WaitForSeconds(delayBetweenUnits);
                }
            }

            // no waves left ? break
            if (spawnTable.Waves.Count - 1 <= currentIndex) yield break;

            currentIndex++;
            currentWave = spawnTable.Waves[currentIndex];
        }

        private static Vector3 GetRandomSpawnPos(Vector3 extentNegative, Vector3 extentPositive) {
            var x = Random.Range(extentNegative.x, extentPositive.x);
            var y = 1.0f;
            var z = Random.Range(extentNegative.z, extentPositive.z);
            var spawnPos = new Vector3(x, y, z);
            return spawnPos;
        }

        UnitData ModifyUnitData(UnitData instance) {
            var root = new UnitDataModifier().InitializeModifier(instance);

            var modifiers = new List<UnitDataModifier>();

            for (int i = 0; i < modifiers.Count; i++) {
                root.Add(modifiers[i].InitializeModifier(instance));
            }

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return instance;
        }

        WaveTable ModifyWaveData(WaveTable instance) {
            var root = new WaveTableModifier().InitializeModifier(instance);

            var modifiers = new List<WaveTableModifier>();

            for (int i = 0; i < modifiers.Count; i++) {
                root.Add(modifiers[i].InitializeModifier(instance));
            }

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return instance;
        }
    }
}