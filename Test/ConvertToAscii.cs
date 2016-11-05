using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using ConvertToAscii;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class ConvertToAscii
    {
        private string _outputFileName = "output.txt";
        private List<string> _inputFileNames;

        private Dictionary<string, Bitmap> _inputs;

        [TestInitialize]
        public void Init()
        {
            //todo: do a better job at determining the local directory
            string testRoot = "D:\\Code\\ImageToAscii\\Test\\Assets\\";

            _inputs = new Dictionary<string, Bitmap>();
            _inputFileNames = new List<string>();
            _outputFileName = testRoot + _outputFileName;

            foreach (var file in Directory.GetFiles(testRoot, "*.jpg")) 
            {
                string[] fileNameParts = file.Split('\\');

                _inputs.Add(fileNameParts[fileNameParts.Length - 1], new Bitmap(file));
                _inputFileNames.Add(file);
            }
            
            RemoveOutputFile();
        }

        [TestMethod]
        public void TwoValidFileNames_WritesMoreThanZeroLines()
        {
            ImageToAscii.ConvertToAscii(_inputFileNames[0], _outputFileName);
            
            Assert.AreNotEqual(0, File.ReadLines(_outputFileName).ToList().Count);
        }

        [TestMethod]
        public void TwoValidFileNamesAndValidWidth_WritesProperScaledWidth()
        {
            int width = 100;
            ImageToAscii.ConvertToAscii(_inputFileNames[0], _outputFileName, width);

            // ReadLines eats the line endings, so no need to account for their width.
            Assert.AreEqual(width, File.ReadLines(_outputFileName).ToList()[0].Length);
        }

        [TestMethod]
        public void BitmapAndFileNameAndValidWidth_WritesSomethingToFile()
        {
            int width = 200;
            ImageToAscii.ConvertToAscii(_inputs["cat.jpg"], _outputFileName, width);

            Assert.IsTrue(File.Exists(_outputFileName));
            Assert.AreNotEqual(0, File.ReadAllText(_outputFileName).Length);
        }

        [TestMethod]
        public void ValidGradientBitmapInput_WritesAllCharacters()
        {
            ImageToAscii.ConvertToAscii(_inputs["gradient.jpg"], _outputFileName, 500);

            string outputContents = File.ReadAllText(_outputFileName);

            foreach (var character in ImageToAscii.CharacterSet)
            {
                Assert.IsTrue(outputContents.Contains(character));
            }
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            RemoveOutputFile();
        }

        private void RemoveOutputFile()
        {
            //todo: properly mock objects so we don't write to disk, and instead capture attemps
            try
            {
                File.Delete(_outputFileName);
            }
            catch { }
        }
    }
}
