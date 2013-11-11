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
        //CharRecogniser recog;
        

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
            ImageData refpic = new ImageData("pic1.bmp");
            // use the imageclass of the BotSuite.NET and copy the captured screen into it
            ImageData source = new ImageData(CapturedScreen);
            ImageData rightEdge = new ImageData("pic2.bmp");
            // list of all found icons:
            // search all matches in the picture

            uint tolerance = 0;
            List<Rectangle> AllMatches;
            while (true)
            {
                AllMatches = Template.AllImages(source, refpic, tolerance);
                if (AllMatches.Any())
                    break;

                tolerance += 10;
            }

            tolerance = 0;
            List<Rectangle> RightMatches;
            while (true)
            {
                RightMatches = Template.AllImages(source, rightEdge, tolerance);
                if (RightMatches.Any())
                    break;

                tolerance += 10;
            }
            
            
            
            // try to activate the browser by doing a leftclick
            //Mouse.Move(AllMatches.First().Left, AllMatches.First().Top, true, 20);
            //Mouse.Move(this.Left + this.Width, this.Top, true, 200);
            //Mouse.Move(this.Left + this.Width, this.Top + 400, true, 200);
            //Mouse.Move(this.Left, this.Top + 400, true, 200);

            

            // click all buildings below the icons
            for (int i = 0; i < AllMatches.Count; i++)
            {
                // extract the coordinates
                //Point target = new Point(AllMatches[i].Left, AllMatches[i].Bottom);
                // move to the building (add some offset as a distance from the icon to the building)
                //Mouse.MoveRelativeToWindow(this.Handle, target.X + 15, target.Y + 65, true, 50);
                Mouse.Move(AllMatches[i]);
                // jiggle the mouse as an human
                Mouse.Jiggle();
                // we are human, so we are not that fast as a computer
                Utility.Delay(500, 1000);
                // do a leftclick to collect simoleons
                //Mouse.LeftClick();
                // wait again
                Utility.Delay(1000, 2000);

                Mouse.Move(RightMatches[i].Right-2, RightMatches[i].Top);
                // jiggle the mouse as an human
                Mouse.Jiggle();
                // we are human, so we are not that fast as a computer
                Utility.Delay(500, 1000);
                // do a leftclick to collect simoleons
                //Mouse.LeftClick();
                // wait again
                Utility.Delay(1000, 2000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (var recog = new TessOCRforEVE())
            {
                //lblOutput.Text = "String is : " + recog.RecogniseString(new ImageData("string.bmp"));
                lblOutput2.Text = "String is : " + recog.RecogniseString(new ImageData("string.bmp"), System.Convert.ToInt16(boxMult.Text) );                
            }
        }

        private void btnRelearn_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        
    }
}
