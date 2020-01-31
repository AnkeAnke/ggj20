using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    {
        private string _word;
        private SpriteFont _font;

        public Word(string word)
        {
            _word = word;
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("DefaultFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _word, Vector2.One, Color.White);
        }
    }
}