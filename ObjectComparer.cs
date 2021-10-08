using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectComparerFunction
{
static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
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
            Console.WriteLine(result);

        }
    }

   

    /// <summary>
    /// Class to compare collection
    /// </summary>
    public class CollectionComparer : ObjectComparer
    {
        /// <summary>
        /// Compare objects
        /// </summary>
        /// <param name="firstObject"></param>
        /// <param name="secondObject"></param>
        /// <returns></returns>
        public  override bool CompareObjects(object firstObject, object secondObject)
        {
            IEnumerable<object> collectionItems1;
            IEnumerable<object> collectionItems2;
            int collectionItemsCount1;
            int collectionItemsCount2;

            // null check
            if (firstObject == null && secondObject != null || firstObject != null && secondObject == null)
            {
                return false;
            }
            else if (firstObject != null && secondObject != null)
            {
                collectionItems1 = ((IEnumerable)firstObject).Cast<object>();
                collectionItems2 = ((IEnumerable)secondObject).Cast<object>();
                collectionItemsCount1 = collectionItems1.Count();
                collectionItemsCount2 = collectionItems2.Count();

                // check the counts 
                if (collectionItemsCount1 != collectionItemsCount2)
                {
                    return false;
                }
                // and if they do, compare each item.
                else
                {
                    object collectionItem1 = collectionItems1.FirstOrDefault();
                    Type collectionItemType = collectionItem1.GetType();
                    if (collectionItemType.IsPrimitive || collectionItemType.IsValueType || typeof(IComparable).IsAssignableFrom(collectionItemType))
                    {
                        var set = new HashSet<object>(collectionItems1);
                        var equals = set.SetEquals(collectionItems2);

                        if (!equals)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < collectionItemsCount1; i++)
                    {
                        object collectionItem2;

                        collectionItem1 = collectionItems1.ElementAt(i);
                        collectionItem2 = collectionItems2.ElementAt(i);
                        if (!base.CompareObjects(collectionItem1, collectionItem2)) return false;
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// PRimitive type comparer
    /// </summary>
    public class PrimitiveTypeComparer : ObjectComparer
    {
        public override bool  CompareObjects(object firstObject, object secondObject)
        {
            if (firstObject == null && secondObject != null || firstObject != null && secondObject == null)
                return false; // one of the values is null

            else if (!object.Equals(firstObject, secondObject))
                return false;

            return true;
        }
       
    }

    /// <summary>
    /// Base class 
    /// </summary>
    public class ObjectComparer
    { 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstObject"></param>
        /// <param name="secondObject"></param>
        /// <returns></returns>
        public virtual bool CompareObjects(object firstObject, object secondObject)
        {
            ObjectComparer compare;
            if (firstObject != null && secondObject != null)
            {
                Type objectType = firstObject.GetType();
                if (!firstObject.GetType().Equals(secondObject.GetType()))
                {
                    return false;
                }

                var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object firstObjectValue;
                    object secondObjectValue;

                    firstObjectValue = propertyInfo.GetValue(firstObject);
                    secondObjectValue = propertyInfo.GetValue(secondObject);

                    var propertyType = propertyInfo.PropertyType;
                    //  primitive type, value type or implements IComparable, just direct compare values
                    if (propertyType.IsPrimitive || propertyType.IsValueType || typeof(IComparable).IsAssignableFrom(propertyType))
                    {
                        compare = new PrimitiveTypeComparer();
                        if(!compare.CompareObjects(firstObjectValue, secondObjectValue))
                        {
                            return false;
                        }
                    }
                    // if it implements IEnumerable, then scan any items
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        compare = new CollectionComparer();
                       if(! compare.CompareObjects(firstObjectValue, secondObjectValue))
                        {
                            return false;
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!CompareObjects(propertyInfo.GetValue(firstObject, null), propertyInfo.GetValue(secondObject, null))) return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
                return object.Equals(firstObject, secondObject);

            return true;
        }


    }

    /// <summary>
    /// Stuen class
    /// </summary>
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public DateTime BirthDate { get; set; }

        public int[] arrayList { get; set; }

    }
}