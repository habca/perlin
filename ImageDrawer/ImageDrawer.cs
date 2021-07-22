﻿using System;
using System.Drawing;
using System.Numerics;

namespace ImageDrawer
{
    public class ImageDrawer
    {
        static void Main(string[] args)
        {
            ImageDrawer drawer = new ImageDrawer(512, 512);
            float[] data = new float[512*512];
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

            return newdata;
        }

        public static float scale(float val, float oldmin, float oldmax, float newmin, float newmax)
        {
            return (newmax - newmin) * ((val - oldmin) / (oldmax - oldmin)) + newmin;
        }

        #nullable enable
        public static Bitmap ReadImage(string filepath)
        {
            return new Bitmap(Image.FromFile(filepath));
        }

        public static void WriteBitmap(string filepath, Bitmap? bitmap)
        {
            // Huom! Tekee eri kuin mikä avattu!
            bitmap?.Save(filepath, System.Drawing.Imaging.ImageFormat.Tiff);
        }
        #nullable disable

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