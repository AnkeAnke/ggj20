using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ggj20
{
    static class LineRendering
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            Vector2 origin = new Vector2(0f, StyleSheet.LineTexture.Height * 0.5f);
            Vector2 scale = new Vector2(length, thickness);
            spriteBatch.Draw(StyleSheet.LineTexture, point, null, color, angle, origin, scale / StyleSheet.LineTexture.Width, SpriteEffects.None, 0);
        }
        public static void DrawLineDashed(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float dashLength, float dashDist,
                                          float thickness = 1f, float offsetBegin = 0f, float offsetEnd = 0f)
        {
            float distance = Vector2.Distance(point1, point2) - offsetEnd - offsetBegin;
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            Vector2 dirNormal = (point2 - point1);
            dirNormal.Normalize();

            int numDashes = (int)Math.Ceiling((distance - dashLength) / (2.0f * (dashLength + dashDist)));
            numDashes *= 2;
            float distOffset = (distance - ((dashLength + dashDist) * numDashes + dashLength)) * 0.5f + offsetBegin;
            numDashes++;
            for (int d = 0; d < numDashes; ++d)
            {
                float distFrom = Math.Max(offsetBegin, distOffset);
                float distTo = Math.Min(distance + offsetBegin, distOffset + dashLength);
                float scale = distTo - distFrom;
                Vector2 dashFrom = point1 + dirNormal * distFrom;
                DrawLine(spriteBatch, dashFrom, scale, angle, color, thickness);
                distOffset += dashLength + dashDist;
            }
        }
    }
}
