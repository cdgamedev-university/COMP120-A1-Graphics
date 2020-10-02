using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FilterSocialMedia {
    public partial class Contract5 : Form {
        public Contract5() {
            InitializeComponent();
        }

        private void Contract5_Load(object sender, EventArgs e) {
            LoadImage();
            SetupWindow();
        }

        private void LoadImage() {
            int splits = 20;
            Bitmap bmp = Image_Resources.Googling_Stuff;
            Bitmap[] bmps = SplitImage(bmp, splits);

            bmps = BulkMirror(bmps, 2, 0);
            bmps = BulkMakeGreyscale(bmps, 2, 1);
            bmps = BulkNegative(bmps, 3, 1);

            Picture.Image = MergeImage(bmps);
        }

        Bitmap[] BulkMakeGreyscale(Bitmap[] bmps, int index, int offset) {
            if (offset > index - 1) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Error. Offset will not work, please change");
                Console.ResetColor();
                return bmps;
            }
            for (int i = 0; i < bmps.Length; i++) {
                if (i % index == offset) {
                    bmps[i] = MakeGreyscale(bmps[i]);
                }
            }

            return bmps;
        }

        Bitmap[] BulkNegative(Bitmap[] bmps, int index, int offset)
        {
            if (offset > index - 1)
            {
                Console.WriteLine("Error. Offset will not work, please change");
                return bmps;
            }
            for (int i = 0; i < bmps.Length; i++)
            {
                if (i % index == offset)
                {
                    bmps[i] = Negative(bmps[i]);
                }
            }

            return bmps;
        }


        Bitmap[] BulkMirror(Bitmap[] bmps, int index, int offset) {
            if (offset > index - 1) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Error. Offset will not work, please change");  //comments would be nice
                Console.ResetColor();                                             // have read through and understand, amazing work
                return bmps;
            }
            for (int i = 0; i < bmps.Length; i++) {
                if (i % index == offset) {
                    bmps[i] = MirrorImage(bmps[i]);
                }
            }

            return bmps;
        }

        Bitmap MirrorImage(Bitmap bmp) {
            int width = bmp.Width;
            int height = bmp.Height;
            Bitmap new_bmp = new Bitmap(width, height);

            for (int i = 0; i < width; i++) {
                int new_i = width - 1 - i;
                for (int j = 0; j < height; j++) {
                    new_bmp.SetPixel(new_i, j, bmp.GetPixel(i, j));
                }
            }

            return new_bmp;
        }

        Bitmap MergeImage(Bitmap[] bmps) {
            int splits = bmps.Length;
            int width = bmps[0].Width;
            int height = bmps[0].Height * (splits - 1) + bmps[bmps.Length - 1].Height;

            Bitmap bmp = new Bitmap(width, height);

            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    int currentSplit = (int) j / (height / splits);
                    bmp.SetPixel(i, j, bmps[currentSplit].GetPixel(i, j - (bmps[currentSplit].Height * currentSplit)));
                }
            }

            return bmp;
        }

        Bitmap[] SplitImage(Bitmap bmp, int splits) {
            Bitmap[] bmps;

            int width = bmp.Width;
            int height = bmp.Height;
            int splitHeight = height / splits;

            int h = (int) Math.Ceiling((double)height / splitHeight);

            bmps = new Bitmap[h];

            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    int currentSplit = (int) j / (height / splits);
                    if (bmps[currentSplit] == null) {
                        bmps[currentSplit] = new Bitmap(width, height / splits);
                    }
                    bmps[currentSplit].SetPixel(i, j - (splitHeight * currentSplit), bmp.GetPixel(i, j));
                }
            }

            return bmps;
        }

        Bitmap Negative(Bitmap bmp)
        {
            
            int width = bmp.Width;
            int height = bmp.Height;


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color rRem = bmp.GetPixel(i, j);
                    int a = rRem.A;
                    int r = rRem.R;
                    int g = rRem.G;
                    int b = rRem.B;
                    int rI = 255 - r;
                    int gI = 255 - g;
                    int bI = 255 - b;


                    bmp.SetPixel(i, j, Color.FromArgb(a, rI, gI, bI));

                }
            }
            return bmp;
        }
        // make the image greyscale
        Bitmap MakeGreyscale(Bitmap bmp) {
            // get dimensions of image
            int width = bmp.Width;
            int height = bmp.Height;

            // run through all pixels of the image and calculate the average pixel colour, then set that pixel to the average
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Color pixel = bmp.GetPixel(i, j);

                    int avg = (pixel.R + pixel.G + pixel.B) / 3;

                    pixel = Color.FromArgb(255, avg, avg, avg);
                    bmp.SetPixel(i, j, pixel);
                }
            }


            // return the new image
            return bmp;
        }

        // set up the window and image
        private void SetupWindow() {
            // resize the screen to fit the image
            int width = Picture.Image.Width;
            int height = Picture.Image.Height;
            this.Size = new Size(width, height);

            // resize and reposition the image
            Picture.Size = new Size(width, height);
            Picture.Location = new Point(0, 0);
        }

        private void Picture_Click(object sender, EventArgs e)
        {

        }
    }
}