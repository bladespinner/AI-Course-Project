using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageToNetworkInput
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageToNetworkInput imgToNet = new ImageToNetworkInput();
            int[][] m = imgToNet.ParseImage(Image.FromFile(@"C:\Users\Dimiter\Desktop\EnglishCapitalLettersImages\Sample011\img011-001.png"));

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(m[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
