using MihailGospodinov.WordPrediction.Domain;
using MihailGospodinov.WordPrediction.Learning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var tg = File.OpenRead("three_gram_data.txt"))
            {
                using(var importer = new ThreeGramFileImporter())
                {
                    importer.Learn(tg);
                }
            }

            using (var fg = File.OpenRead("four_gram_data.txt"))
            {
                using (var importer = new FourGramFileImporter())
                {
                    importer.Learn(fg);
                }
            }
        }
    }
}
