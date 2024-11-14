using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using UniGameEngine.Scene;

namespace UniGameEngine.Graphics
{
    [DataContract]
    public sealed class Camera : Component
    {
        // Type
        private sealed class CameraSorter : IComparer<Camera>
        {
            // Methods
            public int Compare(Camera x, Camera y)
            {
                return x.renderQueue.CompareTo(y.renderQueue);
            }
        }

        // Events
        [DataMember(Name = "OnWillRender")]
        public readonly GameEvent OnWillRender = new GameEvent();

        // Private
        private static readonly CameraSorter cameraSorter = new CameraSorter();
        private static readonly List<Camera> allCameras = new List<Camera>();
        private static readonly List<Camera> allActiveCameras = new List<Camera>();

        [DataMember(Name = "RenderTexture")]
        private Texture renderTexture = null;
        [DataMember(Name = "CullingMask")]
        private uint cullingMask = uint.MaxValue;
        [DataMember(Name = "ClearColor")]
        private Color clearColor = Color.CornflowerBlue;
        [DataMember(Name = "RenderQueue")]
        private int renderQueue = 0;
        [DataMember(Name = "Near")]
        private float near = 0.01f;
        [DataMember(Name = "Far")]
        private float far = 1000f;
        [DataMember(Name = "FieldOfView")]
        private float fieldOfView = 60f;
        [DataMember(Name = "Orthographic")]
        private bool orthographic = false;

        // Internal
        internal Matrix projectionMatrix = Matrix.Identity;
        internal SpriteBatch spriteBatch = null;

        // Properties
        public static IReadOnlyList<Camera> AllCameras
        {
            get { return allCameras; }
        }

        public static IReadOnlyList<Camera> AllActiveCameras
        {
            get { return allActiveCameras; }
        }

        public static Camera MainCamera
        {
            get { return allActiveCameras.Count > 0 ? allActiveCameras[0] : null; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        public uint CullingMask
        {
            get { return cullingMask; }
            set { cullingMask = value; }
        }

        public Color ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; }
        }

        public int RenderQueue
        {
            get { return renderQueue; }
            set { renderQueue = value; }
        }

        public float Near
        {
            get { return near; }
            set
            {
                near = value;
                CreateViewProjectionMatrix();
            }
        }

        public float Far
        {
            get { return far; }
            set
            {
                far = value;
                CreateViewProjectionMatrix();
            }
        }

        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                fieldOfView = value;
                CreateViewProjectionMatrix();
            }
        }

        public bool Orthographic
        {
            get { return orthographic; }
            set
            {
                orthographic = value;
                CreateViewProjectionMatrix();
            }
        }

        public int RenderWidth
        {
            get
            {
                //// Check for render texture
                //if (renderTexture != null)
                //    return renderTexture.Width;

                // Get device width
                return Game.RenderWidth;
            }
        }

        public int RenderHeight
        {
            get
            {
                //// Check for render texture
                //if (renderTexture != null)
                //    return renderTexture.Height;

                // Get device width
                return Game.RenderHeight;
            }
        }

        public float AspectRatio
        {
            get { return (float)RenderWidth / RenderHeight; }
        }

        // Constructor
        public Camera()
        {
        }

        // Methods
        protected internal override void OnLoaded()
        {
            // Add camera
            allCameras.Add(this);

            // Sort by render queue
            allCameras.Sort(cameraSorter);

            // Create batch
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            // Create matrix
            CreateViewProjectionMatrix();
        }

        protected internal override void OnDestroy()
        {
            // Release batch
            spriteBatch.Dispose();

            // Remove camera
            allCameras.Remove(this);
        }

        public void Clear()
        {
            Game.GraphicsDevice.Clear(clearColor);
        }

        public void Render()
        {
            foreach(GameScene scene in Game.scenes)
                Render(scene);
        }

        public void Render(GameScene scene)
        {
            // Check for scene visible
            if (scene == null || scene.Enabled == false)
                return;

            // Check for any draw calls
            if (scene.sceneDrawCalls == null || scene.sceneDrawCalls.Count == 0)
                return;

            // Begin batch
            spriteBatch.Begin();

            // Draw all
            foreach(IGameDraw drawCall in scene.sceneDrawCalls)
            {
                // Draw
                drawCall.OnDraw(this);
            }

            // End batch
            spriteBatch.End();
        }

        protected override void OnEnable()
        {
            // Add camera
            allActiveCameras.Add(this);

            // Sort by render queue
            allActiveCameras.Sort(cameraSorter);
        }

        protected override void OnDisable()
        {
            // Remove camera
            allActiveCameras.Remove(this);
        }

        private void CreateViewProjectionMatrix()
        {
            // Check for orthographic
            if (orthographic == false)
            {
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(fieldOfView), AspectRatio, near, far);
            }
            else
            {
                // Create orthographic
                projectionMatrix = Matrix.CreateOrthographic(
                    -1, 1, near, far);
            }
        }
    }
}
