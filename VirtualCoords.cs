using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ggj20
{
    static class VirtualCoords
    {
        /// <summary>
        /// all relative coordinates are from 0 to RELATIVE_MAX
        /// </summary>
        static public readonly Vector2 RELATIVE_MAX = new Vector2(1.6f, 1.0f);

        static public readonly float RELATIVECOR_ASPECT_RATIO = (float)RELATIVE_MAX.X / RELATIVE_MAX.Y;

        /// <summary>
        /// size of the playing area in pixel
        /// </summary>
        private static Point FieldPixelSize
        {
            get { return fieldSize_pixel; }
        }
        private static Point fieldSize_pixel;

        private static int FieldPixelSizeMin
        {
            get { return Math.Min(fieldSize_pixel.X, fieldSize_pixel.Y);  }
        }


        /// <summary>
        /// offset in x and y of the playing area in pixel
        /// </summary>
        public static Point FieldPixelOffset
        {
            get { return fieldOffset_pixel; }
        }
        private static Point fieldOffset_pixel;

        /// <summary>
        /// the field area in pixels as rectangle
        /// </summary>
        private static Rectangle fieldPixelRectangle;

        /// <summary>
        /// computes pixelrect on damage map from relative cordinates
        /// </summary>
        /// <param name="relativePosition">position in relative game cor</param>
        /// <param name="uniformSize">uniform size in relative game cord</param>
        public static Rectangle ComputePixelRect(Vector2 position, float uniformSize)
        {
            return ComputePixelRect(position, new Vector2(uniformSize, uniformSize));
        }

        public static Rectangle ComputePixelRect(Vector2 position, Vector2 size)
        {
            Vector2 outSize = ComputePixelScale(size) + new Vector2(0.5f);
            Vector2 outpos = ComputePixelPosition(position) + new Vector2(0.5f);
            return new Rectangle((int)outpos.X, (int)outpos.Y, (int)outSize.X, (int)outSize.Y);
        }

        public static Vector2 ComputePixelPosition(Vector2 position)
        {
            return new Vector2(position.X / RELATIVE_MAX.X * FieldPixelSize.X + FieldPixelOffset.X,
                               position.Y / RELATIVE_MAX.Y * FieldPixelSize.Y + FieldPixelOffset.Y);
        }

        public static float ComputePixelScale(float relativeScale)
        {
            return relativeScale * FieldPixelSizeMin;
        }
        public static float ComputeTextureScale(float relativeSize, int textureSize)
        {
            return ComputePixelScale(relativeSize) / textureSize;
        }
        public static Vector2 ComputePixelScale(Vector2 relativeScale)
        {
            return new Vector2(relativeScale.X * FieldPixelSizeMin,
                               relativeScale.Y * FieldPixelSizeMin);
        }

        /// <summary>
        /// computes centered pixelrect on damage map from relative cordinates
        /// </summary>
        /// <param name="relativePosition">position in relative game cor</param>
        /// <param name="uniformSize">uniform size in relative game cord</param>
        public static Rectangle ComputePixelRect_Centered(Vector2 position, float uniformSize)
        {
            return ComputePixelRect_Centered(position, new Vector2(uniformSize / RELATIVECOR_ASPECT_RATIO, uniformSize));
        }

        public static Rectangle ComputePixelRect_Centered(Vector2 position, Vector2 size)
        {
            int rectSizeX = (int)(size.X * fieldSize_pixel.X + 0.5f);
            int halfSizeX = rectSizeX / 2;
            int rectSizeY = (int)(size.Y * fieldSize_pixel.Y + 0.5f);
            int halfSizeY = rectSizeY / 2;

            int rectx = (int)(position.X / RELATIVE_MAX.X * FieldPixelSize.X + FieldPixelOffset.X);
            int recty = (int)(position.Y / RELATIVE_MAX.Y * FieldPixelSize.Y + FieldPixelOffset.Y);
            
            return new Rectangle(rectx - halfSizeX, recty - halfSizeY, rectSizeX, rectSizeY);
        }
        
        public static void OnResize(Point windowSize)
        {
            float sizeY = windowSize.X / RELATIVECOR_ASPECT_RATIO;
            fieldSize_pixel = new Point((int)(sizeY * RELATIVECOR_ASPECT_RATIO), (int)sizeY);

            fieldOffset_pixel = new Point(windowSize.X - fieldSize_pixel.X, windowSize.Y - fieldSize_pixel.Y);
            fieldOffset_pixel.X /= 2;
            fieldOffset_pixel.Y /= 2;

            fieldPixelRectangle = new Rectangle(fieldOffset_pixel.X, fieldOffset_pixel.Y, fieldSize_pixel.X, fieldSize_pixel.Y);
        }
    }
}
