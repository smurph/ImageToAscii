using System.Text;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace ConvertToAscii
{
    public static class ImageToAscii
    {
        /// <summary>
        ///  After calculating proper scale height of an image based on the provided width
        ///  scale the height again to account for the fact that pixels are square and 
        ///  characters are not. Actual proper value will vary per font which isn't handled
        ///  by this app, but something between 2 and 3 should work fine as a rule of thumb.
        /// </summary>
        private const double _verticalScale = 2.5;


        /// <summary>
        /// Array of characters that must be arranged from "lightest" to "darkest" that 
        /// will be used to represent various grayscale values.
        /// </summary>
        public static string[] CharacterSet = new string[10] { " ", ".", ":", "-", "=", "+", "*", "#", "%", "@" };

        
        /// <summary>
        /// Converts an image to ascii with a line width the same as the image width. 
        /// Writes output to file
        /// </summary>
        /// <param name="inputFileName">Fullly qualified filename to any image file</param>
        /// <param name="outputFileName">Full path to dump the output text to</param>
        public static void ConvertToAscii(string inputFileName, string outputFileName)
        {
            ConvertToAscii(inputFileName, outputFileName, 0);
        }

        /// <summary>
        /// Converts an image to ascii with a defined line width, and writes to file
        /// </summary>
        /// <param name="inputFileName">Fullly qualified filename to any image file</param>
        /// <param name="outputFileName">Full path to dump the output text to</param>
        /// <param name="width">Width to scale the image to.</param>
        public static void ConvertToAscii(string inputFileName, string outputFileName, int width)
        {
            Bitmap b = new Bitmap(inputFileName);
            ConvertToAscii(b, outputFileName, width);
        }

        public static void ConvertToAscii(Bitmap input, string outputFileName, int width)
        {
            if (width <= 0) width = input.Width;
            
            int height = (int)(((double)width / input.Width * input.Height) / _verticalScale);

            Bitmap scaledInput = new Bitmap(input, new Size(width, height));
            
            StringBuilder output = new StringBuilder((scaledInput.Width + 2) * scaledInput.Height);
            
            for (int y = 0; y < scaledInput.Height; y++)
            {
                if (output.Length > 0) output.Append("\r\n");
                for (int x = 0; x < scaledInput.Width; x++)
                {
                    Color c = scaledInput.GetPixel(x, y);

                    // Luminosity method:
                    // 0.21 R + 0.72 G + 0.07 B
                    double grayValue = (c.R * 0.21) + (c.G * 0.72) + (c.B * 0.07);

                    output.Append(getCharacterFromGrayValue(grayValue));
                }
            }
            File.WriteAllText(outputFileName, output.ToString());
        }

        private static string getCharacterFromGrayValue(double value)
        {
            for (int i = CharacterSet.Length; i >= 0; i--)
            {
                double n = 255.0 - ((255.0 / CharacterSet.Length) * i);
                if (value < n)
                {
                    return CharacterSet[i];
                }
            }

            return CharacterSet[CharacterSet.Length-1];
        }
    }
}
