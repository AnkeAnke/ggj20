using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    { 
        public string ActiveWord;
        public readonly string OriginalWord;
        private float _goalWidth = 0;

        public float WordWidth { get; private set; }

        public Vector2 CenterPosition { get; set; }
        
        public Word(string activeWord)
        {
            OriginalWord = activeWord;
            ActiveWord = activeWord;
        }
        private float _interpolation = 0f;
        private float _temporaryScaling = 1.0f;
        public float WhiteSpaceWidth => StyleSheet.DefaultFont.MeasureString(" ").X * StyleSheet.ScalingFontToWorld;
        
        public void Update()
        {
            var unscaledPixelSize = StyleSheet.DefaultFont.MeasureString(ActiveWord);
            WordWidth = unscaledPixelSize.X * StyleSheet.ScalingFontToWorld * _temporaryScaling;
        }

        public void UpdateConstallationWord(float selectionInterpolation)
        {
            var unscaledPixelSize = StyleSheet.DefaultFont.MeasureString(ActiveWord);
            float realWidth = unscaledPixelSize.X * StyleSheet.ScalingFontToWorld * _temporaryScaling;
            if (selectionInterpolation == 0)
                _goalWidth = 0;

            if (_goalWidth == 0 && selectionInterpolation > 0)
                _goalWidth = realWidth * 1.3f;

            WordWidth = MathHelper.Lerp(realWidth, _goalWidth, selectionInterpolation);
            _interpolation = selectionInterpolation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scaleInterpolation = 1.0f + 0.2f * _interpolation;
            Color color = Color.Lerp(StyleSheet.HighlightColor, StyleSheet.BackgroundColor, _interpolation);
            spriteBatch.DrawString(StyleSheet.DefaultFont,
                text: ActiveWord,
                position: VirtualCoords.ComputePixelPosition(CenterPosition),
                color: color,
                rotation: 0.0f,
                origin: StyleSheet.DefaultFont.MeasureString(ActiveWord) * 0.5f,
                scale: VirtualCoords.ComputePixelScale(StyleSheet.ScalingFontToWorld * _temporaryScaling * scaleInterpolation),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }
    }
}