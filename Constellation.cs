using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    /// <summary>
    /// - frame/container for Keyboard and SwipeLine
    /// - knows about its associated Word
    /// </summary>
    public class Constellation 
    {
        public const float CONSTELLATION_WIDTH = 0.4f;
        private Vector2 centerPosition;
        private Word _associatedWord;
        private SwipeKeyboard _underlyingKeyboard;

        public Constellation(Word word, Vector2 center)
        {
            _associatedWord = word;
            centerPosition = center;
            _underlyingKeyboard = new SwipeKeyboard();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _underlyingKeyboard.Draw(spriteBatch, centerPosition);
        }

        public void LoadContent(ContentManager content)
        {
            _underlyingKeyboard.LoadContent(content);
        }
    }
}