using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
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
        Texture texture;
        Vector2[] letterPositions = new Vector2[26];
        Vector2 offset;
        float scale;

        void Draw() { }
        void Update(){}

        // GetLetterDistances
        // 

    }
}
