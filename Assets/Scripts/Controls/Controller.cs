using UnityEngine;

namespace Controls
{
    public abstract class Controller : MonoBehaviour
    {
        public virtual InputValues InputValues { get; protected set; } = new InputValues();
        
        public virtual void HandleUpdate(){}
    }
}