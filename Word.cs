using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    { 
        public string ActiveWord;
        public readonly string OriginalWord;

        public float WordWidth { get; private set; }

        public Vector2 CenterPosition { get; set; }
        
        public Word(string activeWord)
        {
            OriginalWord = activeWord;
            ActiveWord = activeWord;
        }

        private float _temporaryScaling = 1.0f;
        public float WhiteSpaceWidth => StyleSheet.DefaultFont.MeasureString(" ").X * StyleSheet.ScalingFontToWorld;
        
        public void Update()
        {
            var unscaledPixelSize = StyleSheet.DefaultFont.MeasureString(ActiveWord);
            WordWidth = unscaledPixelSize.X * StyleSheet.ScalingFontToWorld * _temporaryScaling;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(StyleSheet.DefaultFont,
                text: ActiveWord,
                position: VirtualCoords.ComputePixelPosition(CenterPosition),
                color: Color.White, 
                rotation: 0.0f,
                origin: StyleSheet.DefaultFont.MeasureString(ActiveWord) * 0.5f,
                scale: VirtualCoords.ComputePixelScale(StyleSheet.ScalingFontToWorld * _temporaryScaling),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }
    }
}