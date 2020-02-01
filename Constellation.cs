using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ggj20
{
    /// <summary>
    /// - frame/container for Keyboard and SwipeLine
    /// - knows about its associated Word
    /// </summary>
    public class Constellation 
    {
        public const float CONSTELLATION_WIDTH = 0.4f;
        private static readonly float SELECTION_ANIMATION_DURATION = 0.5f;

        private Vector2 centerPosition;
        private Word _associatedWord;
        private SwipeKeyboard _underlyingKeyboard;
        private float _selectionInterpolation;

        public Constellation(Word word, Vector2 center)
        {
            _associatedWord = word;
            centerPosition = center;
            _underlyingKeyboard = new SwipeKeyboard();
            _selectionInterpolation = 0;
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            Rectangle interactRect = VirtualCoords.ComputePixelRect(centerPosition - new Vector2(CONSTELLATION_WIDTH), CONSTELLATION_WIDTH);
            if (interactRect.Contains(mouseState.Position)) // TODO: or intersects word rect
            {
                _selectionInterpolation = _selectionInterpolation + (float)gameTime.ElapsedGameTime.TotalSeconds / SELECTION_ANIMATION_DURATION;
                _selectionInterpolation = Math.Min(1f, _selectionInterpolation);
            }
            else
            {
                _selectionInterpolation = _selectionInterpolation - (float)gameTime.ElapsedGameTime.TotalSeconds / SELECTION_ANIMATION_DURATION;
                _selectionInterpolation = Math.Max(0f, _selectionInterpolation);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _underlyingKeyboard.Draw(spriteBatch, _selectionInterpolation, centerPosition);
        }
    }
}