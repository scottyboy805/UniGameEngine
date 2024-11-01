using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace UniGameEngine
{
    public sealed class GameSettings
    {
        // Private
        // Game info
        [DataMember(Name = "CompanyName")]
        private string companyName = "Default Company";
        [DataMember(Name = "GameName")]
        private string gameName = "Default Game";
        [DataMember(Name = "GameVersion")]
        private Version gameVersion = new Version(1, 0, 0);

        // Presentation info
        [DataMember(Name = "PreferredScreenWidth")]
        private int preferredScreenWidth = 1280;
        [DataMember(Name = "PreferredScreenHeight")]
        private int preferredScreenHeight = 720;
        [DataMember(Name = "PreferredFullscreen")]
        private bool preferredFullscreen = false;
        [DataMember(Name = "ResizableWindow")]
        private bool resizableWindow = false;

        // Physics
        [DataMember(Name = "Gravity")]
        private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

        [DataMember(Name = "StartupScenes")]
        private List<string> startupScenes = new List<string>();

        // Properties
        public string CompanyName
        {
            get { return companyName; }
        }

        public string GameName
        {
            get { return gameName; }
        }

        public Version GameVersion
        {
            get { return gameVersion; }
        }

        public int PreferredScreenWidth
        {
            get { return preferredScreenWidth; }
        }

        public int PreferredScreenHeight
        {
            get { return preferredScreenHeight; }
        }

        public bool PreferredFullscreen
        {
            get { return preferredFullscreen; }
        }

        public bool ResizableWindow
        {
            get { return resizableWindow; }
        }

        public Vector3 Gravity
        {
            get { return gravity; }
        }

        public IReadOnlyList<string> StartupScenes
        {
            get { return startupScenes; }
        }
    }
}
