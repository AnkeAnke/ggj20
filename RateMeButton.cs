using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ggj20
{
    public class RateMeButton
    {
        private const float Size = 0.1f;
        private readonly Vector2 _centerPosition = new Vector2(0.8f - Size * 0.5f, 0.6f - Size * 0.5f);
        private ButtonState _mouseStateLastFrame = ButtonState.Released;
        public bool IsPressed { get; private set; } = false;

        public void Update()
        {
            var mouseState = Mouse.GetState();
            SwipeKeyboard.GetBoundingBox(_centerPosition, out Vector2 cornerKeyboard, out Vector2 sizeKeyboard);

            Rectangle interactRect = DrawArea();
            IsPressed = mouseState.LeftButton == ButtonState.Pressed && _mouseStateLastFrame == ButtonState.Released && interactRect.Contains(mouseState.Position);

            _mouseStateLastFrame = mouseState.LeftButton;
        }

        private Rectangle DrawArea()
        {
            var tailAspect = (float) StyleSheet.ShootingTailTexture.Width / StyleSheet.ShootingTailTexture.Height;
            var tailSize = new Vector2(Size * tailAspect, Size);
            var offset = new Vector2(-tailSize.X * 0.5f, 0.0f);

            return VirtualCoords.ComputePixelRect(_centerPosition + offset, tailSize);
        }
        
        public void Draw(SpriteBatch spriteBatch, float swipeErrorAllowedPercentage)
        {
            var area = DrawArea();

            spriteBatch.Draw(StyleSheet.ShootingTailTexture, area,
                Color.White * swipeErrorAllowedPercentage);
            
            spriteBatch.Draw(StyleSheet.ShootingStarTexture, destinationRectangle: area, Color.White);
            
            if (swipeErrorAllowedPercentage > 0.0001f)
                spriteBatch.Draw(StyleSheet.ShootingButtonTexture, destinationRectangle: area, Color.DarkGreen);
        }
    }
}