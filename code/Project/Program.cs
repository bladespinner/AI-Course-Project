using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Project
{
    public class Program
    {
        public static string EXAMPLES_FILENAME = @"C:\Users\Dimiter\Desktop\datasetChars74k.txt";
        public static int NUM_NETWORKS = 1;

        public static int NETWORK_NUM_INPUTS = 100;
        public static int NETWORK_NUM_OUTPUTS = 26;
        public static int NETWORK_NUM_HIDDEN_LAYERS = 1;
        public static int[] NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER = new int[] { 70 };

        // Fisher-Yates array shuffle;
        // used to shuffle the dataset before training the networks
        public static void Shuffle<T>(T[] array, Random rand)
        {
            //Random rand = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(rand.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public static void CrossValidate(Random seed)
        {
            DatasetParser datasetParser;
            Example[] examples;
            CharacterRecognition cr;
            CrossValidation cv;


            NetworkEvolution.GENERATIONS_NUMBER = 20000;
            NetworkEvolution.STAGNANT_NUMBER = 50;
            Console.WriteLine("-------stag50||its20000_mut0,003_pop20_30_L1_70-------------------------");
            Console.WriteLine("Start time: " + DateTime.Now.ToString());
            datasetParser = new DatasetParser();
            examples = datasetParser.parseDatasetFile(EXAMPLES_FILENAME);
            cr = new CharacterRecognition(NUM_NETWORKS,
                NETWORK_NUM_INPUTS, NETWORK_NUM_OUTPUTS,
                1, new int[] { 70 }, seed);
            cv = new CrossValidation(examples, 5, ref cr, seed);
            Console.WriteLine(cv.CalculatePrecision(ref examples));
            Console.WriteLine("End time: " + DateTime.Now.ToString());


        }
        
        static void Main(string[] args)
        {
            Random seed = new Random();
            CrossValidate(seed);
            return;

Console.WriteLine("Start time: " + DateTime.Now.ToString());

            DatasetParser datasetParser = new DatasetParser();
            Example[] examples = datasetParser.parseDatasetFile(EXAMPLES_FILENAME);
            Shuffle(examples, seed);

            CharacterRecognition cr = new CharacterRecognition(NUM_NETWORKS, 
                NETWORK_NUM_INPUTS, NETWORK_NUM_OUTPUTS,
                NETWORK_NUM_HIDDEN_LAYERS, NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER,seed);
            cr.Initialize(examples);

            /*while (true)
            {
                double[] input = null;
                double[] output = cr.RecognizeCharacter(input);

foreach(double d in output){Console.Write(d+", ");}Console.WriteLine();
            }*/

            /*Console.WriteLine("Guessing: ");
            cr.RecognizeCharacter(new double[] { 
                0, 1, 0, 0,
                0, 1, 0, 0,
                0, 1, 1, 0,
                0, 0, 1, 0
            });

            cr.RecognizeCharacter(new double[] { 
                0, 1, 1, 0,
                1, 1, 0, 1,
                0, 0, 1, 1,
                0, 0, 1, 0
            });*/

            Console.WriteLine("Guessing: ");
            cr.RecognizeCharacter(new double[] { 
                0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                0, 0, 0, 1, 1, 1, 1, 1, 1, 1,
                0, 0, 1, 1, 0, 0, 0, 0, 0, 1,
                0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 1, 1, 1, 0, 0, 0, 0, 0, 1,
                0, 0, 1, 1, 1, 0, 0, 0, 1, 1,
                0, 0, 0, 0, 1, 1, 1, 1, 1, 0,
                0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
            });
        }
    }
}
