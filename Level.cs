using System.Collections.Generic;
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
        public IEnumerable<Constellation> ActiveConstellations => _constellations.Where(c => c != null); 
        private Constellation[] _constellations;
        private Word[] _words;
        private Dictionary<string, int> _ratingTable = new Dictionary<string, int>();
        
        public float MaxSwipeError { get; private set; }

        public Level()
        {
        }

        public string CurrentSentence => string.Join(' ', _words.Select(w => w.ActiveWord).ToArray());
        public string OriginalSentence => string.Join(' ', _words.Select(w => w.OriginalWord).ToArray());
        
        public int CurrentSentenceRating => _ratingTable.TryGetValue(CurrentSentence.ToLower(), out var rating) ? rating : 0;

        public void LoadLevel(string filename)
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
            
            // load MaxSwipeError
            MaxSwipeError = float.Parse(lines[1]);
            
            // load sentence ratings
            foreach (var ratingString in lines.Skip(2))
            {
                var parts = ratingString.Split(' ');
                var rating = int.Parse(parts[0]);
                var ratedSentence = string.Join(' ', parts.Skip(1));
                _ratingTable.Add(ratedSentence.ToLower(), rating);
            }
        }

        public void Update(GameTime gameTime, Dictionary dictionary)
        {
            // space out the words
            {
                var totalSentenceWidth = _words.Select(w => w.WordWidth).Sum() + (_words.Length - 1) * _words[0].WhiteSpaceWidth;
                float curX = (VirtualCoords.RELATIVE_MAX.X - totalSentenceWidth) * 0.5f;
                                     
                foreach (var word in _words)
                {
                    curX += word.WordWidth * 0.5f;
                    word.CenterPosition = new Vector2(curX, VirtualCoords.RELATIVE_MAX.Y * 0.5f);
                    curX += word.WordWidth * 0.5f + word.WhiteSpaceWidth;
                }
            }
            
            foreach (var word in _words)
                word.Update();
            foreach (var constellation in _constellations)
                constellation?.Update(gameTime, dictionary);
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
            new Vector2(0.80f, 0.85f),
            new Vector2(1.05f, 0.3f),
            new Vector2(0.30f, 0.7f),
            new Vector2(1.30f, 0.7f),
        };
    }
}