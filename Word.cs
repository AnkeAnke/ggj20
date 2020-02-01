using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    {
        public string WordString => _word;
        private string _word;
        public readonly string OriginalWord;

        public float WordWidth { get; private set; }

        public Vector2 CenterPosition { get; set; }
        
        public Word(string word)
        {
            OriginalWord = _word;
            _word = word;
        }

        private float _temporaryScaling = 1.0f;
        public float WhiteSpaceWidth => StyleSheet.DefaultFont.MeasureString(" ").X * StyleSheet.ScalingFontToWorld;
        
        public void Update()
        {
            var unscaledPixelSize = StyleSheet.DefaultFont.MeasureString(_word);
            WordWidth = unscaledPixelSize.X * StyleSheet.ScalingFontToWorld * _temporaryScaling;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(StyleSheet.DefaultFont,
                text: _word,
                position: VirtualCoords.ComputePixelPosition(CenterPosition),
                color: Color.White, 
                rotation: 0.0f,
                origin: StyleSheet.DefaultFont.MeasureString(_word) * 0.5f,
                scale: VirtualCoords.ComputePixelScale(StyleSheet.ScalingFontToWorld * _temporaryScaling),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }
    }
}