﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UniGameEngine.Scene;

namespace UniGameEngine
{
    public enum GameObjectPrimitive
    {
        Cube,
        Sphere,
        Cylinder,
        Quad,
        Triangle,
    }

    public sealed class GameObject : GameElement
    {
        // Private
        [DataMember(Name = "Enabled")]
        private bool enabled = true;
        [DataMember(Name = "IsStatic")]
        private bool isStatic = false;
        [DataMember(Name = "Layer")]
        private uint layer = 0;
        [DataMember(Name = "Tag")]
        private string tag = "";
        [DataMember(Name = "Transform")]
        private Transform transform = null;
        [DataMember(Name = "Components")]
        private List<Component> components = null;

        // Internal
        internal GameScene scene = null;
        internal bool expanded = false;

        // Properties
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                // Trigger enable with event
                DoGameObjectEnabledEvents(this, value);
            }
        }

        public bool EnabledInHierarchy
        {
            get
            {
                // Check simple case
                if (scene.Enabled == false || enabled == false)
                    return false;

                GameObject current = this;

                // Check for parent
                while (current.transform.parent != null)
                {
                    // Check for disabled
                    if (current.enabled == false)
                        return false;

                    // Move up the hierarchy
                    current = current.transform.parent.GameObject;

                    // Check for null
                    if (current == null)
                        break;
                }

                return enabled;
            }
        }

        public bool IsStatic
        {
            get { return isStatic; }
            set { isStatic = value; }
        }

        public uint Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public Transform Transform
        {
            get { return transform; }
        }

        public GameScene Scene
        {
            get { return scene; }
        }

        // Constructor
        private GameObject() : this(null) { }

        public GameObject(string name)
            : base(name)
        {
            this.transform = new Transform(this);
        }

        // Methods
        public override string ToString()
        {
            string nameInfo = (string.IsNullOrEmpty(Name) == false) ? Name : "null";
            return string.Format("{0}({1})", GetType().FullName, nameInfo);
        }

        protected internal override void OnDestroy()
        {
            // Check for parent
            if(Transform.Parent != null)
            {
                Transform.Parent.GameObject.RemoveObject(this);
            }
            else
            {
                Scene.RemoveObject(this);
            }
        }

        #region CreateGameObject
        public GameObject CreateEmptyObject(string name)
        {
            GameObject go = new GameObject(name);

            // Initialize the game object
            CreateObject(go);
            return go;
        }

        public GameObject CreatePrimitiveObject(GameObjectPrimitive primitive, string name)
        {
            GameObject go = new GameObject(name);

            // Add components

            // Initialize the game object
            CreateObject(go);
            return go;
        }

        public GameObject CreateObject(string name, params Type[] componentTypes)
        {
            // Check for null
            if (componentTypes == null)
                throw new ArgumentNullException(nameof(componentTypes));

            GameObject go = new GameObject(name);

            // Initialize the game object
            CreateObject(go);

            // Add components
            foreach (Type componentType in componentTypes)
            {
                go.CreateComponent(componentType);
            }
            return go;
        }

        public Component CreateObject(string name, Type mainComponentType, params Type[] additionalComponentTypes)
        {
            // Check for null
            if (mainComponentType == null)
                throw new ArgumentNullException(nameof(mainComponentType));

            // Create object
            GameObject go = new GameObject(name);

            // Initialize the game object
            CreateObject(go);

            // Add component
            Component result = go.CreateComponent(mainComponentType);

            // Add additional components
            if (additionalComponentTypes != null && additionalComponentTypes.Length > 0)
            {
                foreach (Type componentType in additionalComponentTypes)
                {
                    go.CreateComponent(componentType);
                }
            }

            return result;
        }

        public T CreateObject<T>(string name, params Type[] additionalComponentTypes) where T : Component
        {
            // Create object
            GameObject go = new GameObject(name);

            // Initialize the game object
            CreateObject(go);

            // Add component
            T result = go.CreateComponent<T>();

            // Add additional components
            if (additionalComponentTypes != null && additionalComponentTypes.Length > 0)
            {
                foreach (Type componentType in additionalComponentTypes)
                {
                    go.CreateComponent(componentType);
                }
            }

            return result;
        }

        internal void CreateObject(GameObject go, bool doEnabledEvents = true)
        {
            // Check for child collection
            if (transform.children == null)
                transform.children = new List<Transform>();

            // Register object
            transform.children.Add(go.transform);
            go.scene = scene;
            go.transform.parent = transform;

            // Trigger enable
            if(doEnabledEvents == true)
                GameObject.DoGameObjectEnabledEvents(go, true, true);

            // Update scene
            if (scene != null)
                scene.InvokeSceneModified();
        }

        internal void RemoveObject(GameObject go, bool doDisabledEvents = true)
        {
            if(transform.children != null && transform.children.Contains(go.Transform) == true)
            {
                // Trigger disable
                if(doDisabledEvents == true)
                    GameObject.DoGameObjectEnabledEvents(go, false, true);

                // Unregister parent
                transform.children.Remove(go.transform);
                go.scene = scene;
                go.transform.parent = null;

                // Update scene
                if (scene != null)
                    scene.InvokeSceneModified();
            }
        }
        #endregion

        #region CreateComponent
        public Component CreateComponent(Type componentType)
        {
            // Check type
            if (typeof(Component).IsAssignableFrom(componentType) == false)
                throw new ArgumentException("Component type must derive from SharpityEngine.Component");

            // Create type instance
            Component component = Game.TypeManager.CreateTypeInstanceAs<Component>(componentType);

            // Check for error
            if (component == null)
                throw new TypeInitializationException("Could not create instance of component: " + componentType, null);

            // Initialize component
            CreateComponent(component);
            return component;
        }

        public T CreateComponent<T>() where T : Component
        {
            // Create type instance
            T component = Game.TypeManager.CreateTypeInstanceAs<T>(typeof(T));

            // Check for error
            if (component == null)
                throw new TypeInitializationException("Could not create instance of component: " + typeof(T), null);

            // Initialize component
            CreateComponent(component);
            return component;
        }

        public void CreateComponent(Component existingComponent)
        {
            // Check for null
            if (existingComponent == null)
                throw new ArgumentNullException(nameof(existingComponent));

            // Register component
            if (components == null) components = new List<Component>();
            components.Add(existingComponent);
            existingComponent.gameObject = this;

            // Trigger loaded
            GameElement.DoGameElementLoadedEvents(existingComponent);

            // Trigger enable
            Component.DoComponentEnabledEvents(existingComponent, true, true);

            // Check for scene
            if (scene != null)
                scene.InvokeSceneModified();
        }
        #endregion

        #region GetComponent
        public Component GetComponent(Type type, bool includeDisabled = false)
        {
            // Get transform
            if (type == typeof(Transform))
                return transform;

            // Get components
            if (components != null)
            {
                foreach (Component component in components)
                {
                    if (type.IsAssignableFrom(component.elementType) == true && CheckComponent(component, includeDisabled) == true)
                        return component;
                }
            }
            return null;
        }

        public T GetComponent<T>(bool includeDisabled = false) where T : class
        {
            // Get transform
            if (typeof(T) == typeof(Transform))
                return transform as T;

            // Get components
            if (components != null)
            {
                foreach (Component component in components)
                {
                    if (component is T match && CheckComponent(component, includeDisabled) == true)
                        return match;
                }
            }
            return null;
        }

        public T[] GetComponents<T>(bool includeDisabled = false) where T : class
        {
            // Get transform
            if (typeof(T) == typeof(Transform))
                return new T[] { transform as T };

            // Check for none
            if (components == null || components.Count == 0)
                return Array.Empty<T>();

            // Get components
            return components
                .Where(c => CheckComponent(c, includeDisabled) == true)
                .OfType<T>()
                .ToArray();
        }

        public int GetComponents<T>(IList<T> results, bool includeDisabled = false) where T : class
        {
            // Get transform
            if (typeof(T) == typeof(Transform))
            {
                results.Add(transform as T);
                return 1;
            }

            // Track count
            int count = 0;

            // Get components
            if (components != null)
            {
                foreach (Component component in components)
                {
                    if (component is T match && CheckComponent(component, includeDisabled) == true)
                    {
                        results.Add(match);
                        count++;
                    }
                }
            }
            // Get count
            return count;
        }

        public T GetComponentInChildren<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            // Get transform
            if (typeof(T) == typeof(Transform))
                return transform as T;

            // Search for component
            return BFSSearchComponentChildren<T>(this, includeDisabled, tag);
        }

        public T[] GetComponentsInChildren<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            // Check for transform
            bool isTransform = typeof(T) == typeof(Transform);

            // Search for components
            return BFSSearchComponentsChildren<T>(this, isTransform, includeDisabled, tag)
                .ToArray();
        }

        public int GetComponentsInChildren<T>(IList<T> results, bool includeDisabled = false, string tag = null) where T : class
        {
            // Check for transform
            bool isTransform = typeof(T) == typeof(Transform);

            // Search for components
            return BFSSearchComponentsChildren<T>(this, isTransform, results, includeDisabled, tag);
        }

        public T GetComponentInParent<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            // Get transform
            if (typeof(T) == typeof(Transform))
                return transform as T;

            // Search for component
            return BFSSearchComponentParent<T>(this, false, includeDisabled, tag);
        }

        public T[] GetComponentsInParent<T>(bool includeDisabled = false, string tag = null) where T : class
        {
            // Check for transform
            bool isTransform = typeof(T) == typeof(Transform);

            // Search for components
            return BFSSearchComponentsParent<T>(this, isTransform, includeDisabled, tag)
                .ToArray();
        }

        public int GetComponentsInParent<T>(IList<T> results, bool includeDisabled = false, string tag = null) where T : class
        {
            // Check for transform
            bool isTransform = typeof(T) == typeof(Transform);

            // Search for components
            return BFSSearchComponentsParent<T>(this, isTransform, results, includeDisabled, tag);
        }
        #endregion

        #region SearchComponents(T)
        private static T BFSSearchComponentChildren<T>(GameObject current, bool includeDisabled, string tag) where T : class
        {
            // Check for any components
            if (current.components != null && current.components.Count > 0)
            {
                // Search all
                foreach (Component component in current.components)
                {
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                        return match;
                }

                // Search deeper
                foreach (Component component in current.components)
                {
                    // Search inside child component
                    T result = BFSSearchComponentChildren<T>(component.GameObject, includeDisabled, tag);

                    // Check for match
                    if (result != null)
                        return result;
                }
            }
            // Not found
            return null;
        }

        private static IEnumerable<T> BFSSearchComponentsChildren<T>(GameObject current, bool isTransform, bool includeDisabled, string tag) where T : class
        {
            // Check for transform
            if (isTransform == true)
            {
                // Get transform
                yield return current.transform as T;
            }
            else
            {
                // Search all components
                foreach (Component component in current.components)
                {
                    // Check for match
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                        yield return match;
                }
            }

            // Search deeper
            foreach (Component component in current.components)
            {
                // Search inside child components
                foreach (T result in BFSSearchComponentsChildren<T>(component.GameObject, isTransform, includeDisabled, tag))
                    yield return result;
            }
        }

        private static int BFSSearchComponentsChildren<T>(GameObject current, bool isTransform, IList<T> results, bool includeDisabled, string tag) where T : class
        {
            int count = 0;

            // Check for transform
            if (isTransform == true)
            {
                // Get transform
                results.Add(current.transform as T);
                count++;
            }
            else
            {
                // Search all components
                foreach (Component component in current.components)
                {
                    // Check for match
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                    {
                        results.Add(match);
                        count++;
                    }
                }
            }

            // Search deeper
            foreach (Component component in current.components)
            {
                // Search inside child components
                count += BFSSearchComponentsChildren<T>(component.GameObject, isTransform, results, includeDisabled, tag);
            }
            return count;
        }

        private static T BFSSearchComponentParent<T>(GameObject current, bool isTransform, bool includeDisabled, string tag) where T : class
        {
            if (current.components != null && current.components.Count > 0)
            {
                // Search all components
                foreach (Component component in current.components)
                {
                    // Check for match
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                        return match;
                }
            }

            // Search parent
            if (current.transform.Parent != null)
            {
                // Search inside parent components
                T result = BFSSearchComponentParent<T>(current.transform.Parent.GameObject, isTransform, includeDisabled, tag);

                // Check for found
                if (result != null)
                    return result;
            }
            // Not found
            return null;
        }

        private static IEnumerable<T> BFSSearchComponentsParent<T>(GameObject current, bool isTransform, bool includeDisabled, string tag) where T : class
        {
            // Check for transform
            if (isTransform == true)
            {
                // Get transform
                yield return current.transform as T;
            }
            else
            {
                // Search all components
                foreach (Component component in current.components)
                {
                    // Check for match
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                        yield return match;
                }
            }

            // Search parent
            if (current.transform.Parent != null)
            {
                // Search inside parent components
                foreach (T result in BFSSearchComponentsParent<T>(current.transform.Parent.GameObject, isTransform, includeDisabled, tag))
                    yield return result;
            }
        }

        private static int BFSSearchComponentsParent<T>(GameObject current, bool isTransform, IList<T> results, bool includeDisabled, string tag) where T : class
        {
            int count = 0;

            // Check for transform
            if (isTransform == true)
            {
                // Get transform
                results.Add(current.transform as T);
                count++;
            }
            else
            {
                // Search all components
                foreach (Component component in current.components)
                {
                    // Check for match
                    if (component is T match && CheckComponent(component, includeDisabled, tag) == true)
                    {
                        results.Add(match);
                        count++;
                    }
                }
            }

            // Search parent
            if (current.transform.Parent != null)
                count += BFSSearchComponentsParent<T>(current.transform.Parent.GameObject, isTransform, results, includeDisabled, tag);

            return count;
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckComponent(Component component, bool includeDisabled)
        {
            return (includeDisabled == true || component.Enabled == true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckComponent(Component component, bool includeDisabled, string tag)
        {
            return (includeDisabled == true || component.Enabled == true)
                && (tag == null || component.CompareTag(tag) == true);
        }


        #region HierarchyEvents
        internal static void DoGameObjectSceneInitialize(GameObject gameObject, GameScene scene)
        {
            // Check for null
            if (gameObject != null)
            {
                // Update scene
                gameObject.scene = scene;

                // Check for children
                if (gameObject.Transform.HasChildren == true)
                {
                    // Update children
                    foreach (Transform child in gameObject.Transform.Children)
                        DoGameObjectSceneInitialize(child.GameObject, scene);
                }

                // Check for components
                if (gameObject.components != null)
                {
                    // Update components
                    foreach (Component component in gameObject.components)
                        Component.DoComponentSceneInitialize(component, gameObject);
                }
            }
        }

        internal static void DoGameObjectEnabledEvents(GameObject gameObject, bool enabled, bool forceUpdate = false)
        {
            // Store current enabled state
            bool currentEnabledState = gameObject.enabled;

            // Check for enabled in hierarchy
            bool enabledInHierarchy = gameObject.Transform.Parent != null
                ? gameObject.Transform.Parent.GameObject.EnabledInHierarchy
                : gameObject.Scene.Enabled;

            // Change enabled state
            gameObject.enabled = enabled;

            // Check for disabled in hierarchy
            if (gameObject.scene.Enabled == false || enabledInHierarchy == false || (currentEnabledState == enabled && forceUpdate == false))
                return;

            // Update components
            if (gameObject.components != null && gameObject.components.Count > 0)
            {
                // Update all components
                foreach (Component component in gameObject.components)
                {
                    if (component != null && component.Enabled == true)
                    {
                        // Trigger component enabled callback
                        Component.OnComponentEnabledEvent(component, enabled);
                    }
                }
            }

            // Get transform
            Transform transform = gameObject.transform;

            // Update children recursive
            if (transform.children != null && transform.children.Count > 0)
            {
                foreach (Transform child in transform.children)
                {
                    // Recursive call
                    DoGameObjectEnabledEvents(child.GameObject, enabled, forceUpdate);
                }
            }
        }

        internal static void DoGameObjectDestroyEvents(GameObject gameObject)
        {
            try
            {
                gameObject.OnDestroy();
            }
            catch (Exception e) { Debug.LogException(e); }

            // Update components
            if (gameObject.components != null && gameObject.components.Count > 0)
            {
                // Update all components
                foreach (Component component in gameObject.components)
                {
                    if (component != null && component.Enabled == true)
                    {
                        // Trigger component enabled callback
                        DoGameElementDestroyEvents(component);
                    }
                }
            }

            // Get transform
            Transform transform = gameObject.transform;

            // Update children recursive
            if (transform.children != null && transform.children.Count > 0)
            {
                foreach (Transform child in transform.children)
                {
                    // Recursive call
                    DoGameObjectDestroyEvents(child.GameObject);
                }
            }
        }
        #endregion
    }
}
