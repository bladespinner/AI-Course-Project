using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Project
{
    public class OCR
    {
        public string EXAMPLES_FILENAME;
        public int NUM_NETWORKS;

        public static int NETWORK_NUM_INPUTS = 100;
        public static int NETWORK_NUM_OUTPUTS = 26;
        public static int NETWORK_NUM_HIDDEN_LAYERS = 1;
        public static int[] NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER = new int[] { 70 };

        public CharacterRecognition characterRecognizer;

        public OCR(string _EXAMPLES_FILENAME, int _NUM_NETWORKS = 3)
        {
            this.EXAMPLES_FILENAME = _EXAMPLES_FILENAME;
            this.NUM_NETWORKS = _NUM_NETWORKS;
        }

        public void Generate(Random seed)
        {
            DatasetParser datasetParser = new DatasetParser();
            Example[] examples = datasetParser.parseDatasetFile(EXAMPLES_FILENAME);
            Shuffle(examples, seed);

            this.characterRecognizer = new CharacterRecognition(NUM_NETWORKS, 
                NETWORK_NUM_INPUTS, NETWORK_NUM_OUTPUTS,
                NETWORK_NUM_HIDDEN_LAYERS, NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER, seed);
            this.characterRecognizer.Initialize(examples);
        }

        public void GenerateFromFiles(Random seed, string[] filenames)
        {
            //DatasetParser datasetParser = new DatasetParser();
            //Example[] examples = datasetParser.parseDatasetFile(EXAMPLES_FILENAME);
            //Shuffle(examples, seed);

            this.characterRecognizer = new CharacterRecognition(NUM_NETWORKS,
                NETWORK_NUM_INPUTS, NETWORK_NUM_OUTPUTS,
                NETWORK_NUM_HIDDEN_LAYERS, NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER, seed);
            this.characterRecognizer.InitializeFromFiles(filenames);
        }

        public double[] RecognizeImage(Image img, int width = 10, int height = 10)
        {
            return this.Recognize(
                ConvertImageToNetworkInput(img, width, height)
            );
        }

        public double[] Recognize(double[] input)
        {
            return this.characterRecognizer.RecognizeCharacter(input);
        }

        public double[] ConvertImageToNetworkInput(Image img, int width = 10, int height = 10)
        {
            ImageToNetworkInput imgToNet = new ImageToNetworkInput(width, height);
            int[][] imageBitsArray = imgToNet.ParseImage(img);

            double[] networkInput = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    networkInput[j + i * height] = imageBitsArray[i][j];
                }
            }

            return networkInput;
        }

        public double CrossValidate(Random seed)
        {
            //Console.WriteLine("Start time: " + DateTime.Now.ToString());
            DatasetParser datasetParser = new DatasetParser();
            Example[] examples = datasetParser.parseDatasetFile(this.EXAMPLES_FILENAME);

            CharacterRecognition cr = new CharacterRecognition(this.NUM_NETWORKS,
                NETWORK_NUM_INPUTS, NETWORK_NUM_OUTPUTS,
                NETWORK_NUM_HIDDEN_LAYERS, NETWORK_NUM_NEURONS_PER_HIDDEN_LAYER
                ,seed);

            CrossValidation cv = new CrossValidation(examples, 5, ref cr,seed);
            double precision = cv.CalculatePrecision(ref examples);

            //Console.WriteLine(1.0 - precision);
            //Console.WriteLine("End time: " + DateTime.Now.ToString());
            return 1.0 - precision;
        }

        // Fisher-Yates array shuffle;
        // used to shuffle the dataset before training the networks
        public static void Shuffle<T>(T[] array,Random seed)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(seed.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}
