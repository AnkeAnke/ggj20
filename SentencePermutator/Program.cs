using System;
using System.Linq;
using ggj20;

namespace SentencePermutator
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary();
            dict.Load();
            
            foreach(var perm in dict.ComputeSentenceVariations("the 1princess is in another 0castle").Take(20))
                Console.WriteLine(perm.Item1 + " | " + perm.Item2);
        }
    }
}