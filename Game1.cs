using System;
using System.Diagnostics;
using System.Linq;
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
        private RateMeButton _rateMeButton;
        
        private float _currentSwipeError;
        private float _currentSwipeErrorAllowedPercentage;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            _activeLevel = new Level();
            _rateMeButton = new RateMeButton();
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
            
            _activeLevel.Update(gameTime, _dictionary);
            
            // compute current score
            _currentSwipeError = _activeLevel.ActiveConstellations.Sum(
                constellation =>
                    _dictionary.SwipePatternDifference(constellation.ActiveConfiguration, constellation.OriginalConfiguration)
            );
            _currentSwipeErrorAllowedPercentage = Math.Max(0.0f, 1.0f - _currentSwipeError / _activeLevel.MaxSwipeError);
            
            _rateMeButton.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(StyleSheet.ClearColor);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _activeLevel.Draw(_spriteBatch);
            _rateMeButton.Draw(_spriteBatch, _currentSwipeErrorAllowedPercentage);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
