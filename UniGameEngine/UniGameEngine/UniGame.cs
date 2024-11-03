using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UniGameEngine.Content.Serializers;
using UniGameEngine.Graphics;
using UniGameEngine.Physics;
using UniGameEngine.Scene;
using UniGameEngine.UI;

[assembly: InternalsVisibleTo("UniGamePipeline")]

namespace UniGameEngine
{
    public abstract class UniGame : Game
    {
        // Private
        private static UniGame current = null;
        private static readonly GameUpdateComparer updateComparer = new GameUpdateComparer();
        private static readonly GameDrawComparer drawComparer = new GameDrawComparer();

        private readonly Thread mainThread = Thread.CurrentThread;
        private readonly TypeManager typeManager = Serializer.TypeManager;

        private GraphicsDeviceManager graphics = null;
        private PhysicsSimulation physics = null;
        private GameSettings gameSettings = new GameSettings();
        private List<IGameUpdate> scheduledUpdateElements = new List<IGameUpdate>(256);
        private List<IGameDraw> scheduledDrawElements = new List<IGameDraw>(256);

        private SpriteBatch spriteBatch = null;
        private SpriteFont defaultFont = null;

        // Internal
        internal List<GameScene> scenes = new List<GameScene>();        
        internal List<GameElement> scheduledDestroyDelayElements = new List<GameElement>();
        internal Queue<GameElement> scheduleDestroyElements = new Queue<GameElement>();

        // Public
        public static readonly Version EngineVersion = new Version(1, 0, 0);

        // Properties
        internal static UniGame Current
        {
            get { return current; }
        }

        public Thread MainThread
        {
            get { return mainThread; }
        }

        public TypeManager TypeManager
        {
            get { return typeManager; }
        }

        public PhysicsSimulation Physics
        {
            get { return physics; }
        }

        public GameSettings GameSettings
        {
            get { return gameSettings; }
        }

        public bool IsMainThread
        {
            get { return Thread.CurrentThread == mainThread; }
        }

        public int RenderWidth
        {
            get { return Window.ClientBounds.Width; }
        }

        public int RenderHeight
        {
            get { return Window.ClientBounds.Height; }
        }

        public abstract bool IsEditor { get; }

        public abstract bool IsPlaying { get; }

        public IReadOnlyList<GameScene> Scenes
        {
            get { return scenes; }
        }

        // Constructor
        public UniGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Methods
        protected override void Initialize()
        {
            current = this;

            // Setup debug
#if DEBUG
            Debug.AddLogger(new Debug.ConsoleLogger());
#endif

            Debug.Log("UniGameEngine, " + EngineVersion);

            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Register modules
            typeManager.RegisterAssembly(typeof(UniGame).Assembly);


            // Load game settings
            Debug.Log("Load game settings...");

            // Initialize info
            Debug.Log(gameSettings.CompanyName);
            Debug.Log(gameSettings.GameName + ", " + gameSettings.GameVersion);
                        
            // Initialize display
            Debug.Log(LogFilter.Graphics, "Preferred screen width: " + gameSettings.PreferredScreenWidth);
            Debug.Log(LogFilter.Graphics, "Preferred screen height: " + gameSettings.PreferredScreenHeight);
            Debug.Log(LogFilter.Graphics, "Preferred fullscreen: " + gameSettings.PreferredFullscreen);

            // Apply resolution
            graphics.PreferredBackBufferWidth = gameSettings.PreferredScreenWidth;
            graphics.PreferredBackBufferHeight = gameSettings.PreferredScreenHeight;
            graphics.IsFullScreen = gameSettings.PreferredFullscreen;
            graphics.ApplyChanges();

            Debug.Log(LogFilter.Graphics, "Allow resizing: " + gameSettings.ResizableWindow);
            Window.AllowUserResizing = gameSettings.ResizableWindow;
            Window.Title = gameSettings.GameName;

            // Initialize physics
            Debug.Log(LogFilter.Physics, "Initialize physics...");
            physics = new PhysicsSimulation(gameSettings);

            // Add for update
            AddGameUpdate(physics);

            
            base.Initialize();
        }

        protected override void LoadContent()
        {              
            // Load default font
            defaultFont = Content.Load<SpriteFont>("Arial");


            // Load startup scenes
            if (gameSettings.StartupScenes.Count > 0)
            {
                // Store scenes until all loaded
                List<GameScene> loadedScenes = new List<GameScene>(gameSettings.StartupScenes.Count);

                // Start loading
                Debug.LogF(LogFilter.Content, "Loading startup scenes ({0})...", gameSettings.StartupScenes.Count);
                foreach (string scenePath in gameSettings.StartupScenes)
                {
                    // Load the scene
                    GameScene startupScene = null;

                    try
                    {
                        // Load the scene
                        startupScene = Content.Load<GameScene>(scenePath);

                        // Add the scene
                        loadedScenes.Add(startupScene);
                    }
                    catch(ContentLoadException e)
                    {
                        Debug.LogError("Error loading startup scene: " + scenePath);
                        Debug.LogException(e);
                    }
                }

                // Activate scenes
                foreach(GameScene startupScene in loadedScenes)
                {
                    // Activate
                    startupScene.Activate();
                }
            }
            else
            {
                Debug.Log(LogFilter.Content, "No startup scenes to load!");
            }

            
            GameScene scene = new GameScene("MyScene");

            // Create camera
            Camera cam = scene.CreateObject<Camera>("Camera");
            //cam.Enabled = false;
            cam.Transform.LocalPosition = new Vector3(0f, 0f, -10f);

            ModelRenderer dynamicCube = scene.CreateObject<ModelRenderer>("Dynamic", typeof(BoxCollider), typeof(RigidBody));
            dynamicCube.Model = Content.Load<Model>("Cube");

            dynamicCube.Transform.WorldPosition = new Vector3(0f, 5, 0f);

            // Create object
            ModelRenderer cube = scene.CreateObject<ModelRenderer>("Static");
            cube.gameObject.CreateComponent<BoxCollider>();
            cube.Model = Content.Load<Model>("Cube");
            cube.gameObject.CreateComponent<TestScript>();

            cube.Transform.WorldPosition += new Vector3(0f, -3f, 0f);
            cube.Transform.LocalScale = new Vector3(3f, 0.1f, 3f);
            


            // Create UI
            UICanvas canvas = scene.CreateObject<UICanvas>("Canvas");
            Image img = canvas.GameObject.CreateObject<Image>("Image");
            Label txt = canvas.gameObject.CreateObject<Label>("Txt");
            txt.Text = "Hello World";
            txt.Font = Content.Load<SpriteFont>("Arial");


            GameObject customLoadContent = Content.Load<GameObject>("Test");

            scene.Activate();


            GameScene loadScene = Content.Load<GameScene>("Scene/TestScene");

            loadScene.Activate();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Do scheduled updates
            DoScheduledUpdateEvents(gameTime);
            DoScheduledDestroyedElements(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Get rendering cameras
            IReadOnlyList<Camera> activeSortedRenderingCameras = Camera.AllActiveCameras;

            // Check for any
            if(activeSortedRenderingCameras.Count == 0)
            {
                // Clear to black
                GraphicsDevice.Clear(Color.Black);

                string message = "No Active Rendering Camera!";

                // Get size of string
                Vector2 size = defaultFont.MeasureString(message);

                // Draw error hint
                spriteBatch.Begin();
                spriteBatch.DrawString(defaultFont, message, new Vector2(RenderWidth / 2 - size.X / 2, RenderHeight / 2 - size.Y / 2), Color.White);
                spriteBatch.End();
            }
            else
            {
                // Clear the first camera only
                activeSortedRenderingCameras[0].Clear();

                // Draw all cameras
                foreach (Camera camera in activeSortedRenderingCameras)
                {
                    camera.Render();
                }
            }

            base.Draw(gameTime);
        }

        internal bool AddGameScene(GameScene scene)
        {
            // Check for already added
            if (scenes.Contains(scene) == true)
                return false;

            // Add the scene
            scenes.Add(scene);
            return true;
        }

        internal void AddGameUpdate(IGameUpdate update)
        {
            // Check for null
            if (update == null)
                return;

            // Add to collection
            if(scheduledUpdateElements.Contains(update) == false)
            {
                // Add
                scheduledUpdateElements.Add(update);

                // Sort
                scheduledUpdateElements.Sort(updateComparer);
            }
        }

        internal void AddGameDraw(IGameDraw draw)
        {
            // Check for null
            if (draw == null)
                return;

            // Add to collection
            if(scheduledDrawElements.Contains(draw) == false)
            {
                // Add
                scheduledDrawElements.Add(draw);

                // Sort
                scheduledDrawElements.Sort(drawComparer);
            }
        }

        internal protected void DoScheduledUpdateEvents(GameTime gameTime)
        {
            // Do update events
            foreach (IGameUpdate updateReceiver in scheduledUpdateElements)
            {
                try
                {
                    updateReceiver.OnUpdate(gameTime);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal protected void DoScheduledDrawEvents(GameTime gameTime)
        {
            // Do draw events
            foreach(IGameDraw drawReceiver in scheduledDrawElements)
            {
                try
                {
                    drawReceiver.OnDraw(null);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal protected void DoScheduledDestroyedElements(GameTime time)
        {
            // Update all delayed elements
            foreach (GameElement element in scheduledDestroyDelayElements)
            {
                if (time != null)
                {
                    // Update time
                    element.scheduledDestroyTime -= (float)time.ElapsedGameTime.TotalSeconds;

                    // Check for time expired
                    if (element.scheduledDestroyTime <= 0f)
                        scheduleDestroyElements.Enqueue(element);
                }
                else
                {
                    scheduleDestroyElements.Enqueue(element);
                }
            }

            // Update all destroyed elements
            while (scheduleDestroyElements.Count > 0)
            {
                // Remove element
                GameElement destroyed = scheduleDestroyElements.Dequeue();

                // Remove from delayed collection
                if (scheduledDestroyDelayElements.Contains(destroyed) == true)
                    scheduledDestroyDelayElements.Remove(destroyed);

                // Trigger destroy event
                try
                {
                    destroyed.OnDestroy();
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }

                // Destroy element
                //TypeManager.DestroyElementTypeInstance(destroyed);


            }
        }

        internal protected void ScheduleDestruction(GameElement element)
        {
            // Check for error
            if (element == null || element.IsDestroyed == true)
                return;

            // Push to queue
            scheduleDestroyElements.Enqueue(element);
        }

        internal protected void ScheduleDestruction(GameElement element, float delay)
        {
            // Check for error
            if (element == null || element.IsDestroyed == true)
                return;

            // Set target delay
            element.scheduledDestroyTime = delay;

            // Push to collection
            scheduledDestroyDelayElements.Add(element);
        }
    }
}
