using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;

namespace Project
{
    public class NetworkChromosome : ChromosomeBase
    {
        public static double MIN_WEIGHT = -1.0;
        public static double MAX_WEIGHT = 1.0;
        private Random r;

        public NetworkChromosome(int length, Random seed)
            : base(length)
        {
            r = seed;
            for (int i = 0; i < length; i++)
            {
                this.ReplaceGene(i,
                    new Gene(MIN_WEIGHT + r.NextDouble() * (MAX_WEIGHT - MIN_WEIGHT)));
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(MIN_WEIGHT + r.NextDouble() * (MAX_WEIGHT - MIN_WEIGHT));
        }

        public override IChromosome CreateNew()
        {
            return new NetworkChromosome(this.Length,r);
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as NetworkChromosome;
            return clone;
        }

        public NeuralNetwork ToNetwork(int numHiddenLayers, int[] numNeuronsPerHiddenLayer,
            int numInputs, int numOutputs)
        {
            double[][][] weights;
            double[][] biases;
            makeArrays(out weights, out biases,
                numHiddenLayers, numNeuronsPerHiddenLayer,
                numInputs, numOutputs);
            return new NeuralNetwork(numInputs, numOutputs,
                numHiddenLayers, numNeuronsPerHiddenLayer,
                weights, biases);
        }

        public void makeArrays(out double[][][] weights, out double[][] biases,
            int numHiddenLayers, int[] numNeuronsPerHiddenLayer,
            int numInputs, int numOutputs)
        {
            double[] genes = new double[this.GetGenes().Length];
            genes = this.GetGenes().Select(g => (double)g.Value).ToArray();
            int arrayIndex = 0;

            weights = new double[numHiddenLayers + 1][][];
            biases = new double[numHiddenLayers + 1][];

            // first hidden layer
            weights[0] = new double[numNeuronsPerHiddenLayer[0]][];
            biases[0] = new double[numNeuronsPerHiddenLayer[0]];
            for (int neuronIndex = 0; neuronIndex < numNeuronsPerHiddenLayer[0]; neuronIndex++)
            {
                weights[0][neuronIndex] = new double[numInputs];
                Array.Copy(genes, arrayIndex, weights[0][neuronIndex], 0, numInputs);
                arrayIndex += numInputs;

                biases[0][neuronIndex] = genes[arrayIndex];
                arrayIndex += 1;
            }

            // other hidden layers
            int layerIndex = 1;
            for (; layerIndex < numHiddenLayers; layerIndex++)
            {
                weights[layerIndex] = new double[numNeuronsPerHiddenLayer[layerIndex]][];
                biases[layerIndex] = new double[numNeuronsPerHiddenLayer[layerIndex]];
                for (int neuronIndex = 0; neuronIndex < numNeuronsPerHiddenLayer[layerIndex]; neuronIndex++)
                {
                    int numInputsPerLayer = numNeuronsPerHiddenLayer[layerIndex - 1];
                    weights[layerIndex][neuronIndex] = new double[numInputsPerLayer];
                    Array.Copy(genes, arrayIndex, weights[layerIndex][neuronIndex], 0, numInputsPerLayer);
                    arrayIndex += numInputsPerLayer;

                    biases[layerIndex][neuronIndex] = genes[arrayIndex];
                    arrayIndex += 1;
                }
            }

            // output layer
            weights[layerIndex] = new double[numOutputs][];
            biases[layerIndex] = new double[numOutputs];
            for (int neuronIndex = 0; neuronIndex < numOutputs; neuronIndex++)
            {
                int numInputsPerLayer = numNeuronsPerHiddenLayer[layerIndex - 1];
                weights[layerIndex][neuronIndex] = new double[numInputsPerLayer];
                Array.Copy(genes, arrayIndex, weights[layerIndex][neuronIndex], 0, numInputsPerLayer);
                arrayIndex += numInputsPerLayer;

                biases[layerIndex][neuronIndex] = genes[arrayIndex];
                arrayIndex += 1;
            }
        }
    }
}
