using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj20
{
    public class Level
    {
        private Constellation[] _constellations;
        private Word[] _words;

        public Level(string filename)
        {
            LoadLevel(filename);
        }
        
        private void LoadLevel(string filename)
        {
            var contents = File.ReadAllText(filename);
            var lines = contents.Split('\n');
            var startConfigExpression = lines[0].Trim();

            var wordStrings = startConfigExpression.Split(" ");
            _constellations = new Constellation[5];
            _words = new Word[wordStrings.Length];
            for (var i = 0; i < wordStrings.Length; i++)
            {
                var wordString = wordStrings[i];
                _words[i] = new Word(wordString.TrimStart('0', '1', '2', '3', '4'));
                if (int.TryParse(wordString[0].ToString(), out var constellationNumber))
                {
                    Debug.Assert(constellationNumber < _constellations.Length);
                    Debug.Assert(_constellations[constellationNumber] == null);
                    _constellations[constellationNumber] = new Constellation(_words[i], CONSTELLATION_CENTERS[constellationNumber]);
                }
            }
        }
        
        public void LoadContent(ContentManager content)
        {
            foreach (var word in _words)
                word.LoadContent(content);
            foreach (var constellation in _constellations)
                constellation?.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var constellation in _constellations)
                constellation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var word in _words)
                word.Draw(spriteBatch);
            foreach (var constellation in _constellations)
                constellation?.Draw(spriteBatch);
        }

        static readonly Vector2[] CONSTELLATION_CENTERS = new Vector2[]
        {
            new Vector2(0.55f, 0.3f),
            new Vector2(0.80f, 0.7f),
            new Vector2(1.05f, 0.3f),
            new Vector2(0.30f, 0.7f),
            new Vector2(1.30f, 0.7f),
        };
    }
}