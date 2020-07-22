using System.Collections;
using System.Collections.Generic;

namespace Data.Stats {
    public class StatEnumerator :  IEnumerator<Stats>{
        private int curIndex;
        private Statistic curStat;
        private List<Statistic> collection;

        public StatEnumerator(Stats stats) {
            collection = new List<Statistic>();
            collection.Add(stats.Endurance);
            collection.Add(stats.Strength);
            collection.Add(stats.MovementSpeed);
            curIndex = -1;
            curStat = default(Statistic);
        }
        
        public bool MoveNext() {
            //Avoids going beyond the end of the collection.
            if (++curIndex >= collection.Count)
            {
                return false;
            }
            else
            {
                // Set current box to next item in collection.
                curStat = collection[curIndex];
            }
            return true;
        }

        public void Reset() { curIndex = -1; }

        public Stats Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose() {
            throw new System.NotImplementedException();
        }
    }
}