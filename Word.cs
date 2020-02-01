using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    {
        public string WordString => _word;
        private string _word;

        public float WordWidth { get; private set; }

        public Vector2 Position { get; set; }
        
        public Word(string word)
        {
            _word = word;
        }

        private float _temporaryScaling = 1.0f;
        public float WhiteSpaceWidth => StyleSheet.DefaultFont.MeasureString(" ").X * WordWidth * _temporaryScaling; // todo
        
        public void Update()
        {
            var unscaledPixelSize = StyleSheet.DefaultFont.MeasureString(_word);
            WordWidth = unscaledPixelSize.X * StyleSheet.ScalingFontToWorld;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(StyleSheet.DefaultFont,
                text: _word,
                position: VirtualCoords.ComputePixelPosition(Position),
                color: Color.White, 
                rotation: 1.0f,
                origin: Vector2.One * 0.5f,
                scale: VirtualCoords.ComputePixelScale(StyleSheet.ScalingFontToWorld) * _temporaryScaling,
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }
    }
}