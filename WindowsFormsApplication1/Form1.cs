using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotSuite;
using BotSuite.ImageLibrary;
using BotSuite.Recognition.Character;

namespace BotTest
{
    public partial class Form1 : Form
    {
        CharRecogniser recog;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // take a screenshot below the window (height of screenshot=400 px starting from the windows top)
            Bitmap CapturedScreen = ScreenShot.Create();
            //Bitmap CapturedScreen = ScreenShot.Create(this.Left, this.Top, this.Width, 400);
            // our cropped image of the blizzard
            ImageData refpic = new ImageData("blitz1.bmp");
            // use the imageclass of the BotSuite.NET and copy the captured screen into it
            ImageData source = new ImageData(CapturedScreen);
            // list of all found icons
            List<Rectangle> AllBlizzards = new List<Rectangle>();
            // search all blizzards in the picture and use a tolerance of 124
            AllBlizzards = Template.AllImages(source, refpic, 124);
            // try to activate the browser by doing a leftclick
            Mouse.Move(this.Left, this.Top, true, 200);
            //Mouse.Move(this.Left + this.Width, this.Top, true, 200);
            //Mouse.Move(this.Left + this.Width, this.Top + 400, true, 200);
            //Mouse.Move(this.Left, this.Top + 400, true, 200);

            OCR recog = new OCR();

            recog.Recognize(new ImageData("letterC.bmp"));

            System.Console.WriteLine("We think it's:  " + recog.Recognize(new ImageData("letterC.bmp")));

            // click all buildings below the icons
            for (int i = 0; i < AllBlizzards.Count; i++)
            {
                // extract the coordinates
                Point target = new Point(AllBlizzards[i].Left, AllBlizzards[i].Bottom);
                // move to the building (add some offset as a distance from the icon to the building)
                //Mouse.MoveRelativeToWindow(this.Handle, target.X + 15, target.Y + 65, true, 50);
                Mouse.Move(target);
                // jiggle the mouse as an human
                Mouse.Jiggle();
                // we are human, so we are not that fast as a computer
                Utility.Delay(500, 1000);
                // do a leftclick to collect simoleons
                Mouse.LeftClick();
                // wait again
                Utility.Delay(1000, 2000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<char, string> letterDict = new Dictionary<char, string> { };
            letterDict.Add('a', "letters\\a");
            letterDict.Add('b', "letters\\b");

            recog = new CharRecogniser(letterDict);
        }

        /// <summary>
        ///     Pops up a debug box with the magic sticks analysis of the given image
        /// </summary>
        /// <param name="letter">The image</param>
        /// <param name="MyOCR">The OCR object</param>
        private void showLetterFloats(ImageData letter, OCR MyOCR)
        {
            float[] test = MyOCR.GetMagicSticksPattern(letter);  // To see whats going on
            MessageBox.Show(string.Join(" ", test));  // show the results from the magic sticks
        }
        /// <summary>
        ///     Pops up a debug box with the magic sticks analysis of the given image
        /// </summary>
        /// <param name="letter">The path to the image</param>
        /// <param name="MyOCR">The OCR object</param>
        private void showLetterFloats(string letter, OCR MyOCR)
        { showLetterFloats(new ImageData(letter), MyOCR); }


        private void button2_Click(object sender, EventArgs e)
        {
            lblOutput.Text = "String is : " + recog.RecogniseString(new ImageData("string.bmp"));
        }

    }
}
