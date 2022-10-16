using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DonkeyKong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public const int G_W = 920;
        public const int G_H = 680;
        private SpriteManager m_spriteManager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_spriteManager = new SpriteManager(this);
            Components.Add(m_spriteManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _graphics.PreferredBackBufferWidth = G_W;
            _graphics.PreferredBackBufferHeight = G_H;
            _graphics.ApplyChanges();
            
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            
            base.Draw(gameTime);
        }
    }
}