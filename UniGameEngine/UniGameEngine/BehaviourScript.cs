using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UniGameEngine
{
    public abstract class BehaviourScript : Component, IGameUpdate, IGameFixedUpdate
    {
        // Private
        [DataMember(Name = "Priority")]
        private int priority = 100;

        // Properties
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public Thread MainThread
        {
            get { return Game.MainThread; }
        }

        public bool IsMainThread
        {
            get { return Game.IsMainThread; }
        }

        public ContentManager Content
        {
            get { return Game.Content; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }

        public GameWindow Window
        {
            get { return Game.Window; }
        }

        // Methods
        public virtual void OnStart() { }
        public virtual void OnUpdate(GameTime gameTime) { }

        public virtual void OnFixedUpdate(GameTime gameTime, float fixedStep) { }

        protected override void RegisterSubSystems()
        {
            base.RegisterSubSystems();

            // Register for fixed update
            if(this is IGameFixedUpdate)
                Scene.sceneFixedUpdateCalls.Add((IGameFixedUpdate)this);
        }

        protected override void UnregisterSubSystems()
        {
            base.UnregisterSubSystems();

            // Unregister fixed update
            if(this is IGameFixedUpdate)
                Scene.sceneFixedUpdateCalls.Remove((IGameFixedUpdate)this);
        }

        #region CreateComponent
        public Component CreateComponent(Type componentType)
        {
            return GameObject.CreateComponent(componentType);
        }

        public T CreateComponent<T>() where T : Component
        {
            return GameObject.CreateComponent<T>();
        }

        public void CreateComponent(Component existingComponent)
        {
            GameObject.CreateComponent(existingComponent);
        }
        #endregion

        #region GetComponent
        public Component GetComponent(Type type, bool includeDisabled = false)
        {
            return GameObject.GetComponent(type, includeDisabled);
        }

        public T GetComponent<T>(bool includeDisabled = false) where T : class
        {
            return GameObject.GetComponent<T>(includeDisabled);
        }

        public T[] GetComponents<T>(bool includeDisabled = false) where T : class
        {
            return GameObject.GetComponents<T>(includeDisabled);
        }

        public int GetComponents<T>(IList<T> results, bool includeDisabled = false) where T : class
        {
            return GameObject.GetComponents<T>(results, includeDisabled);
        }

        public T GetComponentInChildren<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentInChildren<T>(includeDisabled);
        }

        public T[] GetComponentsInChildren<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentsInChildren<T>(includeDisabled, tag);
        }

        public int GetComponentsInChildren<T>(IList<T> results, bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentsInChildren<T>(results, includeDisabled, tag);
        }

        public T GetComponentInParent<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentInParent<T>(includeDisabled, tag);
        }

        public T[] GetComponentsInParent<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentsInParent<T>(includeDisabled, tag);
        }

        public int GetComponentsInParent<T>(IList<T> results, bool includeDisabled = false, string tag = null) where T : class
        {
            return GameObject.GetComponentsInParent<T>(results, includeDisabled, tag);
        }
        #endregion
    }
}
