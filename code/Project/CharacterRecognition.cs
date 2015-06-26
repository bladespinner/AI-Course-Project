using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Project
{
    public class CharacterRecognition
    {
        public NeuralNetwork[] networks;
        public int numNetworks;

        public int networkNumInputs;
        public int networkNumOutputs;
        public int networkNumHiddenLayers;
        public int[] networkNumNeuronsPerHiddenLayer;
        private Random seed;

        public CharacterRecognition(int _numNetworks, 
            int _networkNumInputs, int _networkNumOutputs,
            int _networkNumHiddenLayers, int[] _networkNumNeuronsPerHiddenLayer,
            Random seed)
        {
            this.seed = seed;
            this.numNetworks = _numNetworks;
            this.networkNumInputs = _networkNumInputs;
            this.networkNumOutputs = _networkNumOutputs;
            this.networkNumHiddenLayers = _networkNumHiddenLayers;
            this.networkNumNeuronsPerHiddenLayer = _networkNumNeuronsPerHiddenLayer;
        }

        public void Initialize(Example[] examples)
        {
            this.networks = new NeuralNetwork[this.numNetworks];
            for (int i = 0; i < this.numNetworks; i++)
            {
                NetworkEvolution evolution = new NetworkEvolution(ref examples,
                    this.networkNumInputs, this.networkNumOutputs,
                    this.networkNumHiddenLayers, this.networkNumNeuronsPerHiddenLayer, seed);
                networks[i] = evolution.EvolveNetwork();

                /*foreach (Example example in examples)
                {
                    networks[i].learn(example.ToNetworkInput(), example.ToNetworkOutput());
                }*/
Console.WriteLine("Network No. " + i);
Console.WriteLine(DateTime.Now.ToString());
            }
        }

        public double[] RecognizeCharacter(double[] inputMatrix)
        {
            double[] allOutputs = new double[this.networkNumOutputs];
            foreach (NeuralNetwork network in networks)
            {
                double[] output = network.classify(inputMatrix);
                allOutputs = allOutputs.Zip(output, (x, y) => x + y).ToArray();
//foreach(double d in output){Console.Write(d+", ");}Console.WriteLine();
            }

//Console.WriteLine("avg vector:");
//foreach (double d in allOutputs.Select(x => x / networks.Length).ToArray()) { Console.Write(d + ", "); } Console.WriteLine("\n");
            return allOutputs.Select(x => x / networks.Length).ToArray();
        }

        public void InitializeFromFiles(string[] filenames)
        {
            this.networks = new NeuralNetwork[this.numNetworks];
            int i = 0;
            foreach (string filename in filenames)
            {
                FileStream stream = File.OpenRead(filename);
                var formatter = new BinaryFormatter();
                networks[i++] = (NeuralNetwork)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        public void SaveToDirectory(string dirname, string filenamePrefix)
        {
            int i = 0;
            foreach (NeuralNetwork network in this.networks)
            {
                FileStream stream = File.Create(dirname + filenamePrefix + i);
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, network);
                stream.Close();
                i++;
            }
        }
    }
}
