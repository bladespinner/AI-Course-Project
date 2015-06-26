using MihailGospodinov.WordPrediction.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Prediction
{
    public class FourGramPredictor : IWordPredictor
    {
        public List<Tuple<string,double>> Predict(string context)
        {
            if (context == null) context = String.Empty;
            var words = context.Split(',', '.', '!', '?', ':', ';', '"', '\'', ' ', '\t', '\n', '\r')
                .Where(w => !String.IsNullOrWhiteSpace(w)).ToList();

            int n = Math.Max(words.Count() - 3, 0);

            words = words.Skip(n).Take(3)
                .Select(w => w.ToLower())
                .ToList();

            using(var dbContext = new WordPredictionContext())
            {
                if(words.Count == 0)
                {
                    var word = dbContext.FourGrams.Where(a => a.FirstWord != string.Empty && !a.FirstWord.Contains(".")).OrderByDescending(w => w.Count)
                        .GroupBy(g => g.FirstWord)
                        .Select(a => new
                        {
                            p = a.Max(e => e.Probability),
                            w = a.Key
                        })
                        .OrderByDescending(a => a.p)
                        .Take(3)
                        .ToList()
                        .Select(a => new Tuple<string, double>(a.w, a.p)).ToList();
                    return word;
                }

                string word0 = words[0];

                if(words.Count == 1)
                {
                    var fge = dbContext.FourGrams.Where(w => w.FirstWord == word0)
                        .GroupBy(g => g.SecondWord)
                        .Select(a => new{
                            p = a.Max(e => e.Probability),
                            w = a.Key
                        })
                        .OrderByDescending(a => a.p)
                        .Take(3);

                    return fge
                        .ToList()
                        .Select(a => new Tuple<string, double>(a.w, a.p))
                        .ToList();
                }
                
                string word1 = words[1];

                if(words.Count == 2)
                {
                    var fge = dbContext.FourGrams.Where(w => w.FirstWord == word0
                        && w.SecondWord == word1).GroupBy(g => g.ThirdWord)
                        .Select(a => new{
                            p = a.Max(e => e.Probability),
                            w = a.Key
                        })
                        .OrderByDescending(a => a.p)
                        .Take(3);

                    return fge
                        .ToList()
                        .Select(a => new Tuple<string, double>(a.w, a.p))
                        .ToList();
                }

                string word2 = words[2];

                var fourGramEntities = dbContext.FourGrams.Where(w => w.FirstWord == word0
                    && w.SecondWord == word1
                    && w.ThirdWord == word2).OrderByDescending(a => a.Probability);

                return fourGramEntities
                    .ToList()
                    .Select(a => new Tuple<string, double>(a.FourthWord, a.Probability))
                    .ToList();
            }
        }
    }
}
