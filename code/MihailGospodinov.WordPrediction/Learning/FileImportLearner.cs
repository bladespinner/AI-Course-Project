using MihailGospodinov.WordPrediction.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    public abstract class FileImportLearner : IWordPredictionLearner
    {
        protected int expectedCount = 0;
        protected int offset = 0;
        protected WordPredictionContext context;
        public FileImportLearner()
        {
            this.context = new WordPredictionContext();
        }
        protected abstract void FillGram(string[] words);
        public virtual void Learn(Stream words)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            StreamReader reader = new StreamReader(words);
            int l = 0;
            while (!reader.EndOfStream)
            {
                l++;
                expectedCount++;
                string line = reader.ReadLine();
                FillGram(line.Split(' ', '\t'));
                if(expectedCount == 1000)
                {
                    Console.WriteLine(l);
                    context.SaveChanges();
                    context.Dispose();
                    context = new WordPredictionContext();
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    expectedCount = 0;
                }
            }
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
