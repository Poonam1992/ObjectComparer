using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace HelloWorldTests
    {
        [TestClass]
        public class ObjectComparerUnitTest
        {
            
            [TestMethod]
            public void TestCompareObjectsNegative()
            {
                int firstValue = 10;
                int secondValue = 20;
                ObjectComparer ojectComparer = new ObjectComparer();

                var result = ojectComparer.CompareObjects(firstValue, secondValue);
                Assert.AreEqual(false, result);
            }


            [TestMethod]
            public void TestCompareObjectsPositive()
            {
                int firstValue = 10;
                int secondValue = 10;
                ObjectComparer ojectComparer = new ObjectComparer();

                var result = ojectComparer.CompareObjects(firstValue, secondValue);
                Assert.AreEqual(true, result);
            }

            [TestMethod]
            public void TestCompareObjectsPositive()
            {
                Student student1 = new Student
                {
                    FirstName = "Pham",
                    LastName = "Hoang",
                    Age = 15,
                    BirthDate = new DateTime(1992, 12, 5),
                    arrayList = new Int32[4] { 1, 2, 3, 4 },

                };

                Student student2 = new Student
                {
                    Age = 15,
                    FirstName = "Pham",
                    LastName = "Hoang",

                    BirthDate = new DateTime(1992, 12, 5),
                    arrayList = new Int32[4] { 4, 3, 2, 1 },

                };
                ObjectComparer ojectComparer = new ObjectComparer();

                var result = ojectComparer.CompareObjects(student1, student2);
                Assert.AreEqual(true, result);
            }
        }
    }