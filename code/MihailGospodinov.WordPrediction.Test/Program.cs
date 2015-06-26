using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WordPrediction.Initialize();
            LinkedList<string> words = new LinkedList<string>();
            //if you like
            words.AddLast("for");
            words.AddLast("he");
            words.AddLast("was");
            foreach(var word in words)
            {
                Console.Write(word + " ");
            }
            while (true)
            {
                //var line = Console.ReadLine();
                var predictions = WordPrediction.Predict(words.Aggregate((a, b) => a + " " + b));
                predictions.OrderByDescending(a => a.Item2);
                words.RemoveFirst();
                var prediction = predictions.FirstOrDefault();
                if (prediction == null) break;
                var word = predictions.FirstOrDefault().Item1;
                words.AddLast(word);
                Console.Write(" " + word);
                Thread.Sleep(250);
            }
        }
    }
}
