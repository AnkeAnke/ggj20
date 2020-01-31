using Microsoft.Xna.Framework;

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

        public Constellation(Word word)
        {
            _associatedWord = word;
        }

        public void Update()
        {
        }

        public void Draw()
        {
        }
    }
}