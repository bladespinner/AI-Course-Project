using MihailGospodinov.WordPrediction.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public class ThreeGramFileImporter : FileImportLearner
    {
        protected override void FillGram(string[] words)
        {
            int count = int.Parse(words[0]);
            string firstWord = words[1];
            string secondWord = words[2];
            string thirdWord = words[3];
            //var gram = context.ThreeGrams.FirstOrDefault(w => w.FirstWord == firstWord
            //   && w.SecondWord == secondWord
            //   && w.ThirdWord == thirdWord);

            //if (gram == null)
            //{
                var gram = context.ThreeGrams.Add(new ThreeGram()
                {
                    Count = count,
                    FirstWord = firstWord,
                    SecondWord = secondWord,
                    ThirdWord = thirdWord,
                });
            //}

            //gram.Count += count;
        }
    }
}
