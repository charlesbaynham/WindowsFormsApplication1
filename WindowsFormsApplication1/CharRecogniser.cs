using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using BotSuite.ImageLibrary;
using BotSuite.Recognition.Character;

namespace BotTest
{
    /// <summary>
    ///     Class to recognise characters and strings from BMP images.
    /// </summary>
    class CharRecogniser
    {
        OCR MyOCR;

        /// <summary>
        ///     Initialises CharRecogniser with images to learn. 
        ///     Note that this cannot be added to later,
        ///     so the whole configuration must be discarded to add more letters in after initialisation. 
        /// </summary>
        /// <param name="ImageTraining">Dictionary of char -> list(ImageData) to be learnt.</param>
        public CharRecogniser(Dictionary<Char, List<ImageData>> ImageTraining)
        {
            init(ImageTraining);
        }
        /// <summary>
        ///     Initialises CharRecogniser with images to learn. Folder contains BW bitmap images to be learnt
        ///     as letters. 
        ///     Note that this cannot be added to later,
        ///     so the whole configuration must be discarded to add more letters in after initialisation. 
        /// </summary>
        /// <param name="ImageTraining">Dictionary of char -> folder of images.</param>
        public CharRecogniser(Dictionary<Char, string> ImageTraining)
        {
            Dictionary<Char, List<ImageData>> finalTraining = new Dictionary<char, List<ImageData>> { };

            foreach (char key in ImageTraining.Keys)
            {
                List<ImageData> tmpList = new List<ImageData> { };

                string[] images = Directory.GetFiles(ImageTraining[key], "*.bmp");

                for (int i = 0; i < images.GetLength(0); i++) {
                    tmpList.Add(new ImageData(images[i]));
                    Console.WriteLine(key + ": adding: " + images[i]);
                }

                finalTraining.Add(key, tmpList);
            }

            init(finalTraining);
        }

        void init(Dictionary<Char, List<ImageData>> ImageTraining)
        {
            MyOCR = new OCR(30);
            MyOCR.StartTrainingSession(ImageTraining);
        }

        /// <summary>
        ///     Recognise a single letter   
        /// </summary>
        /// <param name="letter">ImageData of the letter image to be recognised.</param>
        /// <returns></returns>
        public char RecogniseLetter(ImageData letter)
        {
            return MyOCR.Recognize(letter);
        }
        /// <summary>
        ///     Recognise a single letter   
        /// </summary>
        /// <param name="letter">Bitmap of the letter image to be recognised.</param>
        /// <returns></returns>
        public char RecogniseLetter(Bitmap letter) { return RecogniseLetter(new ImageData(letter)); }
        /// <summary>
        ///     Recognise a single letter   
        /// </summary>
        /// <param name="letter">Path to the bitmap letter image to be recognised.</param>
        /// <returns></returns>
        public char RecogniseLetter(string letter) { return RecogniseLetter(new ImageData(letter)); }
    }
}
