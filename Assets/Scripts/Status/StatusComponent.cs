using System.Linq;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Status
{
    public class StatusComponent : MonoBehaviour {
        private Material _fresnel;
        public Unit Owner { get; private set; }
        public StatusType StatusType { get; private set; } = 0;
        public StatusComponent Initialize (Unit owner) {
            Owner = owner;
            _fresnel = MonoHelper.LoadMaterial(MaterialType.MarkOutline);
            return this;
        }
        
        public void AddStatus(StatusType statusType, float duration, float amount) {
            if (StatusType.HasFlag(statusType)) return;
            AddTimedStatus(statusType, duration, amount);
            StatusType |= statusType;
        }
        
        public void AddStatus(StatusType statusType, float amount) {
            if (StatusType.HasFlag(statusType)) return;
            AddUntimedStatus(statusType, amount);
            StatusType |= statusType;
        }

        private void AddUntimedStatus(StatusType statusType, float amount) {
            switch (statusType) {
                case StatusType.Marked:
                    var spawnPos = Owner.transform.position;
                    spawnPos.y += 1.5f;
                    var vfx = MonoHelper.SpawnVfx(VfxType.Mark, spawnPos);
                    vfx.transform.SetParent(Owner.transform);
                    var rend = Owner.transform.root.GetComponentInChildren<Renderer>();
                    var materials = rend.materials.ToList();
                    materials.Add(_fresnel);
                    rend.materials = materials.ToArray();
                    gameObject.AddComponent<Marked>().Initialize(Owner, false, amount);
                    break;
                case StatusType.Hidden:
                    break;
            }
        }

        private void AddTimedStatus(StatusType statusType, float duration, float amount) {
            switch (statusType) {
                case StatusType.Stunned:
                    gameObject.AddComponent<Stunned>().Initialize(Owner, duration, amount);
                    break;
                case StatusType.Hidden:
                    break;
            }
        }

        public void RemoveStatus(StatusType statusType) {
            if (!StatusType.HasFlag(statusType)) return;
            switch (statusType) {
                case StatusType.Marked:
                    var rend = Owner.transform.root.GetComponentInChildren<Renderer>();
                    var materials = rend.materials;
                    var withoutFresnel = materials.ToList().Where(m => m.shader.name != _fresnel.shader.name).ToArray();
                    rend.materials = withoutFresnel;
                    break;
            }
            StatusType &= ~statusType;
            Debug.Log("Removed Status");
        }

        public bool IsVisible() => !StatusType.HasFlag(StatusType.Hidden);
        public bool IsStunned() => StatusType.HasFlag(StatusType.Stunned);
    }
 
}