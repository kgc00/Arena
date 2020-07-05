using System;
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
            Debug.Log("context = " + context);
            InputValues.Horizontal = context.ReadValue<Vector2> ().x;
            InputValues.Forward = context.ReadValue<Vector2> ().y;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            InputValues.Turn = context.ReadValue<Vector2> ().x;
            InputValues.Look = context.ReadValue<Vector2> ().y;
            InputValues.SetControlType(context.action.activeControl.displayName);
        }

        public void OnSkill1(InputAction.CallbackContext context)
        {
            // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Skill1].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Skill1].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Skill1].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Skill1].PressInteraction = context.interaction;
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
             // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Skill2].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Skill2].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Skill2].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Skill2].PressInteraction = context.interaction;
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
             // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Skill3].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Skill3].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Skill3].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Skill3].PressInteraction = context.interaction;
        }

        public void OnSkill4(InputAction.CallbackContext context)
        {
             // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Skill4].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Skill4].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Skill4].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Skill4].PressInteraction = context.interaction;
        }
        
        public void OnNormal1(InputAction.CallbackContext context)
        {
            // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Normal1].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Normal1].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Normal1].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Normal1].PressInteraction = context.interaction;
        }

        public void OnNormal2(InputAction.CallbackContext context) {
            // Must use this if statement- it sets HasStartedFire to true for this frame and
            //  is not overwritten by subsequent input calls on the same frame.  If we modify this if statement
            //  to also set false in certain conditions, the 'true' state will never make it to consumers.
            
            
            // HasStartedFire must be reset in late update to keep an accurate picture of user input.
            if(context.phase == InputActionPhase.Started && context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                InputValues.ButtonValues[ButtonType.Normal2].HasStartedPress = true;
            }
            
            InputValues.ButtonValues[ButtonType.Normal2].PressValue = context.ReadValue<Single>();
            InputValues.ButtonValues[ButtonType.Normal2].PressPhase = context.phase;
            InputValues.ButtonValues[ButtonType.Normal2].PressInteraction = context.interaction;
        }

        // #endregion
    }
}