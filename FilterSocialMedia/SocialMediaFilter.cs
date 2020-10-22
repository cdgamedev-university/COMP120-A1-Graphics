using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Imaging;

namespace FilterSocialMedia {
    public partial class Contract5 : Form {

        // initialize an array to store the images' sections
        public static List<Bitmap> bmps = new List<Bitmap>();

        // set a constant image width and height
        const float MAX_IMAGE_WIDTH = 1280f;
        const float MAX_IMAGE_HEIGHT = 720f;

        // initialise the form
        public Contract5() {
            InitializeComponent();
        }

        // load the form
        private void Contract5_Load(object sender, EventArgs e) {
            // run the start program function
            StartProgram();
        }


        // start the program
        void StartProgram() {
            // start a stop watch
            Stopwatch s = Stopwatch.StartNew();

            // reset the Bitmap list
            bmps = new List<Bitmap>();

            // set the image to a default image
            Picture.Image = Image_Resources.NoImage;

            // setup the window
            SetupWindow();

            // stop the stop watch
            s.Stop();

            // log how long the program took to start
            Console.WriteLine($"Program started in: {s.ElapsedMilliseconds} ms");
        }

        // load the image
        void LoadImage() {
            // create a new dialog to import any bitmap image
            OpenFileDialog OFDialog = new OpenFileDialog {
                // set the initial directory to be the C drive
                InitialDirectory = "C:\\",
                // add some filters to the dialog
                Filter = "Supported image files (*.bmp, *.gif, *.exif, *.jpg, *.png, *.tiff)|*.bmp;*.exif;*.gif;*.jpg;*.png;*.tiff|"    // supported files
                + "All files (*.*)|*.*",                                                                                                // all files
                FilterIndex = 1,            // start the filter index at 1 (supported files)
                RestoreDirectory = true,    // enable restoring directory when the dialog is closed
                Title = "Load an image..."  // change the title to something more fitting
            };

            // if the dialog opens a file
            if (OFDialog.ShowDialog() == DialogResult.OK) {
                // start a stop watch
                Stopwatch s = Stopwatch.StartNew();

                // start a try catch for errors
                try {
                    // try running this code:

                    // load the file path
                    string filePath = OFDialog.FileName;
                    // load the bitmap from the file
                    Bitmap bmp = new Bitmap(filePath);
                    // set the image to the loaded bitmap
                    Picture.Image = bmp;
                } catch {
                    // in the event of an error:

                    // set the message for the user
                    string message = "This file doesn't appear to be a supported image format. Please choose a different image.";
                    // set the title of the dialog
                    string caption = "Image Import Error";
                    // set the buttons for the message box
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    // show the message box
                    MessageBox.Show(message, caption, buttons);
                    // rerun the function
                    LoadImage();
                }
                // stop the stop watch
                s.Stop();

                // log how long the image took to load
                Console.WriteLine($"Image loaded in: {s.ElapsedMilliseconds} ms");
            }
        }

        // function for saving the image
        void SaveImage() {

            // create a new save file dialog
            SaveFileDialog SFDialog = new SaveFileDialog {
                InitialDirectory = "C:\\",              // set the starting directory to the C drive
                // create a filter
                Filter = "Bitmap file (*.bmp)|*.bmp"    // bitmap file saving
                + "|GIF file (*.gif)|*.gif"             // gif file saving
                + "|exif file (*.exif)|*.exif"          // exif file saving
                + "|JPeg file (*.jpg)|*.jpg"            // jpg file saving
                + "|PNG file (*.png)|*.png"             // png file saving
                + "|TIFF file (*.tiff)|*.tiff",         // tiff file saving
                // start the filter at png files (common format)
                FilterIndex = 5,
                // set the title
                Title = "Save an image..."              
            };
            // show the dialog
            SFDialog.ShowDialog();

            // if the filename is not blank
            if (SFDialog.FileName != "") {
                // start a stop watch
                Stopwatch s = Stopwatch.StartNew();

                // switch the filter indexx
                switch (SFDialog.FilterIndex) {
                    // save as bmp
                    case 1:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Bmp);
                        break;
                    // save as gif
                    case 2:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Gif);
                        break;
                    // save as exif
                    case 3:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Exif);
                        break;
                    // save as jpeg
                    case 4:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Jpeg);
                        break;
                    // save as png
                    case 5:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Png);
                        break;
                    // save as tiff
                    case 6:
                        Picture.Image.Save(SFDialog.FileName, ImageFormat.Tiff);
                        break;
                }
                // stop the stop watch
                s.Stop();

                // log how long the image took to save
                Console.WriteLine($"Image saved in: {s.ElapsedMilliseconds} ms");
            }
        }

        // make changes to the image
        void ManipulateImage() {
            // start a stop watch
            Stopwatch s = Stopwatch.StartNew();

            // set the amount of splits
            int splits = 500;

            // split the image
            ImageTool.Split(new Bitmap(Picture.Image), splits);
            
            // mirror every 2 sections (2n)
            Mirror.Bulk(2, 0);

            // make every 2 sections negative offset by 1 (2n+1)
            Negative.Bulk(2, 1);

            // swap the red and blue channels for every 3 seconds offset by 0 (3n)
            SwapChannels.Bulk(SwapChannels.Channel.Blue, SwapChannels.Channel.Red, 3, 0);

            // make every 3 sections greyscale offset by 2 (3n+2)
            Greyscale.Bulk(3, 2);

            // merge the new image and show it
            Picture.Image = ImageTool.Merge(bmps);

            // setup the windows
            SetupWindow();
            
            // stop the stop watch
            s.Stop();

            // log how long the image took to manipulate
            Console.WriteLine($"Image manipulation took: {s.ElapsedMilliseconds} ms");
        }

        // set up the window and image
        private void SetupWindow() {
            // if the bitmap list is empty
            if (bmps.Count == 0) {
                //  hide the apply filter, export and restart buttons
                btnApplyFilter.Hide();
                btnExportImage.Hide();
                btnRestart.Hide();
                // show the load image button
                btnLoadImage.Show();

                // set the size of the form and picture box
                this.Size = new Size(640, 360);
                Picture.Size = new Size(600, 200);
            } 
            // if the bitmap list contains sections
            else {
                // resize the screen to fit the image
                float width = Picture.Image.Width;
                float height = Picture.Image.Height;

                // if the width is bigger than the width area of the screen
                if (width > Screen.PrimaryScreen.WorkingArea.Width) {
                    height = MAX_IMAGE_WIDTH / width * height;
                    width = MAX_IMAGE_WIDTH;
                }
                // if the height is bigger than the working height of the screen
                if (height > Screen.PrimaryScreen.WorkingArea.Height) {
                    width = (MAX_IMAGE_HEIGHT / height) * width;
                    height = MAX_IMAGE_HEIGHT;
                }

                // log the dimensions of the image
                Console.WriteLine("Width: " + width + ", Height: " + height);

                // calculate the new height of the image
                int imageWidth = (int)Math.Floor((double)width);
                int imageHeight = (int)Math.Floor((double)height);

                // resize and reposition the image
                Picture.SizeMode = PictureBoxSizeMode.Zoom;
                Picture.Size = new Size(imageWidth, imageHeight);
                Picture.Location = new Point(0, 0);

                // set the size of the form to add space for some butons
                this.Size = new Size(imageWidth, imageHeight + 120);

                // hide the apply filter and load image buttons
                btnApplyFilter.Hide();
                btnLoadImage.Hide();

                // show the export image and restart buttons
                btnExportImage.Show();
                btnRestart.Show();
            }

            // center the form to the screen
            CenterToScreen();
        }
        
        // when the load image button is pressed
        private void btnLoadImage_Click(object sender, EventArgs e) {
            // load the image and show the filter button
            LoadImage();
            btnApplyFilter.Show();
        }

        // when the apply filter button is pressed
        private void btnApplyFilter_Click(object sender, EventArgs e) {
            // manipulate the image
            ManipulateImage();
        }

        // when the export image button is pressed
        private void btnExportImage_Click(object sender, EventArgs e) {
            // save the image
            SaveImage();
        }

        // when the restart button is pressed
        private void btnRestart_Click(object sender, EventArgs e) {
            // restart the program
            StartProgram();
        }
    }
    //starting a new class
    public static class Greyscale {

        // make the image greyscale
        public static Bitmap Single(Bitmap bmp) {
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

        /// <summary>
        /// make multiple bitmaps greyscale
        /// </summary>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        /// <returns></returns>
        public static void Bulk(int multiple, int offset) {
            // run through the bitmaps (starting at offset)
            for (int i = offset; i < Contract5.bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // make the bitmap greyscale
                    Contract5.bmps[i] = Single(Contract5.bmps[i]);
                }
            }
        }
    }
    //starting a new class
    public static class SwapChannels {

        // different color channels and their corresponding location values (DO NOT CHANGE)
        public enum Channel {
            Alpha = 0,
            Red = 1,
            Green = 2,
            Blue = 3
        }

        // swap channels for the image using a Bitmap and then two channel locations
        public static Bitmap Single(Bitmap bmp, Channel channel0, Channel channel1) {
            // get width and height of the image
            int width = bmp.Width;
            int height = bmp.Height;

            // convert channel values into int values
            int[] channelsSwapping = { (int)channel0, (int)channel1 };

            // loop through all pixels on the bmp
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    // create a new int array for storing the channel values
                    int[] channel = new int[4];

                    // get the current pixel
                    Color pixel = bmp.GetPixel(i, j);

                    // move all pixel ARGB info into the channels array (makes accessing easier)
                    channel[0] = pixel.A;
                    channel[1] = pixel.R;
                    channel[2] = pixel.G;
                    channel[3] = pixel.B;

                    // create a temp value and set it to the first channel "Channel channel0"
                    int temp = channel[channelsSwapping[0]];
                    // set the first channel value "Channel channel0" to the second value "Channel channel1"
                    channel[channelsSwapping[0]] = channel[channelsSwapping[1]];
                    // set the second channel value "Channel channel1" to the temp value
                    channel[channelsSwapping[1]] = temp;

                    // recreate the pixel with the moved values
                    pixel = Color.FromArgb(channel[0], channel[1], channel[2], channel[3]);

                    // set the pixel in the image
                    bmp.SetPixel(i, j, pixel);
                }
            }

            // return the image
            return bmp;
        }

        public static void Bulk(Channel channel0, Channel channel1, int multiple, int offset) {
            for (int i = offset; i < Contract5.bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // swap channels of the bitmap
                    Contract5.bmps[i] = Single(Contract5.bmps[i], channel0, channel1);
                }
            }
        }
    }
    //starting a new class
    public static class Mirror { 

        // mirroring the images vertically
        public static Bitmap Single(Bitmap bmp) {
            // get the dimensions of the bitmap
            int width = bmp.Width;
            int height = bmp.Height;

            // create a new bitmap of the same size
            Bitmap new_bmp = new Bitmap(width, height);

            // run through all pixels of the bitmap
            for (int i = 0; i < width; i++) {
                // calculate the new i value
                int new_i = width - 1 - i;
                for (int j = 0; j < height; j++) {
                    // set the pixel at the mirrored point
                    new_bmp.SetPixel(new_i, j, bmp.GetPixel(i, j));
                }
            }

            // return the new bitmap
            return new_bmp;
        }

        /// <summary>
        /// Make multiple bitmaps mirrored
        /// </summary>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        public static void Bulk(int multiple, int offset) {
            // run through the bitmaps (startign at the offset)
            for (int i = offset; i < Contract5.bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // mirror the bitmap
                    Contract5.bmps[i] = Single(Contract5.bmps[i]);
                }
            }
        }
    }
    //starting a new class
    public static class Negative {
        // make images negative
        public static Bitmap Single(Bitmap bmp) {
            // get the dimensions of the image
            int width = bmp.Width;
            int height = bmp.Height;

            // run through all the pixels in the image
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    // get the colour of the current pixel
                    Color pixel = bmp.GetPixel(i, j);

                    // get the pixel channel values
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    // invert the pixel colors
                    int rI = 255 - r;
                    int gI = 255 - g;
                    int bI = 255 - b;

                    // set the pixel with the inverted values
                    bmp.SetPixel(i, j, Color.FromArgb(255, rI, gI, bI));
                }
            }

            // return the image
            return bmp;
        }

        /// <summary>
        /// make multiple bitmaps negative
        /// </summary>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        public static void Bulk(int multiple, int offset) {
            // run through the bitmaps (startign at the offset)
            for (int i = offset; i < Contract5.bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // make the bitmap negative
                    Contract5.bmps[i] = Single(Contract5.bmps[i]);
                }
            }
        }
    }
    //starting a new class
    public static class ImageTool {
        // split up images into several bitmaps
        public static void Split(Bitmap bmp, int splits) {
            // get the dimensions of the bitmap
            int width = bmp.Width;
            int height = bmp.Height;

            // if there are more splits than the image can handle
            if (splits > height) {
                // set the splits to the maximum
                splits = height;
            }

            // calculate the height of each split
            int splitHeight = height / splits;

            // calculate the amount of splits (if the image isn't divisible buy the original value, it will create additional splits)
            int h = (int)Math.Ceiling((double)height / splitHeight);

            // create a new list of the correct length to store the bitmaps
            Contract5.bmps = new List<Bitmap>(h);

            // run through every pixel of the bitmap
            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    // calculate which split should be worked on now
                    int currentSplit = (int)j / (height / splits);
                    // if the split doest exist, make a new bitmap
                    if (Contract5.bmps.Count == currentSplit) {
                        Contract5.bmps.Add(new Bitmap(width, height / splits));
                    }

                    // set the pixel from the current location to the right location on the split
                    Contract5.bmps[currentSplit].SetPixel(i, j - (splitHeight * currentSplit), bmp.GetPixel(i, j));
                }
            }
        }



        // merge a split image together
        public static Bitmap Merge(List<Bitmap> bmps) {
            // calculate the amount of splits
            int splits = bmps.Count;

            // get the width and height of the final bitmap
            int width = bmps[0].Width;
            int height = bmps[0].Height * (splits - 1) + bmps[bmps.Count - 1].Height;

            // create a new bitmap for the images to merge into
            Bitmap bmp = new Bitmap(width, height);

            // run through all the pixels of the new bitmap
            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    // calculate which split we are currently working with
                    int currentSplit = (int)j / (height / splits);

                    // set the pixel based off the split pixel value at the right location
                    bmp.SetPixel(i, j, bmps[currentSplit].GetPixel(i, j - (bmps[currentSplit].Height * currentSplit)));
                }
            }

            // return the bitmap
            return bmp;
        }
    }
}