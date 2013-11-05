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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
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

        private void Form1_Load(object sender, EventArgs e) { }

        private void showLetterFloats(ImageData letter, OCR MyOCR)
        {
            float[] test = MyOCR.GetMagicSticksPattern(letter);  // To see whats going on
            MessageBox.Show(string.Join(" ", test));  // show the resulst from the magic sticks
        }
        private void showLetterFloats(string letter, OCR MyOCR)
        { showLetterFloats(new ImageData(letter), MyOCR); }

        private void button2_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory("letters");
            string[] letters = Directory.GetDirectories(".");

            Console.WriteLine(letters[1]);

            for (int i = 0; i < letters.GetLength(0); i++ )
            {
                letters[i] = letters[i].Remove(0, 2);
            }

            OCR MyOCR = InitNetwork();

            ImageData letterImg = new ImageData("b\\letterB.bmp");

            //showLetterFloats("a\\letterA.bmp", MyOCR);
            //showLetterFloats("a\\a2.bmp", MyOCR);
            //showLetterFloats("b\\letterB.bmp", MyOCR);
            //showLetterFloats("b\\b2.bmp", MyOCR);

            char result = MyOCR.Recognize(letterImg);

            //float[] erg = MyOCR.GetAnswer(ImgTmp);    // try to recognise
            //MessageBox.Show(string.Join(" ", erg));   // show the result

            Console.WriteLine("Output should be \"A\". It is: " + result);

            string[] dirs = Directory.GetFiles(@"a");
            foreach (string dir in dirs)
            {
                Bitmap tmpBmp = new Bitmap(dir);
                ImageData tmpImg = new ImageData(tmpBmp);

                Console.WriteLine("File " + dir + " gives \"" + MyOCR.Recognize(tmpImg) + "\"");
            }

            string[] dirs2 = Directory.GetFiles(@"b");
            foreach (string dir in dirs2)
            {
                Bitmap tmpBmp = new Bitmap(dir);
                ImageData tmpImg = new ImageData(tmpBmp);

                Console.WriteLine("File " + dir + " gives \"" + MyOCR.Recognize(tmpImg) + "\"");
            }
            
        }


        class CharRecogniser
        {


            private OCR InitNetwork()
            {
                Dictionary<Char, List<ImageData>> ImageTraining = new Dictionary<Char, List<ImageData>> { };
                List<ImageData> ImagesForA = new List<ImageData> { };
                List<ImageData> ImagesForB = new List<ImageData> { };

                string[] dirs = Directory.GetFiles(@"a");
                foreach (string dir in dirs)
                {
                    Bitmap tmpBmp = new Bitmap(dir);
                    ImageData tmpImg = new ImageData(tmpBmp);
                    ImagesForA.Add(tmpImg);

                }
                ImageTraining.Add('a', ImagesForA);

                string[] dirs2 = Directory.GetFiles(@"b");
                foreach (string dir in dirs2)
                {
                    ImageData tmpImg;
                    Bitmap tmpBmp = new Bitmap(dir);
                    tmpImg = new ImageData(tmpBmp);
                    ImagesForB.Add(tmpImg);

                }
                ImageTraining.Add('b', ImagesForB);

                OCR MyOCR = new OCR(30);
                MyOCR.StartTrainingSession(ImageTraining);
                //MyOCR.RandomTrain(50000);

                return MyOCR;
            }
        }

    }

    
}
