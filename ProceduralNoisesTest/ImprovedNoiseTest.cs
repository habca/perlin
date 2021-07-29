using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Numerics;

using ProceduralNoises;

namespace ProceduralNoisesTest
{
    [TestClass]
    public class ImprovedNoiseTest
    {
        private static double eps = 1E-5;

        /// <summary>
        /// Kohinan pitäisi olla nolla hyperkuution kärkipisteissä. 
        /// Testi osoittaa, että kohinafunktion arvot lasketaan 
        /// oikein hyperkuution kärkipisteissä.
        /// </summary>
        [TestMethod]
        public void NoiseEqualZeroLatticePointsTest()
        {
            Vector3[] test =
            {
                new(0,0,0),
                new(0,1,0),
                new(1,0,1),
                new(1,1,1),
            };

            foreach (Vector3 vec in test)
            {
                double perlin = ImprovedNoisePerlin.noise(vec);
                Vector4 custom = ImprovedNoise.noise(vec);
                Vector4 gustavson = ImprovedNoiseGustavson.noise(vec);

                Assert.AreEqual(0, perlin, eps);
                Assert.AreEqual(0, custom.X, eps);
                Assert.AreEqual(0, gustavson.X, eps);
            }            
        }

        /// <summary>
        /// Kohinan pitäisi vastata alkuperäistä toteutusta
        /// interpoloiduissa pisteissä. Testi osoittaa, että
        /// menetelmät ovat keskenään vaihdettavissa.
        /// </summary>
        [TestMethod]
        public void NoiseTest()
        {
            Vector3[] test =
            {
                new(-0.15f,-0.15f,-0.15f),
                new(-0.25f,0.25f,-0.25f),
                new(0.40f,-0.50f,0.60f),
                new(0.75f,0.75f,0.75f),
                new(-0.80f,0.80f,-0.80f),
                new(0.15f,-0.5f,0.80f),
                new(-0.5f,-0.75f,-0.20f),
                new(0.90f,0.10f,0.5f),
            };

            foreach (Vector3 vec in test)
            {   
                double perlin = ImprovedNoisePerlin.noise(vec);
                Vector4 custom = ImprovedNoise.noise(vec);
                Vector4 gustavson = ImprovedNoiseGustavson.noise(vec);

                Assert.AreEqual(perlin, custom.X, eps);
                Assert.AreEqual(perlin, gustavson.X, eps);
            }
        }

        /// <summary>
        /// Gradienttien pitäisi vastata jatkuvan kohinafunktion
        /// osittaisia derivaattoja hyperkuution kärkipisteissä, 
        /// joten testataan ovatko kärkipisteiden gradientit ja 
        /// kohinafunktion derivaatat samat. Testi osoittaa, että
        /// derivaatat lasketaan oikein gradienttien kohdalla.
        /// </summary>
        [TestMethod]
        public void NoiseDerivativesEqualGradientsTest()
        {
            Vector3[] test =
            {
                new(0,0,0),
                new(0,1,0),
                new(1,0,1),
                new(1,1,1),
            };

            foreach (Vector3 vec in test)
            {
                int x = (int)vec.X;
                int y = (int)vec.Y;
                int z = (int)vec.Z;

                int hash = ImprovedNoise.hash(x, y, z);

                Vector3 gradient = ImprovedNoise.grad3[hash];
                Vector4 gustavson = ImprovedNoiseGustavson.noise(vec);
                Vector4 custom = ImprovedNoise.noise(vec);
                
                Assert.AreEqual(gradient.X, gustavson.Y, eps);
                Assert.AreEqual(gradient.Y, gustavson.Z, eps);
                Assert.AreEqual(gradient.Z, gustavson.W, eps);

                Assert.AreEqual(gradient.X, custom.Y, eps);
                Assert.AreEqual(gradient.Y, custom.Z, eps);
                Assert.AreEqual(gradient.Z, custom.W, eps);
            }
        }

        /// <summary>
        /// Gradienttien pitäisi vastata jatkuvan kohinafunktion
        /// osittaisia derivaattoja hyperkuution kärkipisteissä, 
        /// joten testataan ovatko kärkipisteiden gradientit ja 
        /// kohinafunktion derivaatat samat. Testi osoittaa, että
        /// derivaatat ovat interpoloitavissa siinä missä arvotkin.
        /// </summary>
        [TestMethod]
        public void NoiseDerivativesInterpolatedFromGradientsTest()
        {
            Vector3[] test =
            {
                new(-0.15f,-0.15f,-0.15f),
                new(-0.25f,0.25f,-0.25f),
                new(0.40f,-0.50f,0.60f),
                new(0.75f,0.75f,0.75f),
                new(-0.80f,0.80f,-0.80f),
                new(0.15f,-0.5f,0.80f),
                new(-0.5f,-0.75f,-0.20f),
                new(0.90f,0.10f,0.5f),
            };

            foreach (Vector3 vec in test)
            {
                Vector4 custom = ImprovedNoise.noise(vec);
                Vector4 gustavson = ImprovedNoiseGustavson.noise(vec);
                
                Assert.AreEqual(gustavson.Y, custom.Y, eps);
                Assert.AreEqual(gustavson.Z, custom.Z, eps);
                Assert.AreEqual(gustavson.W, custom.W, eps);
            }
        }
    }
}
