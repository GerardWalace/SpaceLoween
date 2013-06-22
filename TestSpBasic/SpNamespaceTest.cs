using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTest;
using System.IO;
using System.Reflection;

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
                "SampleSpItems.xml");

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
    }
}
