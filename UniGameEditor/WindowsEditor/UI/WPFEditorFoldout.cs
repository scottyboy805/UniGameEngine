﻿using Microsoft.Xna.Framework;
using System.Windows;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorFoldout : EditorFoldout
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal Expander expander = null;
        internal StackPanel stackPanel = null;

        // Properties
        public override float Width
        {
            get => (float)expander.ActualWidth;
            set => expander.Width = value;
        }
        public override float Height
        {
            get => (float)expander.ActualHeight;
            set => expander.Height = value;
        }
        public override string Text
        {
            get => (string)expander.Content;
            set => expander.Content = value;
        }
        public override bool IsExpanded
        {
            get => expander.IsExpanded;
            set => expander.IsExpanded = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => dragDrop.DropHandler = value;
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                expander.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorFoldout(Panel parent, string text, bool isExpanded)
        {
            expander = new Expander();
            InitializeExpanded(text, isExpanded);

            parent.Children.Add(expander);
        }

        public WPFEditorFoldout(ItemsControl parent, string text, bool isExpanded)
        {
            expander = new Expander();
            InitializeExpanded(text, isExpanded);

            parent.Items.Add(expander);
        }

        // Methods
        private void InitializeExpanded(string text, bool isExpanded)
        {
            dragDrop = new WPFDragDrop(expander);
            expander.Content = stackPanel = new StackPanel();
            expander.Header = text;
            expander.IsExpanded = isExpanded;

            // Add listener
            expander.Expanded += (object sender, RoutedEventArgs e) => OnExpandedEvent(true);
            expander.Collapsed += (object sender, RoutedEventArgs e) => OnExpandedEvent(false);
        }

        public override EditorLabel AddLabel(string text)
        {
            return new WPFEditorLabel(stackPanel, text);
        }

        public override EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText)
        {
            return new WPFEditorPropertyLabel(stackPanel, property, overrideText);
        }

        public override EditorInput AddInput(string text)
        {
            return new WPFEditorInput(stackPanel, text);
        }

        public override EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue)
        {
            return new WPFEditorNumberInput(stackPanel, value, min, max);
        }

        public override EditorButton AddButton(string text)
        {
            return new WPFEditorButton(stackPanel, text);
        }

        public override EditorToggleButton AddToggleButton(string text, bool on)
        {
            return new WPFEditorToggleButton(stackPanel, text, on);
        }

        public override EditorToggle AddToggle(string text, bool on)
        {
            return new WPFEditorToggle(stackPanel, text, on);
        }

        public override EditorDropdown AddDropdown()
        {
            return new WPFEditorDropdown(stackPanel);
        }

        public override EditorCombinationDropdown AddCombinationDropdown()
        {
            return new WPFEditorCombinationDropdown(stackPanel);
        }

        public override EditorRenderView AddRenderView(Game gameHost)
        {
            return new WPFEditorRenderView(stackPanel, gameHost);
        }

        public override EditorTreeView AddTreeView()
        {
            return new WPFEditorTreeView(stackPanel);
        }

        public override EditorFoldout AddFoldoutLayout(string text, bool isExpanded = false)
        {
            return new WPFEditorFoldout(stackPanel, text, isExpanded);
        }

        public override EditorLayoutControl AddFlowLayout()
        {
            return new WPFEditorWrapPanelLayout(stackPanel);
        }

        public override EditorLayoutControl AddHorizontalLayout()
        {
            return new WPFEditorStackLayout(stackPanel, Orientation.Horizontal);
        }

        public override EditorLayoutControl AddVerticalLayout()
        {
            return new WPFEditorStackLayout(stackPanel, Orientation.Vertical);
        }

        public override EditorSplitViewLayoutControl AddHorizontalSplitLayout()
        {
            return new WPFSplitView(stackPanel, Orientation.Horizontal);
        }

        public override EditorSplitViewLayoutControl AddVerticalSplitLayout()
        {
            return new WPFSplitView(stackPanel, Orientation.Vertical);
        }

        public override EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true)
        {
            return new WPFEditorScrollView(stackPanel, horizontal, vertical);
        }

        public override void Clear()
        {
            stackPanel.Children.Clear();
        }
    }
}
