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
        
        public void Draw(SpriteBatch spriteBatch, float swipeErrorAllowedPercentage)
        {
            var tailAspect = (float) StyleSheet.ShootingTailTexture.Width / StyleSheet.ShootingTailTexture.Height;
            var tailSize = new Vector2(Size * tailAspect, Size);
            var offset = new Vector2(-tailSize.X * 0.5f, 0.0f);

            spriteBatch.Draw(StyleSheet.ShootingTailTexture, destinationRectangle:
                VirtualCoords.ComputePixelRect(_centerPosition + offset, tailSize),
                Color.White * swipeErrorAllowedPercentage);
            
            spriteBatch.Draw(StyleSheet.ShootingStarTexture, destinationRectangle: VirtualCoords.ComputePixelRect(_centerPosition + offset, tailSize), Color.White);
            
            if (swipeErrorAllowedPercentage > 0.0001f)
                spriteBatch.Draw(StyleSheet.ShootingButtonTexture, destinationRectangle: VirtualCoords.ComputePixelRect(_centerPosition + offset, tailSize), Color.DarkGreen);
        }
    }
}