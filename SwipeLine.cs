
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
        private static readonly float SELECTED_STAR_SIZE = 0.02f;
        private static readonly float BASE_LINE_THICKNESS = 0.005f;
        private static readonly float SELECTION_ANIMATION_DURATION = 0.3f;
        private string _startConfiguration;
        private float[] _selectIntepolationHandles;
        private int _selectedHandle = -1;
        private Point _selectedLastMousePos;
        public Vector2[] HandlePositionsRelative { get; private set; }
        public Vector2[] OriginalHandlePositionsRelative { get; private set; }
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
            OriginalHandlePositionsRelative = (Vector2[])HandlePositionsRelative.Clone();
        }

        public void Draw(SpriteBatch spriteBatch, float selectionInterpolation, Vector2 centerPosition)
        {
            SwipeKeyboard.GetBoundingBox(centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);
            Rectangle rectKeyboard = VirtualCoords.ComputePixelRect(cornerKeyboard, sizeKeyboard);
            for (int h = 0; h < Length; ++h)
            {
                float radius = MathHelper.Lerp(BASE_STAR_SIZE, SELECTED_STAR_SIZE, _selectIntepolationHandles[h]);

                Color color = Color.Lerp(StyleSheet.BackgroundColor, StyleSheet.HighlightColor, selectionInterpolation);


                float thicknessScaled = VirtualCoords.ComputePixelScale(BASE_LINE_THICKNESS);
                if (h > 0)
                {
                    Vector2 pos0 = HandlePositionsRelative[h - 1] * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    Vector2 pos1 = HandlePositionsRelative[h] * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    LineRendering.DrawLine(spriteBatch, pos0, pos1, Color.Red, thicknessScaled);
                }

                DrawFromRelative(spriteBatch, rectKeyboard, StyleSheet.DotTexture,
                                 StyleSheet.BackgroundColor * selectionInterpolation, OriginalHandlePositionsRelative[h], BASE_LINE_THICKNESS * 2, 0);

                DrawFromRelative(spriteBatch, rectKeyboard, StyleSheet.StarTexture,
                                 color, HandlePositionsRelative[h], radius * 2, _selectIntepolationHandles[h] + selectionInterpolation + h);
            }
        }

        private void GetBoundingBoxFromRelative(Rectangle rectKeyboard, Texture2D texture, Vector2 relativePosition, float sizeX,
                                                out Vector2 center, out Vector2 scale)
        {
            scale = new Vector2(VirtualCoords.ComputePixelScale(sizeX) / texture.Width);

            center = new Vector2(rectKeyboard.X + 0.5f, rectKeyboard.Y + 0.5f) + relativePosition * rectKeyboard.Width;
        }

        private void DrawFromRelative(SpriteBatch spriteBatch, Rectangle rectKeyboard, Texture2D texture, Color color, Vector2 relativePosition, float sizeX, float rotation)
        {
            GetBoundingBoxFromRelative(rectKeyboard, StyleSheet.StarTexture, relativePosition, sizeX, out Vector2 center, out Vector2 scale);
            spriteBatch.Draw(
                texture: texture,
                position: center,
                sourceRectangle: null,
                color: color,
                rotation: rotation,
                origin: new Vector2(StyleSheet.StarTexture.Width * 0.5f, StyleSheet.StarTexture.Height * 0.5f),
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 1);
        }

        public void Update(GameTime gameTime, Vector2 centerPosition)
        {
            var mouseState = Mouse.GetState();
            SwipeKeyboard.GetBoundingBox(centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);
            Rectangle rectKeyboard = VirtualCoords.ComputePixelRect(cornerKeyboard, sizeKeyboard);
            if (_selectedHandle != -1)
            {
                HandlePositionsRelative[_selectedHandle] = (mouseState.Position - rectKeyboard.Location).ToVector2() / rectKeyboard.Width;
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