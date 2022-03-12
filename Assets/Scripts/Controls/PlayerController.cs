using System;
using Data.Types;
using UI.InGameShop;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.NotificationCenter;

namespace Controls
{
    public partial class PlayerController : Controller, Generated.PlayerInputMappings.IPlayerActions, Generated.PlayerInputMappings.IUIActions {

        [SerializeField] private Rigidbody body; 
        [SerializeField] private Generated.PlayerInputMappings playerInput;

        public ControlSchemeEnum ControlScheme { get; private set; }

        // #region MultipleHandlers
        public void OnEnable()
        {
            InputValues ??= new InputValues();
        
            if (playerInput == null)
            {
                playerInput = new Generated.PlayerInputMappings();
                playerInput.Player.SetCallbacks(this);
                playerInput.UI.SetCallbacks(this);
            }

            EnablePlayerSchema();
        }

        private void OnDisable() {
            DisableControls();
        }

        private void DisableControls() {
            playerInput.Player.Disable();
            playerInput.UI.Disable();
            ControlScheme = ControlSchemeEnum.None;
            InputValues.ControlSchema = ControlScheme;
        }

        public void EnablePlayerSchema() {
            playerInput.UI.Disable();
            playerInput.Player.Enable();
            ControlScheme = ControlSchemeEnum.Player;
            InputValues.ControlSchema = ControlScheme;
            InputValues.ResetButtonValues();
            PreviousPress = null;
        }
        
        public void EnableUISchema(){
            playerInput.Player.Disable();
            playerInput.UI.Enable();
            ControlScheme = ControlSchemeEnum.UI;
            InputValues.ControlSchema = ControlScheme;
            InputValues.ResetButtonValues();
            PreviousPress = null;
        }

        // Player
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

        private void HandleButtonPress(InputAction.CallbackContext context, ButtonType skill) {
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
            HandleButtonPress(context, ButtonType.Skill1);
        }


        public void OnSkill2(InputAction.CallbackContext context)
        {
            HandleButtonPress(context, ButtonType.Skill2);
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
            HandleButtonPress(context, ButtonType.Skill3);
        }

        public void OnSkill4(InputAction.CallbackContext context)
        {
            HandleButtonPress(context, ButtonType.Skill4);
        }
        
        public void OnNormal1(InputAction.CallbackContext context)
        {
            HandleButtonPress(context, ButtonType.Normal1);
        }

        public void OnNormal2(InputAction.CallbackContext context) {
            HandleButtonPress(context, ButtonType.Normal2);
        }

        public void OnMenu(InputAction.CallbackContext context) {
            HandleButtonPress(context, ButtonType.ShopMenu);
        }
        
        // UI
        public void OnNavigate(InputAction.CallbackContext context) { }
        public void OnSubmit(InputAction.CallbackContext context) { }
        public void OnCancel(InputAction.CallbackContext context) { }
        public void OnPoint(InputAction.CallbackContext context) { }
        public void OnClick(InputAction.CallbackContext context) { }
        public void OnScrollWheel(InputAction.CallbackContext context) { }
        public void OnMiddleClick(InputAction.CallbackContext context) { }
        public void OnRightClick(InputAction.CallbackContext context) { }
        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
        public void OnTrackedDeviceSelect(InputAction.CallbackContext context) { }
        // #endregion
    }
}