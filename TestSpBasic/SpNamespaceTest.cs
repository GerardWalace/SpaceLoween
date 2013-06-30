using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTest;
using System.IO;
using System.Reflection;
using SpaceTest.SpFrwk;
using SpaceTest.SpFrwk.Tools;

namespace TestSpBasic
{
    [TestClass]
    public class SpNamespaceTest
    {
        private SpObject CreateInstance()
        {
            SpObject obj = new SpObject();
            return obj;
        }

        [TestMethod]
        public void TestInstanciation()
        {
            SpObject obj = CreateInstance();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestSerialization()
        {
            SpObject obj = CreateInstance();

            List<SpObject> objs = new List<SpObject>()
            {
                obj
            };

            using (Stream stream = File.Open("SpaceObjectXml.txt", FileMode.Create))
            {
                var xmlformatter = new SpaceSerializer<List<SpObject>>();
                xmlformatter.Serialize(stream, objs);
            }

            List<SpObject> objs2;
            using (Stream stream = File.Open("SpaceObjectXml.txt", FileMode.Open))
            {
                var xmlformatter = new SpaceSerializer<List<SpObject>>();
                objs2 = xmlformatter.Deserialize(stream);
            }

            SpObject obj2 = objs2.First();

            Assert.IsNotNull(obj2);
            Assert.IsTrue(obj2.Equals(obj));
        }

        [TestMethod]
        public void TestDeSerialization()
        {
            String filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestDeSerialization.xml");

            List<SpItem> objs;
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var xmlformatter = new SpaceSerializer<List<SpItem>>();
                objs = xmlformatter.Deserialize(stream);
            }

            SpItem obj = objs.First();

            Assert.IsNotNull(obj);

            List<SpItem> objs2;
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var xmlformatter = new SpaceSerializer<List<SpItem>>();
                objs2 = xmlformatter.Deserialize(stream);
            }

            SpItem obj2 = objs2.First();

            Assert.IsNotNull(obj2);
            Assert.IsTrue(obj2.Equals(obj));
        }

        [TestMethod]
        public void TestAlmostEquals()
        {
            Double x = 5;
            Assert.IsTrue(x.AlmostEquals(4, 1.5));
            Assert.IsTrue(x.AlmostEquals(6, 1.5));
            Assert.IsFalse(x.AlmostEquals(3, 1.5));
            Assert.IsFalse(x.AlmostEquals(7, 1.5));

            SpVector v = new SpVector(100, 200, 300);
            Assert.IsTrue(v.AlmostEquals(new SpVector(110, 220, 330), v * 0.1));
            Assert.IsTrue(v.AlmostEquals(new SpVector(90, 180, 270), v * 0.1));
            Assert.IsFalse(v.AlmostEquals(new SpVector(110, 220, 330), v * 0.05));
            Assert.IsFalse(v.AlmostEquals(new SpVector(90, 180, 270), v * 0.05));

            v = new SpVector(100, 0, 0);
            Assert.IsTrue(v.AlmostEquals(new SpVector(100, 10, 0), v * 0.1));
            Assert.IsTrue(v.AlmostEquals(new SpVector(105, 5, 0), v * 0.1));
            Assert.IsFalse(v.AlmostEquals(new SpVector(100, 10, 0), v * 0.05));
            Assert.IsFalse(v.AlmostEquals(new SpVector(105, 5, 0), v * 0.05));
        }

        [TestMethod]
        public void TestOrbiteMercureSoleil()
        {
            String filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestOrbiteMercureSoleil.xml");

            List<SpItem> objs;
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var xmlformatter = new SpaceSerializer<List<SpItem>>();
                objs = xmlformatter.Deserialize(stream);
            }

            SpItem soleil = objs.FirstOrDefault(i => i.Name == "Soleil");
            SpItem mercure = objs.FirstOrDefault(i => i.Name == "Mercure");

            Assert.IsNotNull(soleil, "Soleil item is missing from input file");
            Assert.IsNotNull(mercure, "Mercure item is missing from input file");

            Double ratioCibleApprox = 0.05;
            Double ratioCibleApproxFinal = 0.00002;
            Double periodeRevolution = 87.96934 /*jours*/;

            SpVector initialPosition = mercure.P;
            SpVector initialVitesse = mercure.S;
            SpVector initialPositionSoleil = soleil.P;
            SpVector initialVitesseSoleil = soleil.S;
            SpVector approximation = initialPosition * ratioCibleApprox;

            // On met le Soleil et Mercure dans un univers
            SpUnivers univers = new SpUnivers();
            univers.AddItem(soleil);
            univers.AddItem(mercure);
            // On fait tourner pendant 88/4 jours
            for (int i = 0; i < periodeRevolution * 24 * 3600 / 4; i++)
                univers.Run();
            // On verifie que Mercure a bougé et toujours à une distance acceptable du Soleil
            Assert.IsFalse(initialPosition.AlmostEquals(mercure.P, approximation), "Mercure ne semble pas avoir bouge");
            Assert.IsTrue(initialPosition.Length2.AlmostEquals(mercure.P.Length2, approximation.Length2), "Mercure n'est plus a une distance acceptable du Soleil");
            // On fait tourner pendant 88/4 jours
            for (int i = 0; i < periodeRevolution * 24 * 3600 / 4; i++)
                univers.Run();
            // On verifie que Mercure est de l'autre coté du Soleil et toujours à une distance acceptable du Soleil
            Assert.IsTrue(initialPosition.AlmostEquals(-mercure.P, approximation), "Mercure ne tourne pas autour du Soleil");
            Assert.IsTrue(initialPosition.Length2.AlmostEquals(mercure.P.Length2, approximation.Length2), "Mercure n'est plus a une distance acceptable du Soleil");
            // On fait tourner pendant 88/2 jours
            for (int i = 0; i < periodeRevolution * 24 * 3600 / 2; i++)
                univers.Run();
            // On verifie que Mercure est revenue a son point de depart
            Assert.IsTrue(initialPosition.AlmostEquals(mercure.P, approximation), "Mercure n'est pas revenu a son point de depart");
            Assert.IsTrue(initialPosition.Length2.AlmostEquals(mercure.P.Length2, approximation.Length2), "Mercure n'est plus a une distance acceptable du Soleil");

            // On verifie que Mercure et le Soleil sont revenus dans une situation a peu pres identique a l'initiale
            Assert.IsTrue(initialPosition.AlmostRatio(mercure.P - soleil.P) < ratioCibleApproxFinal, "Le systeme n'est pas revenue a une position stable");
            Assert.IsTrue(initialVitesse.AlmostRatio(mercure.S - soleil.S) < ratioCibleApproxFinal, "Le systeme n'est pas revenue a une vitesse stable");
        }
    }
}
