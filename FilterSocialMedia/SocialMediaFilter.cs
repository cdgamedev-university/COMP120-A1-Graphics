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

namespace FilterSocialMedia {
    public partial class Contract5 : Form {

        public List<Bitmap> bmps = new List<Bitmap>();

        // different color channels and their corresponding location values (DO NOT CHANGE)
        public enum Channel {
            Alpha = 0,
            Red = 1,
            Green = 2,
            Blue = 3
        }

        // initialise the form
        public Contract5() {
            InitializeComponent();
        }

        // load the form
        private void Contract5_Load(object sender, EventArgs e) {
            Bitmap bmp = Image_Resources.Googling_Stuff;
            int splits = 10;
            bmps = SplitImage(bmp, splits);
            Picture.Image = ManipulateImage(bmp);
            SetupWindow();
        }

        // load in the image and make changes
        Bitmap ManipulateImage(Bitmap bmp) {
            if (GreyscaleChk.Checked == true) {
                BulkGreyscale(2, 1);
            }

            return MergeImage(bmps);
        }

        // set up the window and image
        private void SetupWindow() {
            // resize the screen to fit the image
            int width = Picture.Image.Width + 250;
            int height = Picture.Image.Height;
            this.Size = new Size(width, height);

            // resize and reposition the image
            Picture.Size = new Size(width, height);
            Picture.Location = new Point(0, 0);
        }

        List<Bitmap> BulkSwapChannels(Channel channel0, Channel channel1, int multiple, int offset) {
            for (int i = offset; i < bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // swap channels of the bitmap
                    bmps[i] = SwapChannels(bmps[i], channel0, channel1);
                }
            }

            return bmps;
        }

        // swap channels for the image using a Bitmap and then two channel locations
        Bitmap SwapChannels(Bitmap bmp, Channel channel0, Channel channel1) {
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

        /// <summary>
        /// make multiple bitmaps greyscale
        /// </summary>
        /// <param name="bmps">an array of bitmaps you want to change</param>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        /// <returns></returns>
        List<Bitmap> BulkGreyscale(int multiple, int offset) {
            // run through the bitmaps (starting at offset)
            for (int i = offset; i < bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // make the bitmap greyscale
                    bmps[i] = Greyscale(bmps[i]);
                }
            }

            // return the bitmaps
            return bmps;
        }

        /// <summary>
        /// make multiple bitmaps negative
        /// </summary>
        /// <param name="bmps">an array of bitmaps you want to change</param>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        /// <returns></returns>
        List<Bitmap> BulkNegative(int multiple, int offset) {
            // run through the bitmaps (startign at the offset)
            for (int i = offset; i < bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // make the bitmap negative
                    bmps[i] = Negative(bmps[i]);
                }
            }

            // return the bitmaps
            return bmps;
        }

        /// <summary>
        /// make multiple bitmaps mirrored
        /// </summary>
        /// <param name="bmps">an array of bitmaps you want to change</param>
        /// <param name="multiple">the multiple you want to change (ex. 1 for every bitmap, 2 for every 2, 3 for every 3)</param>
        /// <param name="offset">the offset you want to start at (ex. an offset of 1 will skip the first one)</param>
        /// <returns></returns>
        List<Bitmap> BulkMirror(int multiple, int offset) {
            // run through the bitmaps (startign at the offset)
            for (int i = offset; i < bmps.Count; i++) {
                // if i is at the right multiple and offset
                if (i % multiple == offset % multiple) {
                    // mirror the bitmap
                    bmps[i] = MirrorImage(bmps[i]);
                }
            }

            // return the bitmaps
            return bmps;
        }

        // mirroring the images vertically
        Bitmap MirrorImage(Bitmap bmp) {
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

        // merge a split image together
        Bitmap MergeImage(List<Bitmap> bmps) {
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
                    int currentSplit = (int) j / (height / splits);

                    // set the pixel based off the split pixel value at the right location
                    bmp.SetPixel(i, j, bmps[currentSplit].GetPixel(i, j - (bmps[currentSplit].Height * currentSplit)));
                }
            }

            // return the bitmap
            return bmp;
        }


        // split up images into several bitmaps
        List<Bitmap> SplitImage(Bitmap bmp, int splits) {
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
            int h = (int) Math.Ceiling((double)height / splitHeight);

            // create a new list of the correct length to store the bitmaps
            bmps = new List<Bitmap>(h);

            // run through every pixel of the bitmap
            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    // calculate which split should be worked on now
                    int currentSplit = (int) j / (height / splits);
                    // if the split doest exist, make a new bitmap
                    if (bmps.Count == currentSplit) {
                        bmps.Add(new Bitmap(width, height / splits));
                    }
                    
                    // set the pixel from the current location to the right location on the split
                    bmps[currentSplit].SetPixel(i, j - (splitHeight * currentSplit), bmp.GetPixel(i, j));
                }
            }

            // return the bitmaps
            return bmps;
        }

        // make images negative
        Bitmap Negative(Bitmap bmp) {
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

        // make the image greyscale
        Bitmap Greyscale(Bitmap bmp) {
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

        void GreyscaleChk_CheckedChanged(object sender, EventArgs e) {
            Stopwatch s = Stopwatch.StartNew();

            Bitmap bmp = Image_Resources.Googling_Stuff;
            if (GreyscaleChk.Checked) {
                Picture.Image = ManipulateImage(bmp);
            }

            s.Stop();

            Console.WriteLine($"Execution Time: {s.ElapsedMilliseconds} ms");
        }
    }
}