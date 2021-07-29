using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

using Perlin;
using Simplex;
using Value;

namespace ImageDrawer
{
    public class ImageDrawer
    {
        static void Main(string[] args)
        {
            Run();
            //NoiseGradients.Run();
        }

        private static void Run()
        {
            ImageDrawer drawer = new ImageDrawer(512, 512);
            float[] data = new float[512*512];

            data = drawer.generate(FractalNoise.fbm, ValueNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-bourke-fbm.png");

            data = drawer.generate(FractalNoise.nms, ValueNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-bourke-nms.png");

            data = drawer.generate(FractalNoise.fbm, ValueNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-quilez-fbm.png");

            data = drawer.generate(FractalNoise.nms, ValueNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-quilez-nms.png");

            data = drawer.generate(FractalNoise.fbm, ImprovedNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-gustavson-fbm.png");

            data = drawer.generate(FractalNoise.nms, ImprovedNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-gustavson-nms.png");

            data = drawer.generate(FractalNoise.fbm, SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "simplex-qustavson-fbm.png");

            data = drawer.generate(FractalNoise.nms, SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "simplex-qustavson-nms.png");

            data = drawer.generate(FractalNoise.nms, ImprovedNoise.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, "perlin-custom-nms.png");
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