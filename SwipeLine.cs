using System.Numerics;

namespace ggj20
{
    /// <summary>
    /// - rendering of the lines (startset & activeset) and stars (only activeset)
    /// - configure startset
    ///     - set by level config
    /// - activeset controls
    /// </summary>
    public class SwipeLine
    {
        private string _startConfiguration;
        public Vector2[] Handles { get; private set; }

        SwipeLine(string startConfiguration)
        {
            _startConfiguration = startConfiguration;
            
        }
    }
}