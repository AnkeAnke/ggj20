using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ggj20
{
    static class StyleSheet
    {
        public static readonly float TEXT_HEIGHT = 0.1f;

        public static SpriteFont DefaultFont { get; private set; }

        public static float ScalingFontToWorld { get; private set; }

        public static readonly Color ClearColor = new Color(5,5,10);
        public static readonly Color BackgroundColor = Color.DeepSkyBlue;

        public static Color HighlightColor { get; private set; }

        public static Texture2D KeyboardTexture { get; private set; }

        public static Texture2D StarTexture { get; private set; }
        public static Texture2D ShootingStarTexture { get; private set; }
        public static Texture2D ShootingTailTexture { get; private set; }
        public static Texture2D ShootingButtonTexture { get; private set; }

        public static Texture2D LineTexture { get; private set; }

        public static Texture2D DotTexture { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            KeyboardTexture = content.Load<Texture2D>("Keyboard/keyboard");
            StarTexture     = content.Load<Texture2D>("Keyboard/star");
            DefaultFont     = content.Load<SpriteFont>("DefaultFont");
            LineTexture     = content.Load<Texture2D>("Keyboard/line");
            DotTexture      = content.Load<Texture2D>("Keyboard/circle");
            ShootingStarTexture = content.Load<Texture2D>("ShootingStar/shootingStar");
            ShootingTailTexture = content.Load<Texture2D>("ShootingStar/shootingTail");
            ShootingButtonTexture = content.Load<Texture2D>("ShootingStar/shootingButtonWhite");

            ScalingFontToWorld = TEXT_HEIGHT / DefaultFont.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").Y;
            HighlightColor = Color.White;
        }
    }
}
