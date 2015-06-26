using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Serializable]
    public class NeuralNetwork
    {
        public int numInputs;

        public int numOutputs;
        public Neuron[] outputLayer;

        public int numHiddenLayers;
        public int[] numNeuronsPerHiddenLayer;
        public Neuron[][] hiddenLayers;

        public double LEARNING_RATE { get; set; }
        public double MOMENTUM { get; set; }


        public NeuralNetwork(int _numInputs, int _numOutputs,
            int _numHiddenLayers, int[] _numNeuronsPerHiddenLayer,
            double[][][] _weights, double[][] _biases,
            double _LEARNING_RATE = 0.01, double _MOMENTUM = 0.0)
        {
            this.numInputs = _numInputs;
            this.numOutputs = _numOutputs;

            this.numHiddenLayers = _numHiddenLayers;
            this.numNeuronsPerHiddenLayer = new int[this.numHiddenLayers];
            _numNeuronsPerHiddenLayer.CopyTo(this.numNeuronsPerHiddenLayer, 0);

            this.outputLayer = new Neuron[this.numOutputs];
            for (int k = 0; k < this.numOutputs; k++)
            {
                this.outputLayer[k] = new Neuron(
                    this.numNeuronsPerHiddenLayer[this.numHiddenLayers - 1],
                    _weights[this.numHiddenLayers][k], _biases[this.numHiddenLayers][k]);
            }

            this.hiddenLayers = new Neuron[this.numHiddenLayers][];
            this.hiddenLayers[0] = new Neuron[this.numNeuronsPerHiddenLayer[0]];
            for (int i = 0; i < this.numNeuronsPerHiddenLayer[0]; i++)
            {
                this.hiddenLayers[0][i] = new Neuron(
                    this.numInputs, _weights[0][i], _biases[0][i]);
            }
            for (int j = 1; j < this.numHiddenLayers; j++)
            {
                this.hiddenLayers[j] = new Neuron[this.numNeuronsPerHiddenLayer[j]];
                for (int i = 0; i < this.numNeuronsPerHiddenLayer[j]; i++)
                {
                    this.hiddenLayers[j][i] = new Neuron(
                        this.numNeuronsPerHiddenLayer[j - 1],
                        _weights[j][i], _biases[j][i]);
                }
            }

            this.LEARNING_RATE = _LEARNING_RATE;
            this.MOMENTUM = _MOMENTUM;
        }


        public int numLayers
        {
            get { return this.numHiddenLayers + 2; }
        }

        public Neuron[] getLayer(int i)
        {
            if (i == 0)
            {
                return null; // input layer (not a Neuron[])
            }
            if (i == this.numHiddenLayers + 1)
            {
                return this.outputLayer;
            }
            return this.hiddenLayers[i-1];
        }

        public int getLayerSize(int i)
        {
            if (i == 0)
            {
                return this.numInputs;
            }
            if (i == this.numHiddenLayers + 1)
            {
                return this.numOutputs;
            }
            return this.numNeuronsPerHiddenLayer[i-1];
        }


        public double[] feedForward(double[] inputs)
        {
            double[] inputsForLayer = new double[this.getLayerSize(0)];
            double[] outputsForLayer = null;
            inputs.CopyTo(inputsForLayer, 0);

            for (int layer = 1; layer < this.numLayers; layer++)
            {
                outputsForLayer = new double[getLayerSize(layer)];

                for (int i = 0; i < getLayerSize(layer); i++)
                {
                    outputsForLayer[i] = this.getLayer(layer)[i].calculateOutput(inputsForLayer);
                }

                inputsForLayer = new double[getLayerSize(layer)];
                outputsForLayer.CopyTo(inputsForLayer, 0);
            }

            return outputsForLayer;
        }

        public void backPropagate(double[] target)
        {
            // calculate the error signal for each output node
            for (int k = 0; k < this.getLayerSize(this.numLayers - 1); k++)
            {
                double difference = this.getLayer(this.numLayers - 1)[k].output - target[k];
                this.getLayer(this.numLayers - 1)[k].modifyWeights(difference, LEARNING_RATE, MOMENTUM);
            }

            // calculate the error signal for all hidden layers
            for (int j = this.numLayers - 2; j > 0; j--)
            {
                for (int i = 0; i < this.getLayerSize(j); i++)
                {
                    double weightedSignalError = this.calculateWeightedSignalError(j, i);
                    this.getLayer(j)[i].modifyWeights(weightedSignalError, LEARNING_RATE, MOMENTUM);
                }
            }
        }

        public double[] learn(double[] inputs, double[] target)
        {
            double[] output = this.feedForward(inputs);
            this.backPropagate(target);
            return output;
        }

        public double[] classify(double[] inputs)
        {
            double[] output = this.feedForward(inputs);
            return output;
        }

        public double calculateWeightedSignalError(int currentLayer, int currentIndex)
        {
            int nextLayer = currentLayer + 1;
            double weightedSignalError = 0.0;
            for (int i = 0; i < this.getLayerSize(nextLayer); i++)
            {
                weightedSignalError += this.getLayer(nextLayer)[i].weights[currentIndex] * this.getLayer(nextLayer)[i].errorSignal;
            }
            return weightedSignalError;
        }

        public double squaredError(double[] target)
        {
            double squaredError = 0.0;
            for (int k = 0; k < this.getLayerSize(this.numLayers - 1); k++)
            {
                double difference = this.getLayer(this.numLayers - 1)[k].output - target[k];
                squaredError += difference * difference;
            }
            return squaredError;
        }
    }
}
