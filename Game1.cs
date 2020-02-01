using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ggj20
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Level _activeLevel;
        private Dictionary _dictionary;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            _activeLevel = new Level();
        }

        protected override void Initialize()
        {
            VirtualCoords.OnResize(new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            Window.ClientSizeChanged += (sender, args) => VirtualCoords.OnResize(this.Window.ClientBounds.Size);
            _dictionary = new Dictionary();
            _dictionary.Load();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            StyleSheet.LoadContent(Content);
			_activeLevel.LoadLevel("Content/level1.lvl");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _activeLevel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(StyleSheet.ClearColor);

            _spriteBatch.Begin(blendState: BlendState.Additive);
            _activeLevel.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
