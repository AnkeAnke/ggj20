using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

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

        private Vector2 _centerPosition;
        private Word _associatedWord;
        private SwipeKeyboard _underlyingKeyboard;
        private SwipeLine _swipeLine;
        private float _selectionInterpolation;

        public Vector2[] ActiveConfiguration => _swipeLine.HandlePositionsRelative;
        public readonly Vector2[] OriginalConfiguration;

        public Constellation(Word word, Vector2 center)
        {
            _associatedWord = word;
            _centerPosition = center;
            _underlyingKeyboard = new SwipeKeyboard();
            _swipeLine = new SwipeLine(word.OriginalWord);
            _selectionInterpolation = 0;

            OriginalConfiguration = SwipeKeyboard.WordToSwipePositions(_associatedWord.OriginalWord).ToArray();
        }

        public void Update(GameTime gameTime, Dictionary dictionary)
        {
            var mouseState = Mouse.GetState();
            SwipeKeyboard.GetBoundingBox(_centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);

            Rectangle interactRect = VirtualCoords.ComputePixelRect(cornerKeyboard - new Vector2(CONSTELLATION_WIDTH * 0.01f), sizeKeyboard + new Vector2(CONSTELLATION_WIDTH * 0.02f));
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

            _swipeLine.Update(gameTime, _centerPosition);

            _associatedWord.ActiveWord = dictionary.ClosestWordToSwipePattern(_swipeLine.HandlePositionsRelative);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _underlyingKeyboard.Draw(spriteBatch, _selectionInterpolation, _centerPosition);
            _swipeLine.Draw(spriteBatch, _selectionInterpolation, _centerPosition);
        }
    }
}