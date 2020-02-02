
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
        private static readonly float BASE_LINE_DASH = 0.007f;
        private static readonly float SELECTION_ANIMATION_DURATION = 0.3f;
        private string _startConfiguration;
        private float[] _selectIntepolationHandles;
        private int _selectedHandle = -1;
        private MouseState _lastMouseState;
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

            float thicknessScaled = VirtualCoords.ComputePixelScale(BASE_LINE_THICKNESS);
            float dashLengthScaled = VirtualCoords.ComputePixelScale(BASE_LINE_DASH * selectionInterpolation);
            float dashTotalScaled = VirtualCoords.ComputePixelScale(BASE_LINE_DASH);
            float radiusScaled = VirtualCoords.ComputePixelScale(BASE_LINE_THICKNESS);
            for (int h = 0; h < Length; ++h)
            {

                if (h > 0)
                {
                    Vector2 pos0 = OriginalHandlePositionsRelative[h - 1] * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    Vector2 pos1 = OriginalHandlePositionsRelative[h]     * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    LineRendering.DrawLineDashed(spriteBatch, pos0, pos1, StyleSheet.BackgroundColor * selectionInterpolation, dashLengthScaled, dashTotalScaled * 2 - dashLengthScaled,
                                                 thicknessScaled * 0.7f, radiusScaled * 1.2f, radiusScaled * 1.2f);
                }

                DrawFromRelative(spriteBatch, rectKeyboard, StyleSheet.DotTexture,
                                 StyleSheet.BackgroundColor * selectionInterpolation, OriginalHandlePositionsRelative[h], BASE_LINE_THICKNESS * 1.5f, 0);

            }

            for (int h = 0; h < Length; ++h)
            {
                Color color = Color.Lerp(StyleSheet.BackgroundColor, StyleSheet.HighlightColor, selectionInterpolation);
                float radius = MathHelper.Lerp(BASE_STAR_SIZE, SELECTED_STAR_SIZE, _selectIntepolationHandles[h]);

                if (h > 0)
                {
                    Vector2 pos0 = HandlePositionsRelative[h - 1] * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    Vector2 pos1 = HandlePositionsRelative[h]     * rectKeyboard.Width + new Vector2(rectKeyboard.X, rectKeyboard.Y);
                    LineRendering.DrawLineDashed(spriteBatch, pos0, pos1, color * (0.5f + 0.5f * selectionInterpolation), dashTotalScaled + dashLengthScaled, dashTotalScaled - dashLengthScaled,
                                                 thicknessScaled, radiusScaled * 2f, radiusScaled * 2f);
                }

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
            }

            bool clicked  = mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released && rectKeyboard.Contains(mouseState.Position);
            bool released = mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed;
            float radius = SELECTED_STAR_SIZE;
            float radiusScaled = VirtualCoords.ComputePixelScale(radius);
            int closestStar = -1;

            if (_selectedHandle == -1 && rectKeyboard.Contains(mouseState.Position))
            {
                float minDist = float.MaxValue;
                for (int h = 0; h < Length; ++h)
                {
                    Point centerStar = new Point((int)(rectKeyboard.X + rectKeyboard.Width * HandlePositionsRelative[h].X + 0.5f),
                            (int)(rectKeyboard.Y + rectKeyboard.Width * HandlePositionsRelative[h].Y + 0.5f));
                    float distStar = (centerStar - mouseState.Position).ToVector2().Length();
                    if (minDist > distStar && distStar < radiusScaled)
                    {
                        minDist = distStar;
                        closestStar = h;
                        if (clicked)
                            _selectedHandle = h;
                    }
                }
            }

            for (int h = 0; h < Length; ++h)
            {

                Point centerStar = new Point((int)(rectKeyboard.X + rectKeyboard.Width * HandlePositionsRelative[h].X + 0.5f),
                                             (int)(rectKeyboard.Y + rectKeyboard.Width * HandlePositionsRelative[h].Y + 0.5f));
                if (closestStar == h || _selectedHandle == h)
                {
                    _selectIntepolationHandles[h] += (float)gameTime.ElapsedGameTime.TotalSeconds / SELECTION_ANIMATION_DURATION;
                    _selectIntepolationHandles[h] = Math.Min(1f, _selectIntepolationHandles[h]);
                }
                else if (h != closestStar)
                {
                    _selectIntepolationHandles[h] -= (float)gameTime.ElapsedGameTime.TotalSeconds / SELECTION_ANIMATION_DURATION;
                    _selectIntepolationHandles[h] = Math.Max(0f, _selectIntepolationHandles[h]);
                }

                if (_selectedHandle == h && released)
                {
                    _selectedHandle = -1;
                }
            }

            if (mouseState.LeftButton != ButtonState.Pressed)
                _selectedHandle = -1;

            _lastMouseState = mouseState;
        }
    }
}