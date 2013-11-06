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
                    ImageData tmpImg = new ImageData(images[i]);

                    tmpImg = toBlackAndWhite(tmpImg);
                    tmpImg = CropImage(tmpImg);

                    tmpList.Add(tmpImg);

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
            letter = toBlackAndWhite(letter);

            letter = CropImage(letter);

            //debug
            letter.Save("croppedLetter.bmp");

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

        /// <summary>
        /// Convert an image to black and white
        /// </summary>
        /// <param name="input">ImageData object to be converted</param>
        /// <returns></returns>
        ImageData toBlackAndWhite(ImageData input)
        {
            Filter.BlackAndWhite(ref input, 127);
            return input;
        }

        /// <summary>
        ///     Recognise a bmp string of letters and output the result
        /// </summary>
        /// <param name="letters">ImageData object of string</param>
        /// <returns></returns>
        public string RecogniseString(ImageData letters)
        {
            // Convert to B&W:
            letters = toBlackAndWhite(letters);

            // debug
            letters.Save("string-BW.bmp");

            // Split:
            List<ImageData> splitLetters = SplitString(letters);

            // Recognise letters:
            string output = "";
            foreach (ImageData letter in splitLetters)
            {
                output += RecogniseLetter(letter);
            }

            return output;
        }
        /// <summary>
        ///     Recognise a bmp string of letters and output the result
        /// </summary>
        /// <param name="letters">Path to .bmp of string</param>
        /// <returns></returns>
        public string RecogniseString(string letters) { return RecogniseString(new ImageData(letters)); }

        /// <summary>
        /// Crop an image based on black parts. Outputs the cropped image
        /// </summary>
        /// <param name="input">Imagedata object of image to be cropped</param>
        /// <returns></returns>
        ImageData CropImage(ImageData input){

            /* --------------- divide letters -------------------------------------------- */
            // column which can  be a separator --> we can draw a vertical line without hitting a letter
            
            int top = 0, bottom = input.Height-1, left=0, right=input.Width-1;

            // test from left to right
            for (Int32 column = 0; column < input.Width; column++)
            {
                bool found = false;
                // test from top to bottom
                for (Int32 row = 0; row < input.Height; row++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + letters.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // Left of image found
                        left = column - 1;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            // test from right to left
            for (Int32 column = input.Width-1; column >= 0; column--)
            {
                bool found = false;
                // test from top to bottom
                for (Int32 row = 0; row < input.Height; row++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + letters.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // Right of image found
                        right = column + 1;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            // Repeat for rows:

            // test from top to bottom
            for (Int32 row = 0; row < input.Height; row++)
            {
                // test from left to right
                bool found = false;
                for (Int32 column = 0; column < input.Width; column++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + letters.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // Top of image found
                        top = row - 1;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            // test from bottom to top
            for (Int32 row = input.Height-1; row >= 0; row--)
            {
                // test from left to right
                bool found = false;
                for (Int32 column = 0; column < input.Width; column++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + letters.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // bottom of image found
                        bottom = row + 1;
                        found = true;
                        break;
                    }
                }
                if (found) break; 
            }

            return new ImageData(input.CreateBitmap(left, top, right - left + 1, bottom - top + 1));
        }

        /// <summary>
        /// Split an image into letters based on whitespace
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        List<ImageData> SplitString(ImageData letters)
        {
            List<ImageData> output = new List<ImageData> { };

            /* --------------- divide letters -------------------------------------------- */
            // column which can  be a separator --> we can draw a vertical line without hitting a letter
            bool[] blankes = new bool[letters.Width];
            // test from left to right
            for (Int32 column = 0; column < letters.Width; column++)
            {
                // we assume it is possible
                bool blank = true;
                // test from top to bottom
                for (Int32 row = 0; row < letters.Height; row++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + letters.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (letters.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // no line can be drawn
                        blank = false;
                        break;
                    }
                }
                // test was passed sucessfully
                if (blank)
                {
                    // line can be drawn
                    blankes[column] = true;
                    // draw line in gray
                    for (int row = 0; row < letters.Height; row++)
                        letters.SetPixel(column, row, Color.FromArgb(225, 225, 225));
                }
                else
                {
                    // line cannot be drawn
                    blankes[column] = false;
                }
            }


            /* --------------- determine start and end of the letters -------------------------------------------- */
            bool open = false;          // = false --> nor red area
            int left = 0, width = 0;    // remember position
            for (Int32 column = 0; column < letters.Width; column++)
            {
                // not in gray area?
                if (!blankes[column])
                {
                    if (open == false)
                    {
                        // new start of letter
                        left = column;
                        // draw green vertical line
                        for (int row = 0; row < letters.Height; row++)
                            letters.SetPixel(column - 1, row, Color.Green);
                    }
                    open = true;
                }
                else // in gray area
                {
                    if (open == true) // start of gray area
                    {
                        // => end of current letter
                        width = column - left - 1;
                        if (width > 1)
                        {
                            ImageData tmpLetter = new ImageData(letters.CreateBitmap(left, 0, width, letters.Height - 3));
//                            output.Add(new ImageData(letters.CreateBitmap(left, 0, width, letters.Height - 3)));

                            output.Add(tmpLetter);
                            //tmpLetter.Save("splitting.bmp");

                            // draw blue vertical line
                            for (int row = 0; row < letters.Height; row++)
                                letters.SetPixel(column + 1, row, Color.Blue);
                            left = column;
                            open = false;
                        }

                    }

                }
            }
            letters.Save("test2.bmp");

            //debug
            output.First().Save("splitout1.bmp");
            return output;
        }


    }
}
