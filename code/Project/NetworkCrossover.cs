using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using System.ComponentModel;
using GeneticSharp.Domain.Crossovers;

namespace Project
{
    public class NetworkCrossover : CrossoverBase
    {
        int[] indexRanges;
        Random seed;

        public double MixProbability { get; set; }

        public NetworkCrossover(int numInputs, int numOutputs,
            int numHiddenLayers, int[] numNeuronsPerHiddenLayer,
            Random seed, double mixProbability = 0.5)
            : base(2, 2)
        {
            this.seed = seed;
            this.MixProbability = mixProbability;

            int[] layers = new int[numHiddenLayers + 2];
            layers[0] = numInputs;
            layers[numHiddenLayers + 1] = numOutputs;
            int rangeArrayLength = numOutputs;
            for (int i = 0; i < numHiddenLayers; i++)
            {
                layers[i + 1] = numNeuronsPerHiddenLayer[i];
                rangeArrayLength += numNeuronsPerHiddenLayer[i];
            }

            indexRanges = new int[rangeArrayLength + 1];
            indexRanges[0] = 0;
            for (int layerIndex = 1, rangeIndex = 0; layerIndex < layers.Length; layerIndex++)
            {
                for (int neuronIndex = 0; neuronIndex < layers[layerIndex]; neuronIndex++)
                {
                    indexRanges[rangeIndex + 1] = indexRanges[rangeIndex] + layers[layerIndex - 1] + 1;
                    rangeIndex++;
                }
            }
        }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];
            var firstChild = firstParent.CreateNew();
            var secondChild = secondParent.CreateNew();

            for (int r = 0; r < indexRanges.Length - 1; r++)
            {
                if (seed.NextDouble() < MixProbability)
                {
                    var parentSwap = firstParent;
                    firstParent = secondParent;
                    secondParent = parentSwap;
                }

                int beginRange = indexRanges[r], endRange = indexRanges[r + 1] - 1;
                for (int i = beginRange; i < endRange; i++)
                {
                    firstChild.ReplaceGene(i, firstParent.GetGene(i));
                    secondChild.ReplaceGene(i, secondParent.GetGene(i));
                }
            }

            return new List<IChromosome> { firstChild, secondChild };
        }
    }
}