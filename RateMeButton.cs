using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
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

        struct Sparcle
        {
            public Vector2 Position;
            public float Size;
            public Color Color;
        }
        
        private List<Sparcle> sparcles = new List<Sparcle>();

        public RateMeButton()
        {
            var rand = new Random(123);
            const int numSparcles = 250;
            const float sparcleMinSize = Size * 0.015f;
            const float sparcleMaxSize = Size * 0.045f;
            for (int i = 0; i < numSparcles; ++i)
            {
                var size = MathHelper.Lerp(sparcleMinSize, sparcleMaxSize, (float)rand.NextDouble());
                
                var randquad = new Vector2(MathF.Sqrt((float)rand.NextDouble()), (float)rand.NextDouble());
                var pos = randquad;
                
                // spiky to the right
                pos *= new Vector2(1.0f, 1.0f - pos.X * 0.8f);
                pos += new Vector2(0.0f, 0.5f * pos.X);
                
                // stretch and bring into position
                pos *= new Vector2(Size * StyleSheet.ShootingTailTexture.Width / StyleSheet.ShootingTailTexture.Height, Size);
                pos -= new Vector2(Size * StyleSheet.ShootingTailTexture.Width / StyleSheet.ShootingTailTexture.Height * 0.5f + 0.05f, 0.005f);
                pos += _centerPosition;

                sparcles.Add(new Sparcle()
                {
                    Position = pos,
                    Size = size,
                    Color = Color.White * ((float)rand.NextDouble() * 0.8f + 0.1f) * MathF.Pow(randquad.Y * 0.5f + 0.5f, 0.4f)
                });
            }
        }
        
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
        
        public void Draw(SpriteBatch spriteBatch, float swipeErrorAllowedPercentage, bool showPlayButton)
        {
            var area = DrawArea();

            for (var index = 0; index < Math.Min(sparcles.Count, sparcles.Count * swipeErrorAllowedPercentage + 0.5f); index++)
            {
                var sparcle = sparcles[index];
                spriteBatch.Draw(StyleSheet.DotTexture,
                    VirtualCoords.ComputePixelRect_Centered(sparcle.Position, sparcle.Size), sparcle.Color);
            }

            //spriteBatch.Draw(StyleSheet.ShootingTailTexture, area, Color.White * swipeErrorAllowedPercentage);
            
            spriteBatch.Draw(StyleSheet.ShootingStarTexture, destinationRectangle: area, Color.White);
            
            if (showPlayButton && swipeErrorAllowedPercentage > 0.0f)
                spriteBatch.Draw(StyleSheet.ShootingButtonTexture, destinationRectangle: area, Color.DarkGreen);
        }
    }
}