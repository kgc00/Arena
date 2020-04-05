using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utils;
using  System.Linq;
using Enums;
using Spawner.Data;
using Units.Data;
using Random = UnityEngine.Random;

namespace Spawner
{

    public enum Intervals
    {
        Timer,
        WaveLastEnemyAlive,
        WaveCertainEnemyDies
    }
    
    public interface IInterval
    {
        bool Enabled { get; }
        void Enable();
        void Disable();
        void HandleUpdate();
    }

    public interface ITimerInterval : IInterval
    {
        float SpawnInterval { get; }
        float TimeSinceLastSpawn { get; }
    }
    
    public abstract class Interval : MonoBehaviour
    {
        public IInterval IntervalType { get; private set; }
        public Spawner Owner { get; private set; }
        public Action Spawn { get; private set; }

        public Interval Initialize(Action spawn, Spawner owner)
        {
            Spawn = spawn;
            Owner = owner;
            return this;
        }
    }
    
    public class TimerInterval : Interval, ITimerInterval
    {
        public bool Enabled { get; private set; }
        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;

        public float SpawnInterval { get; private set; } = 6f;
        public float TimeSinceLastSpawn { get; private set;}

        private void OnEnable()
        {
            // Enabled = true;
            Enabled = false;
        }

        public void Update()
        {
            HandleUpdate();
        }

        public void HandleUpdate()
        {
            if (!Enabled) return;

            TimeSinceLastSpawn += Time.deltaTime;

            if (TimeSinceLastSpawn < SpawnInterval) return;
            
            Spawn();
            TimeSinceLastSpawn = 0f;
        }
    }


    public class WaveHandler
    {
        private SpawnTable spawnTable;
        private WaveTable current;
        private Spawner owner;

        public WaveHandler(SpawnTable table, Spawner owner)
        {
            spawnTable = table;
            current =  table.Waves[0];
            this.owner = owner;
        }

        public void Spawn()
        {
            Vector3 spawnerPos = owner.transform.position;
            Vector3 extentNegative = spawnerPos - owner.Bounds/2;
            Vector3 extentPositive = spawnerPos + owner.Bounds/2;
            foreach (UnitTable unitTable in current.Wave)
            {
                Debug.Log($"Spawning {unitTable.Amount} {unitTable.Unit}");
                for (int i = 0; i < unitTable.Amount; i++)
                {
                    var x = Random.Range(extentNegative.x, extentPositive.x);
                    var y = 1.0f;
                    var z = Random.Range(extentNegative.z, extentPositive.z);
                    var spawnPos = new Vector3(x,y,z);
                    owner.OwningPlayer.InstantiateUnit(
                        SpawnHelper.PrefabFromUnitType(unitTable.Unit),
                        unitData: SpawnHelper.DataFromUnitType(unitTable.Unit),
                        pos: spawnPos
                    );
                }
            }
        }

        
    }

    public class Spawner : MonoBehaviour
    {
        #region Properties
        [Header("Center")]
        [Range(-20f, 20f), SerializeField] private float xCenter = 0f;
        [Range(-20f, 20f), SerializeField] private float zCenter = 0f;
        [Header("Size")]
        [Range(-25f, 25f), SerializeField] private float xOffset = 0f;
        [Range(-25f, 25f), SerializeField] private float zOffset = 0f;
        [Range(1f, 50f), SerializeField] private float size = 48f;
        public Vector3 Bounds { get; private set; }
        [SerializeField] private Interval interval;
        [SerializeField] private WaveHandler handler;
        public Player OwningPlayer { get; private set; }
        #endregion

        private void OnEnable()
        {
            transform.position = new Vector3(xCenter, 0, zCenter);
            Bounds = new Vector3(size+ xOffset, 1f, size + zOffset);
            // WILL BREAK IF WE ADD MORE THAN ONE AI PLAYER
            OwningPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.ControlType == ControlType.Ai);
            if (handler == null) handler = new WaveHandler(Resources.Load<SpawnTable>("Data/Spawns/SpawnTable"), this);
            if (interval == null) interval = gameObject.AddComponent<TimerInterval>().Initialize(handler.Spawn, this);
            
            handler.Spawn();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            transform.position = new Vector3(xCenter, 0, zCenter);
            Vector3 debugPos = new Vector3(size + xOffset, 1f, size + zOffset);
            Gizmos.DrawWireCube(
                transform.position,
                debugPos
            );
        }
#endif
    }
}