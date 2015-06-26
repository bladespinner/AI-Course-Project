using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Example
    {
        public static int IMG_HEIGHT;
        public static int IMG_WIDTH;

        public int[,] imageBits;
        public char letter;

        public Example(int[,] _imageBits, char _letter)
        {
            this.imageBits = new int[IMG_HEIGHT, IMG_WIDTH];
            for (int i = 0; i < IMG_HEIGHT; i++)
            {
                for (int j = 0; j < IMG_WIDTH; j++)
                {
                    this.imageBits[i,j] = _imageBits[i,j];
                }
            }
            this.letter = _letter;
        }

        public int letterToInt()
        {
            return (int)(this.letter - 'A');
        }

        public double[] ToNetworkInput()
        {
            double[] input = new double[IMG_HEIGHT * IMG_WIDTH];
            int i = 0;
            foreach (int bit in this.imageBits)
            {
                input[i++] = (double)bit;
            }
            return input;
        }

        public double[] ToNetworkOutput()
        {
            double[] output = new double[((int)'Z' - 'A') + 1];
            output[this.letterToInt()] = 1.0;
            return output;
        }

        public double SquaredError(double[] networkOutput)
        {
            double[] target = this.ToNetworkOutput();
            double squaredError = 0.0;
            for (int k = 0; k < target.Length; k++)
            {
                double difference = target[k] - networkOutput[k];
                squaredError += difference * difference;
            }
            return squaredError;
        }
    }
}
