using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Project
{
    class ImageToNetworkInput
    {
        public int MATRIX_WIDTH = 10;
        public int MATRIX_HEIGHT = 10;

        public ImageToNetworkInput(int _width = 10, int _height = 10)
        {
            this.MATRIX_WIDTH = _width;
            this.MATRIX_HEIGHT = _height;
        }

        private Rectangle TrimLetter(Bitmap pic)
        {
            Color threshold = Color.FromArgb(255, 255, 255, 255);
            bool shouldBreak = false;
            int xMin = 0;
            for (int x = 0; x < pic.Width && !shouldBreak; x++)
            {
                for (int y = 0; y < pic.Height && !shouldBreak; y++)
                {
                    if (pic.GetPixel(x, y) != threshold)
                    {
                        xMin = x;
                        shouldBreak = true;
                    }
                }
            }

            shouldBreak = false;
            int yMin = 0;
            for (int y = 0; y < pic.Height && !shouldBreak; y++)
            {
                for (int x = 0; x < pic.Width && !shouldBreak; x++)
                {
                    if (pic.GetPixel(x, y) != threshold)
                    {
                        yMin = y;
                        shouldBreak = true;
                    }
                }
            }

            shouldBreak = false;
            int xMax = 0;
            for (int x = pic.Width - 1; x >= 0 && !shouldBreak; x--)
            {
                for (int y = 0; y < pic.Height && !shouldBreak; y++)
                {
                    if (pic.GetPixel(x, y) != threshold)
                    {
                        xMax = x;
                        shouldBreak = true;
                    }
                }
            }

            shouldBreak = false;
            int yMax = 0;
            for (int y = pic.Height - 1; y >= 0 && !shouldBreak; y--)
            {
                for (int x = 0; x < pic.Width && !shouldBreak; x++)
                {
                    if (pic.GetPixel(x, y) != threshold)
                    {
                        yMax = y;
                        shouldBreak = true;
                    }
                }
            }

            // return minimum square that includes the entire letter
            int minSideLength = Math.Max(xMax - xMin, yMax - yMin);
            return new Rectangle(xMin, yMin,
                Math.Min(minSideLength, pic.Width - 1 - xMin), Math.Min(minSideLength, pic.Height - 1 - yMin)
            );
        }

        private int[][] DataToString(ref Bitmap bitmapPic)
        {
            int[][] resultMatrix = new int[this.MATRIX_HEIGHT][];

            for (int y = 0; y < bitmapPic.Height; y++)
            {
                resultMatrix[y] = new int[this.MATRIX_WIDTH];
                for (int x = 0; x < bitmapPic.Width; x++)
                {
                    double grayscaleValue = 0.21 * bitmapPic.GetPixel(x, y).R
                        + 0.72 * bitmapPic.GetPixel(x, y).G
                        + 0.07 * bitmapPic.GetPixel(x, y).B;
                    resultMatrix[y][x] = (grayscaleValue < 127 ? 1 : 0);
                }
            }

            return resultMatrix;
        }

        public int[][] ParseImage(Image pic)
        {
            Bitmap bitmapPic = new Bitmap(this.MATRIX_WIDTH, this.MATRIX_HEIGHT);

            // cut smallest square around the letter
            Rectangle letterSquare = this.TrimLetter(new Bitmap(pic));

            // resize it
            Graphics graphics = Graphics.FromImage(bitmapPic);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(pic,
                new Rectangle(0, 0, this.MATRIX_WIDTH, this.MATRIX_HEIGHT),
                letterSquare,
                GraphicsUnit.Pixel);

            // convert data to int[][]
            int[][] resultMatrix = this.DataToString(ref bitmapPic);

            graphics.Dispose();
            bitmapPic.Dispose();
            pic.Dispose();

            return resultMatrix;
        }
    }
}
