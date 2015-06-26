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
    public class NetworkEvolution
    {
        public static int POPULATION_MIN_SIZE = 30;
        public static int POPULATION_MAX_SIZE = 40;
        public static float MUTATION_PROBABILITY = 0.003f;
        public static int STAGNANT_NUMBER = 50;
        public static int GENERATIONS_NUMBER = 2000;

        public int CHROMOSOME_LENGTH;

        public int numInputs;
        public int numOutputs;
        public int numHiddenLayers;
        public int[] numNeuronsPerHiddenLayer;
        public double LEARNING_RATE;
        public double MOMENTUM;

        public Example[] dataset;
        private Random seed;

        public NetworkEvolution(ref Example[] _dataset, int _numInputs, int _numOutputs,
            int _numHiddenLayers, int[] _numNeuronsPerHiddenLayer, Random seed,
            double _LEARNING_RATE = 0.01, double _MOMENTUM = 0.0)
        {
            this.seed = seed;
            this.dataset = _dataset;
            this.numInputs = _numInputs;
            this.numOutputs = _numOutputs;
            this.numHiddenLayers = _numHiddenLayers;
            this.numNeuronsPerHiddenLayer = _numNeuronsPerHiddenLayer;
            this.LEARNING_RATE = _LEARNING_RATE;
            this.MOMENTUM = _MOMENTUM;

            // set chromosome length
            int[] layersSizes = new int[_numHiddenLayers + 2];
            layersSizes[0] = _numInputs;
            Array.Copy(_numNeuronsPerHiddenLayer, 0, layersSizes, 1, _numHiddenLayers);
            layersSizes[layersSizes.Length - 1] = _numOutputs;

            this.CHROMOSOME_LENGTH = 0;
            for (int i = 1; i < layersSizes.Length; i++)
            {
                this.CHROMOSOME_LENGTH += (layersSizes[i] * (layersSizes[i-1] + 1));
            }
        }

        public NeuralNetwork EvolveNetwork()
        {
            NetworkController sampleController = new NetworkController(this.CHROMOSOME_LENGTH, seed);

            var selection = new EliteSelection();

            var crossover = new UniformCrossover();
            //var crossover = new NetworkCrossover(this.numInputs, this.numOutputs, this.numHiddenLayers, this.numNeuronsPerHiddenLayer, this.seed);
            
            var mutation = new UniformMutation(true);

            var fitness = sampleController.CreateFitness(this.dataset, this.numInputs, this.numOutputs, 
                this.numHiddenLayers, this.numNeuronsPerHiddenLayer, 
                this.LEARNING_RATE, this.MOMENTUM);

            var population = new Population(POPULATION_MIN_SIZE, POPULATION_MAX_SIZE, sampleController.CreateChromosome());
            population.GenerationStrategy = new PerformanceGenerationStrategy();


            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.MutationProbability = MUTATION_PROBABILITY;
            ga.Termination = new OrTermination(new ITermination[] {
                new GenerationNumberTermination(GENERATIONS_NUMBER),
                new FitnessStagnationTermination(STAGNANT_NUMBER)
            });

            ga.Start();

            NetworkChromosome best = (NetworkChromosome)ga.Population.BestChromosome;
Console.WriteLine("f_max = " + fitness.Evaluate(best));
Console.WriteLine("(after " + ga.GenerationsNumber + " iterations)");
            return best.ToNetwork(this.numHiddenLayers, this.numNeuronsPerHiddenLayer, this.numInputs, this.numOutputs);
        }
    }
}
