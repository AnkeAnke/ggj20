﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace ggj20
{
    /// <summary>
    /// Everything about keyboard use.
    /// * Render keyboard
    /// * Know key positions
    /// * Can tell letter distance for a point
    /// </summary>

    class SwipeKeyboard
    {

        public void Draw(SpriteBatch spriteBatch, float selectionInterpolation, Vector2 centerPosition)
        {
            // Hidden if not selected.
            if (selectionInterpolation == 0) return;
            GetBoundingBox(centerPosition, out Vector2 corner, out Vector2 size);

            Rectangle screenRect = VirtualCoords.ComputePixelRect(corner, size);
            Color offColor = StyleSheet.BackgroundColor;
            offColor.A = 0;
            Color color = Color.Lerp(offColor, StyleSheet.BackgroundColor, selectionInterpolation);
            spriteBatch.Draw(StyleSheet.KeyboardTexture, screenRect, color);
        }

        public static void GetBoundingBox(Vector2 centerPosition, out Vector2 corner, out Vector2 size)
        {
            size = new Vector2(Constellation.CONSTELLATION_WIDTH, 0);
            float scale = size.X / StyleSheet.KeyboardTexture.Width;
            size.Y = StyleSheet.KeyboardTexture.Height * scale;

            corner = new Vector2(centerPosition.X - size.X * 0.5f, centerPosition.Y - size.Y * 0.5f);
        }

        public void GetLetterDistances(Vector2 position, float[] letterDists)
        {
            Debug.Assert(letterDists.Length == LETTER_POSITIONS.Length, "Wrong input size, assumed 26.");
            for (int c = 0; c < LETTER_POSITIONS.Length; ++c)
            {
                letterDists[c] = 1.0f / (position - LETTER_POSITIONS[c]).LengthSquared();
            }
        }
        static readonly Vector2[] LETTER_POSITIONS =
        {
            new Vector2(0.097f, 0.204f), // a
            new Vector2(0.601f, 0.350f), // b
            new Vector2(0.396f, 0.350f), // c
            new Vector2(0.298f, 0.204f), // d
            new Vector2(0.248f, 0.058f), // e
            new Vector2(0.396f, 0.204f), // f
            new Vector2(0.500f, 0.204f), // g
            new Vector2(0.601f, 0.204f), // h
            new Vector2(0.754f, 0.058f), // i
            new Vector2(0.703f, 0.204f), // j
            new Vector2(0.804f, 0.204f), // k
            new Vector2(0.905f, 0.204f), // l
            new Vector2(0.804f, 0.350f), // m
            new Vector2(0.703f, 0.350f), // n
            new Vector2(0.855f, 0.058f), // o
            new Vector2(0.956f, 0.058f), // p
            new Vector2(0.046f, 0.058f), // q
            new Vector2(0.348f, 0.058f), // r
            new Vector2(0.197f, 0.204f), // s
            new Vector2(0.449f, 0.058f), // t
            new Vector2(0.652f, 0.058f), // u
            new Vector2(0.500f, 0.350f), // v
            new Vector2(0.147f, 0.058f), // w
            new Vector2(0.298f, 0.350f), // x
            new Vector2(0.548f, 0.058f), // y
            new Vector2(0.197f, 0.350f), // z
        };
    }
}
