using MihailGospodinov.WordPrediction.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MihailGospodinov.WordPrediction.Learning
{
    public class EntityLearner : IWordPredictionLearner
    {
        private WordPredictionContext context;
        public EntityLearner()
        {
            this.context = new WordPredictionContext();
        }
        public void HandleFourGram(List<string> words, ThreeGram threeGram)
        {
            string firstWord = words[0];
            string secondWord = words[1];
            string thirdWord = words[2];
            string forthWord = words[3];

            var gram = context.FourGrams.FirstOrDefault(w => w.FirstWord == firstWord
                       && w.SecondWord == secondWord
                       && w.ThirdWord == thirdWord
                       && w.FourthWord == forthWord);

            if (gram == null)
            {
                gram = context.FourGrams.Add(new FourGram()
                {
                    Count = 0,
                    FirstWord = firstWord,
                    SecondWord = secondWord,
                    ThirdWord = thirdWord,
                    FourthWord = forthWord,
                });
            }
            gram.Count++;
            gram.Probability = gram.Count / threeGram.Count;
        }
        public ThreeGram HandleThreeGram(List<string> words)
        {
            string firstWord = words[0];
            string secondWord = words[1];
            string thirdWord = words[2];
            var gram = context.ThreeGrams.FirstOrDefault(w => w.FirstWord == firstWord
               && w.SecondWord == secondWord
               && w.ThirdWord == thirdWord);

            if (gram == null)
            {
                gram = context.ThreeGrams.Add(new ThreeGram()
                {
                    Count = 0,
                    FirstWord = firstWord,
                    SecondWord = secondWord,
                    ThirdWord = thirdWord,
                });
            }

            gram.Count++;

            return gram;
        }
        public void HandleTwoGram(List<string> words)
        {
            string firstWord = words[0];
            string secondWord = words[1];
            var gram = context.DiGrams.FirstOrDefault(w => w.FirstWord == firstWord
                && w.SecondWord == secondWord);

            if (gram == null)
            {
                gram = context.DiGrams.Add(new DiGram()
                {
                    Count = 0,
                    FirstWord = firstWord,
                    SecondWord = secondWord
                });
            }

            gram.Count++;
        }
        public void HandleWord(string word)
        {
            var wordEntity = context.Words.FirstOrDefault(w => w.Content == word);
            if (wordEntity == null)
            {
                wordEntity = context.Words.Add(new Word()
                {
                    Count = 0,
                    Content = word
                });
            }
            wordEntity.Count++;
        }
        public void Learn(Stream words)
        {
            var wordStream = new WordStream(words);
            LinkedList<string> wordHistory = new LinkedList<string>();
            for (int i = 0; i < 4; i++)
            {
                wordHistory.AddLast(String.Empty);
            }
            foreach (var word in wordStream)
            {
                using (var transaction = new CommittableTransaction())
                {
                    wordHistory.RemoveFirst();
                    wordHistory.AddLast(word);

                    HandleWord(word);
                    HandleTwoGram(wordHistory.AsEnumerable().Skip(2).Take(2).ToList());
                    var gram = HandleThreeGram(wordHistory.AsEnumerable().Skip(1).Take(3).ToList());
                    HandleFourGram(wordHistory.AsEnumerable().ToList(), gram);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void Dispose()
        {
            context.SaveChanges();
            context.Dispose();
        }
    }
}
