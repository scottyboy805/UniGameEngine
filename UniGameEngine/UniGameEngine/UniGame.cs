using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UniGameEngine
{
    public abstract class UniGame : Game
    {
        // Private
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Properties
        public abstract bool IsEditor { get; }

        public abstract bool IsPlaying { get; }

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
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
