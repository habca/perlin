using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Numerics;

using ProceduralNoises;

namespace ProceduralNoisesTest
{
    [TestClass]
    public class ValueNoiseTest
    {
        private static double eps = 1E-5;

        /// <summary>
        /// Kohinan pitäisi olla nolla hyperkuution kärkipisteissä. 
        /// Testi osoittaa, että arvokohinan arvot lasketaan oikein
        /// hyperkuution kärkipisteissä.
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
                Vector4 quilez = ValueNoiseQuilez.noise(vec);
                Vector4 bourke = ValueNoiseBourke.noise(vec);

                Assert.AreEqual(0, perlin, eps);
                Assert.AreEqual(0, bourke.X, eps);
                Assert.AreEqual(0, quilez.X, eps);
            }            
        }

        /// <summary>
        /// Arvokohinoiden pitäisi vastata toisiaan.
        /// Testi osoittaa, että menetelmät ovat 
        /// keskenään vaihdettavissa.
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
                Vector4 bourke = ValueNoiseBourke.noise(vec);
                Vector4 quilez = ValueNoiseQuilez.noise(vec);

                Assert.AreEqual(perlin, bourke.X, eps);
                Assert.AreEqual(perlin, quilez.X, eps);

                Assert.AreEqual(quilez.X, bourke.X, eps);
                Assert.AreEqual(quilez.Y, bourke.Y, eps);
                Assert.AreEqual(quilez.Z, bourke.Z, eps);
                Assert.AreEqual(quilez.W, bourke.W, eps);
            }
        }
    }
}
