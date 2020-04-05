using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Utils
{
    public class CoroutineHelper : MonoBehaviour
    {
        public Unit Owner { get; private set; }
        public List<Coroutine> Coroutines { get; private set; }

        public Coroutine SpawnCoroutine(IEnumerator routine)
        {
            Coroutine coroutine = StartCoroutine(routine);
            Coroutines.Add(coroutine);
            return coroutine;
        }

        public CoroutineHelper Initialize(Unit unit)
        {
            Owner = unit;
            Coroutines = new List<Coroutine>();
            return this;
        }

        private void OnDisable() {
            if (Coroutines != null)
            {
                foreach (var routine in Coroutines)
                    if (routine != null) Stop(routine);
            }
        }

        public void Stop (Coroutine routine)
        {
            StopCoroutine(routine);
        }
    }
}