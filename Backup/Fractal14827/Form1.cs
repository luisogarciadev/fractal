using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fractal14827
{
    public partial class Form1 : Form
    {
        Bitmap imagen;
        int iteraciones = 512;
        Buffer buffer;
        protected const int max = 400;
        protected int[] arregloColores = 
		{
			Color.Red.ToArgb(),
			Color.Black.ToArgb(),
            Color.Blue.ToArgb(),
            Color.Yellow.ToArgb(),
            Color.Pink.ToArgb(),
            Color.Purple.ToArgb(),
            Color.Brown.ToArgb(),
			Color.Green.ToArgb(),
            Color.White.ToArgb(),
            //Color.DarkMagenta.ToArgb(),
            //Color.Crimson.ToArgb(),
            //Color.DarkRed.ToArgb(),
            //Color.PaleVioletRed.ToArgb(),
            //Color.Silver.ToArgb(),
            //Color.Gray.ToArgb(),
            //Color.GhostWhite.ToArgb()
		};

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            buffer = new Buffer(ClientSize.Height, ClientSize.Width);
            Draw();
        }

        public void animacion(int offset)
        {
            int maxColores = arregloColores.Length;
            BitmapData imagen2 = imagen.LockBits(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            unsafe
            {
                int* pImagen = (int*)imagen2.Scan0;
                fixed (int* pCache = &buffer.cache[0, 0])
                {
                    int* pIter = pCache;
                    for (int y = 0; y < ClientSize.Height; y++)
                    {
                        for (int x = 0; x < ClientSize.Width; x++)
                        {
                            if (*pIter == iteraciones)
                                *pImagen++ = Color.Black.ToArgb();
                            else
                                *pImagen++ = arregloColores[(*pIter + offset) % maxColores];
                            ++pIter;
                        }
                    }
                }
            }
            imagen.UnlockBits(imagen2);
        }
        int ColorIndex = 0;
        protected  Color valorColor(double cReal, double cImag)
        {
            double RealZ = 0;
            double ImaginaryZ = 0;
            double RealZ2 = 0;
            double ImaginaryZ2 = 0;
            
            while ((ColorIndex < iteraciones) && (RealZ2 + ImaginaryZ2 < max))
            {
                RealZ2 = RealZ * RealZ;
                ImaginaryZ2 = ImaginaryZ * ImaginaryZ;
                ImaginaryZ = 2 * ImaginaryZ * RealZ + cImag;
                RealZ = RealZ2 - ImaginaryZ2 + cReal;
                ColorIndex++;
            }
            if (ColorIndex < iteraciones)
                return (Color.FromArgb(arregloColores[ColorIndex % arregloColores.Length]));
            else return Color.FromArgb(Color.Black.ToArgb());
        }
        protected void Draw()
        {

            Bitmap bm = new Bitmap(ClientSize.Width, ClientSize.Height);
            double minReal = -2.2, delReal = 0.009, minImag = -2.2, delImag = 0.009;


   
            for (int k = 0;k < bm.Width; k++)
            {
                double RealC = minReal + delReal * (k);

                for (int y = 0;y < bm.Height; y++)
                {
                    double ImagC = minImag + delImag * y;
                    bm.SetPixel(k, y, valorColor(RealC, ImagC));
                    buffer.cache[y, k] = ColorIndex;
                    ColorIndex = 0;
                }
            }
            
            imagen = bm;
            this.BackgroundImage = (Image)imagen;
            //this.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            animacion(++buffer.offset);
            this.Invalidate();
        }

        private void startAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startAnimationToolStripMenuItem.Enabled = false;
            stopAnimationToolStripMenuItem.Enabled = true;
            timer1.Start();
           
        }

        private void stopAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopAnimationToolStripMenuItem.Enabled = false;
            startAnimationToolStripMenuItem.Enabled = true;
            timer1.Stop();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
