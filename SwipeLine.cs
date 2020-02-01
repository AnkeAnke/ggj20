
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ggj20
{
    /// <summary>
    /// - rendering of the lines (startset & activeset) and stars (only activeset)
    /// - configure startset
    ///     - set by level config
    /// - activeset controls
    /// </summary>
    public class SwipeLine
    {
        private static readonly float BASE_STAR_SIZE = 0.01f;
        private static readonly float SELECTED_STAR_SIZE = 0.03f;
        private static readonly float SELECTION_ANIMATION_DURATION = 0.3f;
        private string _startConfiguration;
        private float[] _selectIntepolationHandles;
        private int _selectedHandle = -1;
        private Point _selectedLastMousePos;
        public Vector2[] HandlePositionsRelative { get; private set; }
        public int Length
        {
            get { return _startConfiguration.Length; }
        }

        public SwipeLine(string startConfiguration)
        {
            _startConfiguration = startConfiguration;
            HandlePositionsRelative = new Vector2[Length];
            _selectIntepolationHandles = new float[Length];

            for (int h = 0; h < Length; ++h)
            {
                char letter = char.ToLower(_startConfiguration[h]);
                HandlePositionsRelative[h] = SwipeKeyboard.LETTER_POSITIONS[letter - 'a'];
                _selectIntepolationHandles[h] = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float selectionInterpolation, Vector2 centerPosition)
        {
            SwipeKeyboard.GetBoundingBox(centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);
            Rectangle rectKeyboard = VirtualCoords.ComputePixelRect(cornerKeyboard, sizeKeyboard);
            for (int h = 0; h < Length; ++h)
            {
                float radius = MathHelper.Lerp(BASE_STAR_SIZE, SELECTED_STAR_SIZE, _selectIntepolationHandles[h]);
                float radiusScaled = VirtualCoords.ComputePixelScale(radius);

                Point cornerStar = new Point((int)(rectKeyboard.X + rectKeyboard.Width  * HandlePositionsRelative[h].X - radiusScaled + 0.5f)
                                           , (int)(rectKeyboard.Y + rectKeyboard.Width * HandlePositionsRelative[h].Y - radiusScaled + 0.5f));
                Rectangle starRect = new Rectangle((int)cornerStar.X, (int)cornerStar.Y, (int)radiusScaled * 2, (int)radiusScaled * 2);
                // Rectangle starRect = VirtualCoords.ComputePixelRect_Centered(Handles[h], radius);
                Color color = Color.Lerp(StyleSheet.BackgroundColor, StyleSheet.HighlightColor, selectionInterpolation);

                spriteBatch.Draw(
                    texture: StyleSheet.StarTexture,
                    destinationRectangle: starRect,
                    color: color
                    //rotation: _selectHandles[h],
                    //origin: new Vector2(radiusScaled * 0.5f, radiusScaled * 0.5f)
                    );
            }
        }

        public void Update(GameTime gameTime, Vector2 centerPosition)
        {
            var mouseState = Mouse.GetState();
            SwipeKeyboard.GetBoundingBox(centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);
            Rectangle rectKeyboard = VirtualCoords.ComputePixelRect(cornerKeyboard, sizeKeyboard);
            if (_selectedHandle != -1)
            {
                //Point mouseDist = (mouseState.Position - _selectedLastMousePos);
                HandlePositionsRelative[_selectedHandle] = (mouseState.Position - VirtualCoords.FieldPixelOffset).ToVector2() / VirtualCoords.FieldPixelSizeMin;
                    //+= new Vector2((float)mouseDist.X / rectKeyboard.Width, (float)mouseDist.Y / rectKeyboard.Height);
                _selectedLastMousePos = mouseState.Position;
            }

            for (int h = 0; h < Length; ++h)
            {
                float radius = SELECTED_STAR_SIZE;
                float radiusScaled = VirtualCoords.ComputePixelScale(radius);

                Point centerStar = new Point((int)(rectKeyboard.X + rectKeyboard.Width * HandlePositionsRelative[h].X + 0.5f)
                                           , (int)(rectKeyboard.Y + rectKeyboard.Width * HandlePositionsRelative[h].Y + 0.5f));

                bool mouseIntersectsStar = (centerStar - mouseState.Position).ToVector2().Length() < radiusScaled;

                if (_selectedHandle == h && !mouseIntersectsStar)
                {
                    _selectedHandle = -1;
                }

                if (_selectedHandle == -1 && mouseIntersectsStar)
                {
                    _selectIntepolationHandles[h] = _selectIntepolationHandles[h]
                                                    + (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f / SELECTION_ANIMATION_DURATION;
                    _selectIntepolationHandles[h] = Math.Min(0.5f, _selectIntepolationHandles[h]);

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _selectIntepolationHandles[h] = 1;
                        _selectedHandle = h;
                    }
                }
                else if (!mouseIntersectsStar || _selectedHandle == -1)
                {
                    _selectIntepolationHandles[h] = _selectIntepolationHandles[h]
                                                    - (float)gameTime.ElapsedGameTime.TotalSeconds / SELECTION_ANIMATION_DURATION;
                    _selectIntepolationHandles[h] = Math.Max(0f, _selectIntepolationHandles[h]);
                }
            }

            if (mouseState.LeftButton != ButtonState.Pressed)
                _selectedHandle = -1;
        }
    }
}