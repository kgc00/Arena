using System;
using Data.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerController : Controller, PlayerInputMappings.IPlayerActions {

        [SerializeField] private Rigidbody body; 
        [SerializeField] private PlayerInputMappings playerInput;

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
            playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            // Debug.Log("context = " + context);
            InputValues.Horizontal = context.ReadValue<Vector2> ().x;
            InputValues.Forward = context.ReadValue<Vector2> ().y;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            InputValues.Turn = context.ReadValue<Vector2> ().x;
            InputValues.Look = context.ReadValue<Vector2> ().y;
            InputValues.SetControlType(context.action.activeControl.displayName);
        }

        private void HandleSkill(InputAction.CallbackContext context, ButtonType skill) {
            // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.

            if (context.performed) {
                if (context.control.IsActuated()) InputValues.ButtonValues[skill].HasPerformedPress = true;
                if (!context.control.IsActuated()) InputValues.ButtonValues[skill].HasPerformedRelease = true;
            }
            
            InputValues.ButtonValues[skill].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[skill].PressPhase = context.phase;
            InputValues.ButtonValues[skill].PressInteraction = context.interaction;
        }
        
        public void OnSkill1(InputAction.CallbackContext context) {
            HandleSkill(context, ButtonType.Skill1);
        }


        public void OnSkill2(InputAction.CallbackContext context)
        {
            HandleSkill(context, ButtonType.Skill2);
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
            HandleSkill(context, ButtonType.Skill3);
        }

        public void OnSkill4(InputAction.CallbackContext context)
        {
            HandleSkill(context, ButtonType.Skill4);
        }
        
        public void OnNormal1(InputAction.CallbackContext context)
        {
            HandleSkill(context, ButtonType.Normal1);
        }

        public void OnNormal2(InputAction.CallbackContext context) {
            HandleSkill(context, ButtonType.Normal2);
        }

        // #endregion
    }
}