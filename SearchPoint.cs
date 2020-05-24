using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace Tree
{
    public class SearchPoint<T>
    {
        public Person data;
        public SearchPoint<Person> left;
        public SearchPoint<Person> right;

        public SearchPoint (Person d)
        {
            data = d;
            left = null;
            right = null;

        }

        public SearchPoint()
        {
            data = default;
            left = null;
            right = null;
        }

        public override string ToString()
        {
            return data.ToString() + " ";
        }


        int CompareTo(Person other)
        {
            return data.CompareTo(other);
        }
    }
}
