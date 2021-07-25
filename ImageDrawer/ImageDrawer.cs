using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

using Perlin;
using Simplex;

namespace ImageDrawer
{
    public class ImageDrawer
    {
        static void Main(string[] args)
        {
            //test();
            run();
        }

        private static void test()
        {
            Console.WriteLine("Classic noise:");
            Vector3[] classic = TestPerlin();
            foreach (var grad in classic)
            {
                Console.WriteLine(grad);
            }

            Console.WriteLine("Simplex noise:");
            Vector3[] simplex = TestSimplex();
            foreach (var grad in simplex)
            {
                Console.WriteLine(grad);
            }
        }

        private static Vector3[] TestPerlin()
        {
            Vector3[] grads = new Vector3[16];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)ImprovedNoisePerlin.grad(h, 1, 0, 0);
                grads[h].Y = (float)ImprovedNoisePerlin.grad(h, 0, 1, 0);
                grads[h].Z = (float)ImprovedNoisePerlin.grad(h, 0, 0, 1);
            }
            return grads;
        }

        private static Vector3[] TestSimplex()
        {
            Vector3[] grads = new Vector3[64];
            for (int h = 0; h < grads.Length; h++)
            {
                grads[h].X = (float)NoiseHardware.grad2(h, 1, 0, 0);
                grads[h].Y = (float)NoiseHardware.grad2(h, 0, 1, 0);
                grads[h].Z = (float)NoiseHardware.grad2(h, 0, 0, 1);
            }
            return grads;
        }

        private static void run()
        {
            ImageDrawer drawer = new ImageDrawer(512, 512);
            float[] data = new float[512*512];

            data = drawer.generate(FractalNoise.fbm, ImprovedNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-bourke-fbm.png");

            data = drawer.generate(FractalNoise.nms, ImprovedNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-bourke-nms.png");

            data = drawer.generate(FractalNoise.fbm, ImprovedNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-quilez.fbm.png");

            data = drawer.generate(FractalNoise.nms, ImprovedNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-quilez-nms.png");

            data = drawer.generate(FractalNoise.fbm, Simplex.SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "simplex-qustavson-fbm.png");

            data = drawer.generate(FractalNoise.nms, Simplex.SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "simplex-qustavson-nms.png");
        }

        private int width;
        private int height;

        public ImageDrawer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public float[] generate(
            Func<Func<double, double, double, Vector4>, double, double, double, int, Vector4> g,
            Func<double, double, double, Vector4> f
        )
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            float[] data = new float[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector4 noise = g(f, i, j, 0, 4);
                    float value = (float)noise.X;
                    data[i*height + j] = value;
                }
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);

            return data;
        }

        public static float[] scale(float[] data, float newmin, float newmax)
        {
            float oldmin = float.MaxValue;
            float oldmax = float.MinValue;
            foreach (float val in data)
            {
                oldmin = val <= oldmin ? val : oldmin;
                oldmax = val >= oldmax ? val : oldmax;
            }

            float[] newdata = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newdata[i] = scale(data[i], oldmin, oldmax, newmin, newmax);
            }
            
            Console.WriteLine("min: "+oldmin+", max: "+oldmax);

            return newdata;
        }

        public static float scale(float val, float oldmin, float oldmax, float newmin, float newmax)
        {
            return (newmax - newmin) * ((val - oldmin) / (oldmax - oldmin)) + newmin;
        }

        public void draw(float[] data, string file)
        {

            Bitmap newBitmap = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int r = (int) data[i*height + j];
                    int g = (int) data[i*height + j];
                    int b = (int) data[i*height + j];

                    Color newColor = Color.FromArgb(r, g, b);
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            newBitmap.Save(file, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}