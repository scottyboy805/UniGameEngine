using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace UniGameEngine.UI.Events
{
    public sealed class UIEventDispatcher : Component, IGameUpdate
    {
        // Private
        private List<UIGraphic> allRaycastTargets = new List<UIGraphic>();
        private List<UIGraphic> activeRaycastTargets = new List<UIGraphic>();
        private UIGraphic pressedTarget = null;
        private MouseState lastMouseState = default;

        // Properties
        public int Priority => 100;

        // Methods
        void IGameUpdate.OnStart()
        {

        }

        void IGameUpdate.OnUpdate(GameTime gameTime)
        {
            // Get mouse state
            MouseState currentMouseState = Mouse.GetState();

            // Check for mouse move
            if (currentMouseState.Position != lastMouseState.Position)
            {
                DoMouseMoveUpdate(currentMouseState.Position);
            }

            // Check for left button
            if(currentMouseState.LeftButton != lastMouseState.LeftButton)
            {
                // Do event
                DoMouseButtonUpdate(currentMouseState.Position, currentMouseState.LeftButton, 0);
            }
            //// Check for middle button
            //if (currentMouseState.MiddleButton != lastMouseState.MiddleButton)
            //{
            //    // Do event
            //    DoMouseButtonUpdate(currentMouseState.Position, currentMouseState.MiddleButton, 1);
            //}
            //// Check for right button
            //if (currentMouseState.RightButton != lastMouseState.RightButton)
            //{
            //    // Do event
            //    DoMouseButtonUpdate(currentMouseState.Position, currentMouseState.RightButton, 2);
            //}

            // Update last state
            lastMouseState = currentMouseState;
        }

        private void DoMouseMoveUpdate(Point mousePosition)
        {
            bool didHitTarget = false;

            // Process all targets
            foreach(UIGraphic target in GetRaycastTargets())
            {
                // Check for raycast
                bool hit = target.PerformRaycast(mousePosition);

                // Check for enter
                if(hit == true && didHitTarget == false && activeRaycastTargets.Contains(target) == false)
                {
                    // Set hit flag - only send to the first hit target
                    didHitTarget = true;

                    // Add the target
                    activeRaycastTargets.Add(target);
                    DoPointerEnterEvent(target);
                }
                // Check for exit
                else if(hit == false && activeRaycastTargets.Contains(target) == true)
                {
                    // Remove the target
                    activeRaycastTargets.Remove(target);
                    DoPointerExitEvent(target);
                }
            }
        }

        private void DoMouseButtonUpdate(Point mousePosition, ButtonState state, int button)
        {
            // Check for released
            if (state == ButtonState.Released && pressedTarget != null)
            {
                DoPressEndEvent(pressedTarget);
                pressedTarget = null;
            }

            // Check for pressed
            if (state == ButtonState.Pressed)
            {
                // Only apply to active targets
                foreach (UIGraphic target in activeRaycastTargets)
                {
                    // Check for raycast
                    bool hit = target.PerformRaycast(mousePosition);

                    // Check for hit
                    if (hit == true)
                    {
                        // Add the target
                        pressedTarget = target;
                        DoPressBeginEvent(target);
                        break;
                    }
                }
            }
        }

        private IEnumerable<UIGraphic> GetRaycastTargets()
        {
            foreach(UIGraphic graphic in allRaycastTargets)
            {
                if(graphic.Raycast == true)
                    yield return graphic;
            }
        }

        internal void AddRaycastTarget(UIGraphic target)
        {
            if(target != null && allRaycastTargets.Contains(target) == false)
                allRaycastTargets.Add(target);
        }

        internal void RemoveRaycastTarget(UIGraphic target)
        {
            if(target != null && allRaycastTargets.Contains(target) == true)
            {
                allRaycastTargets.Remove(target);

                // Check for active
                if(activeRaycastTargets.Contains(target) == true)
                {
                    activeRaycastTargets.Remove(target);
                    DoPointerExitEvent(target);
                }
            }
        }

        internal static void DoPointerEnterEvent(UIGraphic target)
        {
            // Check for pointer
            if (target is IUIPointerEvent)
            {
                try
                {
                    ((IUIPointerEvent)target).OnPointerEnter();
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        internal static void DoPointerExitEvent(UIGraphic target)
        {
            // Check for pointer
            if (target is IUIPointerEvent)
            {
                try
                {
                    ((IUIPointerEvent)target).OnPointerExit();
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        internal static void DoPressBeginEvent(UIGraphic target)
        {
            // Check for press
            if(target is IUIPressEvent)
            {
                try
                {
                    ((IUIPressEvent)target).OnPressBegin();
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        internal static void DoPressEndEvent(UIGraphic target)
        {
            // Check for press
            if (target is IUIPressEvent)
            {
                try
                {
                    ((IUIPressEvent)target).OnPressEnd();
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }
    }
}
