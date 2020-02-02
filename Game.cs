using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ggj20
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public enum State
        {
            Playing,
            Rating,
        }
        
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Level _activeLevel;
        private Dictionary _dictionary;
        private RateMeButton _rateMeButton;
        private State _currentState;
        
        private float _currentSwipeError;
        private float _currentSwipeErrorAllowedPercentage;

        private string[] levelNames = new[] {"Content/level1.lvl", "Content/level2.lvl "};
        private int currentLevel = 0;

        public Game()
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
            _activeLevel = new Level();
            _activeLevel.LoadLevel(levelNames[currentLevel]);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (_currentState == State.Playing)
                _activeLevel.Update(gameTime, _dictionary);
            
            // compute current score
            _currentSwipeError = _activeLevel.ActiveConstellations.Sum(
                constellation =>
                    _dictionary.SwipePatternDifference(constellation.ActiveConfiguration, constellation.OriginalConfiguration)
            );
            _currentSwipeErrorAllowedPercentage = Math.Max(0.0f, 1.0f - _currentSwipeError / _activeLevel.MaxSwipeError);
            if (_activeLevel.CurrentSentence == _activeLevel.OriginalSentence)
                _currentSwipeErrorAllowedPercentage = 0.0f;

            _rateMeButton.Update();
            switch (_currentState)
            {
                case State.Playing when _rateMeButton.IsPressed:
                {
                    _currentState = State.Rating;
                    break;
                }
                case State.Rating when _rateMeButton.IsPressed:
                {
                    if (_activeLevel.CurrentSentenceRating >= 2)
                        currentLevel = (currentLevel + 1) % levelNames.Length;
                    _activeLevel = new Level();
                    _activeLevel.LoadLevel(levelNames[currentLevel]);
                    _currentState = State.Playing;
                    break;
                }
            }

            base.Update(gameTime);
        }

        private void DrawRatingStars()
        {
            var rating = _activeLevel.CurrentSentenceRating;
            if (rating == 0)
            {
                const float ratingStarYOffset = 0.3f;
                const float ratingStarSize = 0.1f;
                const float ratingStarSpacing = 0.02f;
                var totalWidth = ratingStarSize * rating + (rating - 1) * ratingStarSpacing;
                var currentX = (VirtualCoords.RELATIVE_MAX.X - totalWidth) * 0.5f;

                for (int i = 0; i < rating; ++i)
                {
                    currentX += ratingStarSize * 0.5f;
                    _spriteBatch.Draw(StyleSheet.StarTexture, destinationRectangle:
                        VirtualCoords.ComputePixelRect_Centered(new Vector2(currentX, ratingStarYOffset),
                            ratingStarSize),
                        Color.LightYellow);
                    currentX += ratingStarSize * 0.5f + ratingStarSpacing;
                }
            }

            var text = rating switch
            {
                0 => "Are you sure? Try again!",
                1 => "Better than before, but not good enough!",
                2 => "Pretty good, on to the next level",
                3 => "Excellent",
                _ => ""
            };

            var centerPos = new Vector2(0.8f, 0.8f);
            _spriteBatch.DrawString(StyleSheet.DefaultFont,
                text: text,
                position: VirtualCoords.ComputePixelPosition(centerPos),
                color: Color.White, 
                rotation: 0.0f,
                origin: StyleSheet.DefaultFont.MeasureString(text) * 0.5f,
                scale: VirtualCoords.ComputePixelScale(StyleSheet.ScalingFontToWorld),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(StyleSheet.ClearColor);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _activeLevel.Draw(_spriteBatch, _currentState);
            
            
            _rateMeButton.Draw(_spriteBatch, _currentSwipeErrorAllowedPercentage);
            if (_currentState == State.Rating)
            {
                DrawRatingStars();
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
