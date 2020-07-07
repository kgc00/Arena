using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utils;
using System.Linq;
using Enums;
using Spawner.Data;
using Units.Data;
using Random = UnityEngine.Random;
using Players;
using State;
using Units.Modifiers;
using UnityEditor;

namespace Spawner {
    public class Spawner : MonoBehaviour {
        #region Properties

        [Header("Center")] [Range(-20f, 20f), SerializeField]
        private float xPos = 0f;

        [Range(-20f, 20f), SerializeField] private float zPos = 0f;

        [Header("Size")] [Range(-25f, 25f), SerializeField]
        private float xModifier = 0f;

        [Range(-25f, 25f), SerializeField] private float zModifier = 0f;
        [Range(1f, 50f), SerializeField] private float size = 48f;

        [Header("Data")] [SerializeField] public Intervals interval;
        [SerializeField] public SpawnTable table;
        public Vector3 Bounds { get; private set; }
        [SerializeField] public Interval Interval { get; private set; }
        [SerializeField] public WaveHandler Handler { get; private set; }
        public Player OwningPlayer { get; private set; }

        #endregion

        private void OnEnable() {
            transform.position = new Vector3(xPos, 0, zPos);
            Bounds = new Vector3(size + xModifier, 1f, size + zModifier);
            // WILL BREAK IF WE ADD MORE THAN ONE AI PLAYER
            OwningPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.ControlType == ControlType.Ai);
            if (Handler == null) Handler = new WaveHandler(table, this);
            if (Interval == null)
                Interval = SpawnHelper.IntervalFromType(interval, gameObject)
                    .Initialize(() => Handler.Spawn(2f, 0), this, 10f, 0f, 2f);

            StartCoroutine(Handler.Spawn(2f, 0));
        }

        public Coroutine StartSpawnCoroutine(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) => StartCoroutine(SpawnCr(delay, spawnVfx, spawnUnit));

        private IEnumerator SpawnCr(float delay, GameObject spawnVfx, Func<Unit> spawnUnit) {
            yield return new WaitForSeconds(delay);
            Destroy(spawnVfx);

            spawnUnit();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            transform.position = new Vector3(xPos, 0, zPos);
            Vector3 debugPos = new Vector3(size + xModifier, 1f, size + zModifier);
            Gizmos.DrawWireCube(
                transform.position,
                debugPos
            );
        }
#endif
    }
}