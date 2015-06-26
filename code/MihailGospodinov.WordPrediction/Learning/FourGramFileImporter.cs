using MihailGospodinov.WordPrediction.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public class FourGramFileImporter : FileImportLearner
    {
        protected override void FillGram(string[] words)
        {
            int count = int.Parse(words[0]);
            string firstWord = words[1];
            string secondWord = words[2];
            string thirdWord = words[3];
            string forthWord = words[4];

            /*var fourGram = context.FourGrams.FirstOrDefault(w => w.FirstWord == firstWord
                       && w.SecondWord == secondWord
                       && w.ThirdWord == thirdWord
                       && w.FourthWord == forthWord);*/

            var threeGram = context.ThreeGrams.FirstOrDefault(w => w.FirstWord == firstWord
                       && w.SecondWord == secondWord
                       && w.ThirdWord == thirdWord);

            //if (fourGram == null)
            //{
                var fourGram = context.FourGrams.Add(new FourGram()
                {
                    Count = count,
                    FirstWord = firstWord,
                    SecondWord = secondWord,
                    ThirdWord = thirdWord,
                    FourthWord = forthWord
                });
            //}
        }

        public override void Learn(System.IO.Stream words)
        {
            base.Learn(words);
            int fourGramCount = context.FourGrams.Select(a => a.Count).Aggregate((a,b) => a + b);
            int threeGramCount = context.ThreeGrams.Select(a => a.Count).Aggregate((a,b) => a + b);
            int counter = 0;
            foreach(var fourGram in context.FourGrams)
            {
                counter++;
                string firstWord = fourGram.SecondWord;
                string secondWord = fourGram.ThirdWord;
                string thirdWord = fourGram.FourthWord;
                var threeGram = context.ThreeGrams.FirstOrDefault(fg => fg.FirstWord == firstWord &&
                    fg.SecondWord == secondWord &&
                    fg.ThirdWord == thirdWord);

                double fourGramProbability = (fourGram.Count + 1) / ((double)fourGramCount + 1);
                double threeGramProbability = (threeGram.Count + 1) / ((double)threeGramCount + 1);

                fourGram.Probability = fourGramProbability / threeGramProbability;
                
                if(counter == 1000)
                {
                    context.SaveChanges();
                    counter = 0;
                }
            }
        }
    }
}
