using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class CrossValidation
    {
        private int numTests;
        private List<Example>[] datasetPartitions;
        private CharacterRecognition cr;
        private Random seed;

        public CrossValidation(Example[] _dataset, int _numTests, ref CharacterRecognition _cr, Random seed)
        {
            this.seed = seed;
            this.numTests = _numTests;
            this.datasetPartitions = new List<Example>[this.numTests];
            for (int n = 0; n < this.numTests; n++)
            {
                this.datasetPartitions[n] = new List<Example>();
            }
            for (int i = 0; i < _dataset.Length; i++)
            {
                this.datasetPartitions[i % this.numTests].Add(_dataset[i]);
            }

            this.cr = _cr;
        }

        // return average error per partition
        private double CalculatePrecisionPartition(ref Example[] examples, int i)
        {
            Example[] testSet = this.datasetPartitions[i].ToArray();
            List<Example> trainSet = new List<Example>();
            for (int j = 0; j < this.datasetPartitions.Length; j++)
            {
                if (i == j % this.numTests)
                    continue;

                trainSet.AddRange(this.datasetPartitions[j]);
            }

            Example[] trainSetArray = trainSet.ToArray();
            OCR.Shuffle(trainSetArray,seed);
            this.cr.Initialize(trainSetArray);

            double sumErrors = 0.0;
            int sumErrorsLetters = 0;
            int sumBigger = 0;
            foreach (Example example in testSet)
            {
                double[] output = this.cr.RecognizeCharacter(example.ToNetworkInput());
                //Console.WriteLine("Output is " + Array.IndexOf(output, output.Max()));
                //Console.WriteLine("Correct is " + example.letterToInt() + "\n");
                //Console.WriteLine("Correct is '" + example.letter +"' - int(" + example.letterToInt() + ")\n");
                for (int x = 0; x < output.Length; x++)
                {
                    sumBigger += (output[x] > output[example.letterToInt()] ? 1 : 0);
                    //Console.Write("" + x + ": \t" + output[x] + "\n");
                }
                //Console.Write("" + example.letterToInt() + "-" + Array.IndexOf(output, output.Max()) + "; ");
                sumErrorsLetters += (example.letterToInt() == Array.IndexOf(output, output.Max()) ? 0 : 1);
                sumErrors += example.SquaredError(output);
            }
            Console.WriteLine("Avg error = " + (sumErrors * 0.5 / (double)testSet.Length));
            Console.WriteLine("Avg error (letter index) = " + ((double)sumErrorsLetters / (double)testSet.Length));
            Console.WriteLine("Avg error (bigger index) = " + ((double)sumBigger / (double)testSet.Length));
            Console.WriteLine();
            this.cr.SaveToDirectory(@"C:\Users\Dimiter\Desktop\", "nn" + i + "_");
            return sumErrors * 0.5 / testSet.Length;
        }

        // return average error of all partition errors
        public double CalculatePrecision(ref Example[] set)
        {
            double sum = 0.0;
            for (int i = 0; i < this.numTests; i++)
            {
                Console.WriteLine("Partition No. " + i);
                sum += this.CalculatePrecisionPartition(ref set, i);
            }
            Console.WriteLine("Avg total error = " + (sum / this.numTests));
            return sum / this.numTests;
        }
    }
}
