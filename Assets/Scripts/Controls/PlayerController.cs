using Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerController : Controller, PlayerInputMappings.IPlayerActions {

        [SerializeField] private Rigidbody body; 
        [SerializeField] private PlayerInputMappings playerInput;
        public Unit Owner { get; private set; }
        // #region MultipleHandlers
        public void OnEnable()
        {
            if (InputValues == null)
            {
                InputValues = new InputValues();
            }
            if (playerInput == null)
            {
                playerInput = new PlayerInputMappings();
                playerInput.Player.SetCallbacks(this);
            }
        
            if (Owner == null)
            {
                Owner = GetComponentInParent<Unit>();
            }
            playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Owner.State.HandleOnMove(context);
            // InputValues.Horizontal = context.ReadValue<Vector2> ().x;
            // InputValues.Forward = context.ReadValue<Vector2> ().y;
            // Debug.Log($"{InputValues.Horizontal}{InputValues.Forward}");
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Owner.State.HandleOnLook(context);
            // InputValues.Turn = context.ReadValue<Vector2> ().x;
            // InputValues.Look = context.ReadValue<Vector2> ().y;
            InputValues.SetControlType(context.action.activeControl.displayName);
            // Debug.Log($"{InputValues.ActiveControl}");
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            Owner.State.HandleOnFire(context);
            // InputValues.Fire = context.ReadValue<Single>();
            // InputValues.FirePhase = context.phase;
            // Debug.Log($"context {context}");
        }
        // #endregion
    }
}