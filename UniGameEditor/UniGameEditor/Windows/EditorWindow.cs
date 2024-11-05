﻿using System.Runtime.CompilerServices;
using UniGameEditor.UI;

[assembly: InternalsVisibleTo("WindowsEditor")]

namespace UniGameEditor.Windows
{
    public enum EditorWindowLocation
    {
        Center,
        Right,
        Left,
        Bottom,
    }

    public abstract class EditorWindow
    {
        // Events
        internal static event Action<EditorWindow, EditorWindowLocation> OnRequestOpenWindow;

        // Internal
        internal UniEditor editor = null;
        internal EditorLayoutControl rootControl = null;
        internal string title = null;
        internal bool isOpen = false;

        // Properties
        public UniEditor Editor
        {
            get { return editor; }
        }

        public EditorLayoutControl RootControl
        {
            get { return rootControl; }
        }

        public string Title
        {
            get { return title; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        // Constructor
        protected EditorWindow()
        {
            title = GetType().Name;
        }

        // Methods
        protected internal virtual void OnShow() { }

        protected internal virtual void OnHide() { }

        // Methods
        public static T OpenEditorWindow<T>(EditorWindowLocation location = EditorWindowLocation.Center) where T : EditorWindow, new()
        {
            // Create instance
            T window = new T();

            // Request show
            OnRequestOpenWindow(window, location);
            return window;
        }
    }
}
