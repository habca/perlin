using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Perlin;

namespace PerlinTest
{
    [TestClass]
    public class ImageDrawerTest
    {
        [TestMethod]
        public void ScaleTest()
        {
            float eps = 1E-5f;

            float[] testdata = new float[3] { -0.5f, 0f, 0.5f };
            float[] result1 = ImageDrawer.scale(testdata, -0.5f, 0.5f);
            float[] result2 = ImageDrawer.scale(testdata, -1f, 1f);
            float[] result3 = ImageDrawer.scale(testdata, 0, 1);
            float[] result4 = ImageDrawer.scale(testdata, 0, 255);

            Assert.AreEqual(-0.5f, result1[0], eps);
            Assert.AreEqual(0f, result1[1], eps);
            Assert.AreEqual(0.5f, result1[2], eps);

            Assert.AreEqual(-1f, result2[0], eps);
            Assert.AreEqual(0f, result2[1], eps);
            Assert.AreEqual(1f, result2[2], eps);

            Assert.AreEqual(0f, result3[0], eps);
            Assert.AreEqual(0.5f, result3[1], eps);
            Assert.AreEqual(1f, result3[2], eps);

            Assert.AreEqual(0f, result4[0], eps);
            Assert.AreEqual(255f, result4[2], eps);
        }
    }
}