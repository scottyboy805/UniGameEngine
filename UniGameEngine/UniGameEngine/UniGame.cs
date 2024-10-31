using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using UniGameEngine.Scene;

namespace UniGameEngine
{
    public abstract class UniGame : Game
    {
        // Private
        private static UniGame current = null;
        private static readonly GameUpdateComparer updateComparer = new GameUpdateComparer();
        private static readonly GameDrawComparer drawComparer = new GameDrawComparer();

        private readonly Thread mainThread = Thread.CurrentThread;
        private readonly TypeManager typeManager = null;// new TypeManager();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<GameScene> scenes = new List<GameScene>();
        private List<IGameUpdate> scheduledUpdateElements = new List<IGameUpdate>(256);
        private List<IGameDraw> scheduledDrawElements = new List<IGameDraw>(256);
        private List<GameElement> scheduledDestroyDelayElements = new List<GameElement>();
        private Queue<GameElement> scheduleDestroyElements = new Queue<GameElement>();

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

        public bool IsMainThread
        {
            get { return Thread.CurrentThread == mainThread; }
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

            // Initialize type manager


            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            

            // TODO: use this.Content to load your game content here
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

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
                // Call start
                try
                {
                    update.OnStart();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

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
                    drawReceiver.OnDraw();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal protected void DoScheduledDestroyedElements(GameTime time)
        {
            //// Update all delayed elements
            //foreach (GameElement element in scheduledDestroyDelayElements)
            //{
            //    if (time != null)
            //    {
            //        // Update time
            //        element.scheduledDestroyTime -= time.ElapsedSeconds;

            //        // Check for time expired
            //        if (element.scheduledDestroyTime <= 0f)
            //            scheduleDestroyElements.Enqueue(element);
            //    }
            //    else
            //    {
            //        scheduleDestroyElements.Enqueue(element);
            //    }
            //}

            //// Update all destroyed elements
            //while (scheduleDestroyElements.Count > 0)
            //{
            //    // Remove element
            //    GameElement destroyed = scheduleDestroyElements.Dequeue();

            //    // Remove from delayed collection
            //    if (scheduledDestroyDelayElements.Contains(destroyed) == true)
            //        scheduledDestroyDelayElements.Remove(destroyed);

            //    // Destroy element
            //    TypeManager.DestroyElementTypeInstance(destroyed);


            //}
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
