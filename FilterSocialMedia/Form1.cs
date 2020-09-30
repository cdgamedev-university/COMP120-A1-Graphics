using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterSocialMedia {
    public partial class Contract5 : Form {
        public Contract5() {
            InitializeComponent();
        }

        private void Contract5_Load(object sender, EventArgs e) {
            LoadImage();
            ResizeScreen();
        }

        private void LoadImage() {
            var img = Resource1._0sat8zoomed0z1l6951;

            Bitmap bmp = new Bitmap(img);

            Picture.Image = MakeGreyscale(bmp);
        }

        Bitmap MakeGreyscale(Bitmap bmp) {
            int width = bmp.Width;
            int height = bmp.Height;

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Color pixel = bmp.GetPixel(i, j);

                    int avg = (pixel.R + pixel.G + pixel.B) / 3;

                    pixel = Color.FromArgb(255, avg, avg, avg);
                    bmp.SetPixel(i, j, pixel);
                }
            }

            return bmp;
        }






















        private void ResizeScreen() {

        }

        private void Picture_Click(object sender, EventArgs e)
        {

        }
    }
}
