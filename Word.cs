using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Word
    {
        private string _word;

        public Word(string word)
        {
            _word = word;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(StyleSheet.DefaultFont, _word, Vector2.One, Color.White);
        }
    }
}