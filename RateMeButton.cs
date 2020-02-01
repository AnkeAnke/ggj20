using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class RateMeButton
    {
        private const float Size = 0.1f;
        private readonly Vector2 _centerPosition = new Vector2(0.8f - Size * 0.5f, 0.6f - Size * 0.5f);
        
        public void Update()
        {
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(StyleSheet.StarTexture, destinationRectangle: VirtualCoords.ComputePixelRect(_centerPosition, Size), Color.White);
        }
    }
}