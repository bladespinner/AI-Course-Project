using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain;
using GeneticSharp.Infrastructure.Threading;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace Project
{
    public class NetworkController
    {
        private int lengthChromosome;
        private NetworkFitness fitness;
        private Random seed;

        public NetworkController(int lengthChromosome, Random seed)
        {
            this.seed = seed;
            this.lengthChromosome = lengthChromosome;
        }

        public IFitness CreateFitness(Example[] dataset, int numInputs, int numOutputs,
            int numHiddenLayers, int[] numNeuronsPerHiddenLayer,
            double LEARNING_RATE = 0.01, double MOMENTUM = 0.0)
        {
            fitness = new NetworkFitness(ref dataset, 
                numInputs, numOutputs, 
                numHiddenLayers, numNeuronsPerHiddenLayer, 
                LEARNING_RATE, MOMENTUM);
            return fitness;
        }

        public IChromosome CreateChromosome()
        {
            return new NetworkChromosome(lengthChromosome, seed);
        }

        public void Draw(IChromosome bestChromosome)
        {
            // not implemented!
        }
    }
}
