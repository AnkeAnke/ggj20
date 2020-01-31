using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ggj20
{
    public class Level
    {
        private Constellation[] _constellations;
        private Word[] _words;
        
        public Level(string filename)
        {
            var contents = File.ReadAllText(filename);
            var lines = contents.Split('\n');
            var startConfigExpression = lines[0].Trim();

            _constellations = new Constellation[5];
            foreach (var wordString in startConfigExpression.Split(" "))
            {
                if (int.TryParse(wordString[0].ToString(), out var constellationNumber))
                {
                    Debug.Assert(constellationNumber < _constellations.Length);
                    Debug.Assert(_constellations[constellationNumber] == null);
                    //_constellations
                }
            }
        }

        public void Update()
        {
            foreach (var constellation in _constellations)
                constellation?.Update();
        }

        public void Draw()
        {
            foreach (var constellation in _constellations)
                constellation?.Draw();
        }
    }
}