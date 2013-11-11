using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using BotSuite.ImageLibrary;
using BotSuite.Recognition.Character;

using Tesseract;

namespace BotTest
{
    /// <summary>
    ///     Class to recognise characters and strings from BMP images.
    /// </summary>
    class TessOCRforEVE : IDisposable
    {
        TesseractEngine engine;

        public void Dispose() { engine.Dispose(); }

        /// <summary>
        /// Initiate the Tesseract OCR engine with english datafiles
        /// </summary>
        /// <param name="dir">Path to datafiles directory (default = "./tessdata")</param>
        /// <param name="lang">Language (default = "eng")</param>
        public TessOCRforEVE(string dir=@"./tessdata", string lang="eng") { engine = new TesseractEngine(dir, lang, EngineMode.Default); }

        /// <summary>
        ///     Recognise a bmp string of letters and output the result
        /// </summary>
        /// <param name="letters">ImageData object of string</param>
        /// <param name="multiple">Multiple by which to scale up the image</param>
        /// <returns></returns>
        public string RecogniseString(ImageData letters, int multiple = 5)
        {

            // to Black and white
            //Filter.BlackAndWhite(ref letters, 85);
            //letters.Save("BW.bmp");

            // Boost resolution
            letters = letters.Resize(multiple, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            CropImage(letters).Save("boosted.bmp");
            
            // Invert colours
            Filter.Invert(ref letters);
            letters.Save("inverted.bmp");
            
            Filter.Sharpen(ref letters, 1000);

            // debug
            letters.Save("string-for-recognition.bmp");
            //engine.SetVariable("VAR_CHAR_WHITELIST", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmopqrstuvwxyz0123456789%,");
            engine.SetVariable("tessedit_char_whitelist", "0123456789,.ISK");

            using (var page = engine.Process(letters.CreateBitmap()))
            {
                var output = page.GetText();
                return output;
            }
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
        ImageData CropImage(ImageData input)
        {

            /* --------------- divide letters -------------------------------------------- */
            // column which can  be a separator --> we can draw a vertical line without hitting a letter

            int top = 0, height = input.Height, left = 0, width = input.Width;

            //debug
            input.Save("cropin.bmp");

            // test from left to right
            for (Int32 column = 0; column < input.Width; column++)
            {
                bool found = false;
                // test from top to bottom
                for (Int32 row = 0; row < input.Height; row++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + input.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // Left of image found
                        left = column;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            // test from right to left
            for (Int32 column = input.Width - 1; column >= 0; column--)
            {
                bool found = false;
                // test from top to bottom
                for (Int32 row = 0; row < input.Height; row++)
                {
                    //Console.WriteLine(column + ", " + row + "= " + input.GetPixel(column, row) + "  whereas black: " + Color.FromArgb(0,0,0));

                    if (input.GetPixel(column, row) == Color.FromArgb(0, 0, 0))
                    {
                        // Pixel is black
                        // Right of image found
                        width = column - left + 1;
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
                        top = row;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            // test from bottom to top
            for (Int32 row = input.Height - 1; row >= 0; row--)
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
                        height = row - top + 1;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            return new ImageData(input.CreateBitmap(left, top, width, height));
        }

        /// <summary>
        /// Split an image into letters based on whitespace
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        List<ImageData> SplitString(ImageData letters)
        {
            List<ImageData> output = new List<ImageData> { };

            //debug
            letters.Save("splitIn.bmp");

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
                        width = column - left;
                        if (width > 1)
                        {
                            ImageData tmpLetter = new ImageData(letters.CreateBitmap(left, 0, width, letters.Height - 3));
                            //                            output.Add(new ImageData(letters.CreateBitmap(left, 0, width, letters.Height - 3)));

                            output.Add(tmpLetter);
                            //tmpLetter.Save("splitting.bmp");

                            // draw blue vertical line
                            for (int row = 0; row < letters.Height; row++)
                                letters.SetPixel(column, row, Color.Blue);
                            left = column;
                            open = false;
                        }

                    }

                }
            }
            //debug
            letters.Save("split.bmp");

            return output;
        }


    }
}
