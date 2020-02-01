using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ggj20
{
    public class Dictionary
    {
        private List<List<string>> _wordsByLength;
        
        public void Load()
        {
            var dict = File.ReadLines("Content/dictionary.txt").ToList();
            _wordsByLength = new List<List<string>>();
            for (int i = 0; i < 10; ++i)
            {
                _wordsByLength.Add(dict.Where(w => w.Length == i).ToList());
            }
            
            // UNIT TEST
            var testWord = "princess";
            var testSwipePattern = new float[SwipeKeyboard.LETTER_POSITIONS.Length, testWord.Length];
            for (int li = 0; li < testWord.Length; ++li)
                testSwipePattern[char.ToLower(testWord[li]) - 'a', li] = 1.0f;
            Debug.Assert(ClosestWordsToSwipePattern(testSwipePattern).First() == testWord);
        }

        public IEnumerable<string> ClosestWordsToSwipePattern(float[,] swipePattern)
        {
            var wordLength = swipePattern.GetLength(1);
            return _wordsByLength[wordLength].OrderByDescending(word => 
                word.Select((letter, letterIndex) => swipePattern[char.ToLower(letter) - 'a', letterIndex]).Sum()
                );
        }
    }
}