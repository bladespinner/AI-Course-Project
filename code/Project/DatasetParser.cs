using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DatasetParser
    {
        public Example[] parseDatasetFile(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);

            string[] dimensions = lines[0].Split(' ');
            Example.IMG_HEIGHT = Int32.Parse(dimensions[0]);
            Example.IMG_WIDTH = Int32.Parse(dimensions[1]);

            int datasetSize = Int32.Parse(lines[1]);
            Example[] dataset = new Example[datasetSize];

            for (int lineIndex = 2, exampleIndex = 0; exampleIndex < datasetSize; 
                lineIndex += Example.IMG_HEIGHT + 1, exampleIndex++)
            {
                char letter = Char.Parse(lines[lineIndex]);
                int[,] imageBits = new int[Example.IMG_HEIGHT,Example.IMG_WIDTH];

                for (int i = 0; i < Example.IMG_HEIGHT; i++)
                {
                    string[] matrixRow = lines[lineIndex + 1 + i].Split(' ');
                    for (int j = 0; j < Example.IMG_WIDTH; j++)
                    {
                        imageBits[i,j] = Int32.Parse(matrixRow[j]);
                    }
                    
                }
                //Console.WriteLine("Loading Examples");
                //Console.WriteLine(exampleIndex + "/" + datasetSize);
                dataset[exampleIndex] = new Example(imageBits, letter);
            }

            return dataset;
        }
    }
}
