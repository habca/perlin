using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;

using ProceduralNoises;

using static ProceduralNoises.FractionalBrownianMotion;

namespace ImageDrawer
{
    public class ImageDrawer
    {
        private int width;
        private int height;

        public ImageDrawer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static float[] generate(Warp q, Fract h, Sumf g, Noise f, int width, int height)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            float k = 0.001f;
            float[] data = new float[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    data[i * height + j] = (float)q(h, g, f, k * i, k * j, 0, out Vector3 _);
                }
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);

            return data;
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

        static void Main(string[] args)
        {
            ImageDrawer drawer = new ImageDrawer(512, 256);
            float[] data = new float[512 * 256];

            string dir = "kuvat/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            /*
            data = drawer.generate(FractionalBrownianMotion.fbm, ValueNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "value-bourke-fbm.png");

            data = drawer.generate(FractionalBrownianMotion.nms, ValueNoiseBourke.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "value-bourke-nms.png");

            data = drawer.generate(FractionalBrownianMotion.fbm, ValueNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "value-quilez-fbm.png");

            data = drawer.generate(FractionalBrownianMotion.nms, ValueNoiseQuilez.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "value-quilez-nms.png");

            data = drawer.generate(FractionalBrownianMotion.fbm, ImprovedNoise.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "perlin-custom-fbm.png");

            data = drawer.generate(FractionalBrownianMotion.nms, ImprovedNoise.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "perlin-custom-nms.png");

            data = drawer.generate(FractionalBrownianMotion.fbm, ImprovedNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "perlin-gustavson-fbm.png");

            data = drawer.generate(FractionalBrownianMotion.nms, ImprovedNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "perlin-gustavson-nms.png");

            data = drawer.generate(FractionalBrownianMotion.fbm, SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "simplex-qustavson-fbm.png");

            data = drawer.generate(FractionalBrownianMotion.nms, SimplexNoiseGustavson.noise);
            data = scale(data, 0, 255);
            drawer.draw(data, dir + "simplex-qustavson-nms.png");
            */

            float[] arr = ImageDrawer.generate(RWarp, Dnoise, Billow, NoiseHardwareDerivatives.noise, 512, 256);
            arr = scale(arr, 0, 255);
            drawer.draw(arr, dir + "simplex-erosion.png");
        }

        /*
        private static void Test()
        {
            NoiseGradients.Run(NoiseGradients.GradientsPerlin);
            NoiseGradients.Run(NoiseGradients.GradientsPerlin3D);
            NoiseGradients.Run(NoiseGradients.GradientsPerlin4D);
            NoiseGradients.Run(NoiseGradients.GradientsSimplex);
        }
        */
    }
}
