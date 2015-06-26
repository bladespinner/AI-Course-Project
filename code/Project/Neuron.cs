using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Serializable]
    public class Neuron
    {
        public int numInputs;
        public double[] inputs;

        public double[] weights;
        public double bias;

        public double output;

        public double errorSignal;
        public double[] deltaWeights;
        public double deltaBias;


        public Neuron(int _numInputs, double[] _weights, double _bias)
        {
            this.numInputs = _numInputs;
            this.inputs = new double[this.numInputs];

            this.weights = new double[this.numInputs];
            _weights.CopyTo(this.weights, 0);

            this.deltaWeights = new double[this.numInputs];
            // gets initialized with 0s

            this.bias = _bias;
            this.deltaBias = 0.0;
        }


        public double calculateOutput(double[] inputs)
        {
            //inputs.CopyTo(this.inputs, 0);
            double actionPotential = sumActionPotential(inputs);
            calculateOutput(actionPotential);
            return this.output;
        }

        public void modifyWeights(double term, double LEARNING_RATE, double MOMENTUM)
        {
            calculateErrorSignal(term);
            adjustWeights(LEARNING_RATE, MOMENTUM);
            adjustBias(LEARNING_RATE, MOMENTUM);
        }


        private double sumActionPotential(double[] inputs)
        {
            double actionPotential = 0.0;
            for (int i = 0; i < this.numInputs; i++)
            {
                actionPotential += inputs[i] * this.weights[i];
            }
            actionPotential += this.bias;
            return actionPotential;
        }

        private void calculateOutput(double actionPotential)
        {
            // output is the value of the activation function (the sigmoid)
            this.output = 1.0 / (1.0 + Math.Exp((-1) * actionPotential));
        }

        private void calculateErrorSignal(double term)
        {
            // term is the error (target - output) for the output layer
            // or the sum term (weighted error signals of all next nodes) for the hidden layers 
            this.errorSignal = term * this.output * (1 - this.output);
        }

        private void adjustWeights(double LEARNING_RATE, double MOMENTUM)
        {
            for (int j = 0; j < this.numInputs; j++)
            {
                this.deltaWeights[j] =
                    LEARNING_RATE * this.errorSignal * this.inputs[j]
                    + this.deltaWeights[j] * MOMENTUM;

                this.weights[j] += this.deltaWeights[j];
            }
        }

        private void adjustBias(double LEARNING_RATE, double MOMENTUM)
        {
            this.deltaBias =
                LEARNING_RATE * this.errorSignal
                + this.deltaBias * MOMENTUM;

            this.bias += this.deltaBias;
        }
    }
}
