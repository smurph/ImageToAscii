using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ImageToAscii
{
    public class ImageToAscii
    {

        private static string[] _characters = new string[6] { " ", ".", "+", "o", "#", "W" };
        private static double[] _characterGrayValueThresholds = new double[6] { 220.0, 185.0, 150.0, 115.0, 80.0, 45.0 };
        
        public static void ConvertToAscii(string pictureFileName, string outputFileName)
        {
            Bitmap b = new Bitmap(pictureFileName);

            StringBuilder output = new StringBuilder((b.Width + 1) * b.Height);

            for (int y = 0; y < b.Height; y++)
            {
                if (output.Length > 0) output.Append("\n");
                for (int x = 0; x < b.Width; x++)
                {
                    Color c =  b.GetPixel(x, y);

                    // Luminosity method:
                    // 0.21 R + 0.72 G + 0.07 B
                    double grayValue = (c.R * 0.21) + (c.G * 0.72) + (c.B * 0.07);

                    output.Append(getCharacterFromGrayValue(grayValue));
                }
            }


            File.WriteAllLines(outputFileName, output.ToString().Split('\n'));
        }

        private static string getCharacterFromGrayValue(double value)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                if (value >= _characterGrayValueThresholds[i])
                {
                    return _characters[i];
                }
            }

            // really dark. 
            return "@";
        }

        private byte[] toByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();

            img.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
