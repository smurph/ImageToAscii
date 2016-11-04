using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using ImageToAscii;
using System.IO;
using System.Linq;

namespace Test
{
    [TestClass]
    public class ImageToAsciiTests
    {
        // realy need to change this path reference to be local
        private string picFileName = "D:\\Code\\ImageToAscii\\Test\\Assets\\dog.jpg";
        private string _outputFileName = "D:\\Code\\ImageToAscii\\Test\\Assets\\output.txt";


        [TestMethod]
        public void ConvertToAsciiReturnsMoreThanZeroLines()
        {
            Bitmap pic = new Bitmap(picFileName);

            ImageToAscii.ImageToAscii.ConvertToAscii(picFileName, _outputFileName);
            
            Assert.AreNotEqual(0, File.ReadLines(_outputFileName).ToList().Count);
        }

        [TestCleanup]
        public void RemoveOutputFile()
        {
            File.Delete(_outputFileName);
        }
    }
}
