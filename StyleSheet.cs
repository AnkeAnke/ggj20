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

        public static SpriteFont DefaultFont
        {
            get; private set;
        }

        public static float ScalingFontToWorld
        {
            get; private set;
        }

        public static readonly Color ClearColor = new Color(5,5,10);
        public static readonly Color BackgroundColor = Color.LightBlue;

        public static Color HighlightColor
        {
            get; private set;
        }

        public static Texture2D KeyboardTexture
        {
            get; private set;
        }

        public static Texture2D StarTexture
        {
            get; private set;
        }

        public static void LoadContent(ContentManager content)
        {
            KeyboardTexture = content.Load<Texture2D>("Keyboard/keyboard");
            StarTexture     = content.Load<Texture2D>("Keyboard/star");
            DefaultFont     = content.Load<SpriteFont>("DefaultFont");
            ScalingFontToWorld = TEXT_HEIGHT / DefaultFont.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").Y;
            HighlightColor = Color.White;
        }
    }
}
