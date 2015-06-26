using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace Project
{
    public class NetworkFitness : IFitness
    {
        public Example[] dataset;

        public int numInputs;
        public int numOutputs;
        public int numHiddenLayers;
        public int[] numNeuronsPerHiddenLayer;
        public double LEARNING_RATE;
        public double MOMENTUM;

        public NetworkFitness(ref Example[] _dataset, int _numInputs, int _numOutputs,
            int _numHiddenLayers, int[] _numNeuronsPerHiddenLayer,
            double _LEARNING_RATE = 0.01, double _MOMENTUM = 0.0) : base()
        {
            this.dataset = _dataset;
            this.numInputs = _numInputs;
            this.numOutputs = _numOutputs;
            this.numHiddenLayers = _numHiddenLayers;
            this.numNeuronsPerHiddenLayer = _numNeuronsPerHiddenLayer;
            this.LEARNING_RATE = _LEARNING_RATE;
            this.MOMENTUM = _MOMENTUM;
        }

        public double Evaluate(IChromosome chromosome)
        {
            NeuralNetwork network = ((NetworkChromosome)chromosome).ToNetwork(
                this.numHiddenLayers, this.numNeuronsPerHiddenLayer, this.numInputs, this.numOutputs);

            double error = CalculateError(ref this.dataset, ref network);

            // fitness function increases as the chromosome gets better
            return 1.0 / (error + 1.0);
        }

        public static double CalculateError(ref Example[] dataset, ref NeuralNetwork network)
        {
            double error = 0.0;
            foreach (Example example in dataset)
            {
                network.classify(example.ToNetworkInput());
                error += network.squaredError(example.ToNetworkOutput());
            }
            return error * 0.5;
        }
    }
}
