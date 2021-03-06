using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

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
            Debug.Assert(ClosestWordToSwipePattern(SwipeKeyboard.WordToSwipePositions(testWord).ToArray()) == testWord);

            //=foreach(var sentence in ComputeSentenceVariations("the 0princess is in another 1castle"))
            //    Console.WriteLine(sentence.Item1, sentence.Item2);
        }

        public string ClosestWordToSwipePattern(Vector2[] swipePositions) => ClosestWordsToSwipePattern(swipePositions).First();
        
        private IEnumerable<string> ClosestWordsToSwipePattern(Vector2[] swipePositions)
        {
            return _wordsByLength[swipePositions.Length].OrderBy(word => 
                SwipeKeyboard.SwipePositionToWordDistance(word, swipePositions)
            );
        }
        
        public float SwipePatternDifference(IEnumerable<Vector2> swipePatternPositionsA, IEnumerable<Vector2> swipePatternPositionsB) =>
            swipePatternPositionsA.Zip(swipePatternPositionsB)
                .Select(tuple => (tuple.First - tuple.Second).LengthSquared()).Sum();

        /// <summary>
        /// hyphothetical - real user words ahve floaty swipe patterns
        /// </summary>
        public float WordDifference(string wordA, string wordB) =>
            SwipePatternDifference(SwipeKeyboard.WordToSwipePositions(wordA), SwipeKeyboard.WordToSwipePositions(wordB));
        
        public IEnumerable<(string sentence, float score)> ComputeSentenceVariations(string sentence)
        {
            var words = sentence.Split(' ');
            
            // Sorted variations for every word.
            var sentenceVariations = new List<(string word, float wordDiff)>[words.Length];
            for(int wi =0; wi<words.Length; ++wi)
            {
                sentenceVariations[wi] = new List<(string word, float wordDiff)>();
                // only words that start with a number can actually change
                if (!char.IsDigit(words[wi][0]))
                    sentenceVariations[wi].Add((words[wi], 0.0f));
                else
                {
                    string word = words[wi].Substring(1);
                    sentenceVariations[wi].AddRange(_wordsByLength[word.Length]
                        .Select(dictionaryWord => (word: dictionaryWord, wordDiff: WordDifference(dictionaryWord, word)))
                        .OrderBy(t => t.wordDiff));
                }
            }
            
            // look at all possible ways to advance from here
            var visitedSentences = new HashSet<string>();
            var indices = new List<(float score, int[] indices, string sentence)>(); // priority queue anyone?
            indices.Add((0.0f, new int[sentenceVariations.Length], 
                string.Join(' ', sentenceVariations.Select(v => v.First().Item1))));

            while (indices.Count > 0)
            {
                // yield best sentence
                indices.Sort((a, b) => b.score.CompareTo(a.score));
                var popped = indices.Last();
                indices.RemoveAt(indices.Count - 1);
                yield return (popped.sentence, popped.score);
                
                // add more sentences
                for (int i = 0; i < sentenceVariations.Length; ++i)
                {
                    if (popped.indices[i] + 1 == sentenceVariations[i].Count)
                        continue;
                    
                    var newIndices = (int[])popped.indices.Clone();
                    newIndices[i] += 1;
                    
                    var selectedWords =
                        sentenceVariations.Zip(newIndices).Select(x => x.First[x.Second]).ToList();

                    var newSentence = string.Join(' ', selectedWords.Select(x => x.word));

                    if (visitedSentences.Add(newSentence))
                    {
                        indices.Add(
                            (
                                score: selectedWords.Sum(tuple => tuple.wordDiff),
                                indices: newIndices,
                                sentence: newSentence
                            ));
                    }
                }
            }
        }
    }
}