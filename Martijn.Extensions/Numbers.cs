using System;
using System.Collections.Generic;
using System.Linq;

namespace Martijn.Extensions.Numbers
{
    public static class LinqNumberExtensions
    {
        public static float Median(this IEnumerable<float> IEnumerable)
        {
            var list = IEnumerable.OrderBy(i => i).ToList();

            if (list.Count == 0)
            {
                throw new ArgumentException("There are no numbers");
            }

            double middle = (list.Count - 1) / 2.0;
            return (list[(int)Math.Ceiling(middle)] + list[(int)Math.Floor(middle)]) / 2.0f;
        }

        public static double Median(this IEnumerable<double> IEnumerable)
        {
            var list = IEnumerable.OrderBy(i => i).ToList();

            if (list.Count == 0)
            {
                throw new ArgumentException("There are no numbers");
            }

            double middle = (list.Count - 1) / 2.0;
            return (list[(int)Math.Ceiling(middle)] + list[(int)Math.Floor(middle)]) / 2.0;
        }

        public static int Median(this IEnumerable<int> IEnumerable)
        {
            var list = IEnumerable.OrderBy(i => i).ToList();

            if (list.Count == 0)
            {
                throw new ArgumentException("There are no numbers");
            }

            double middle = (list.Count - 1) / 2.0;
            return (list[(int)Math.Ceiling(middle)] + list[(int)Math.Floor(middle)]) / 2;
        }

        public static IEnumerable<int> Until(this int start, int end, int steps = 1)
        {
            if(end < start)
            {
                throw new ArgumentException("End is smaller than current number");
            }

            int last = start;
            while(last <= end)
            {
                yield return last;
                last += steps;
            }
        }

        public static IEnumerable<double> Until(this double start, double end, double steps = 1)
        {
            if (end < start)
            {
                throw new ArgumentException("End is smaller than current number");
            }

            double last = start;
            while (last <= end)
            {
                yield return last;
                last += steps;
            }
        }

        public static IEnumerable<float> Until(this float start, float end, float steps = 1)
        {
            if (end < start)
            {
                throw new ArgumentException("End is smaller than current number");
            }

            float last = start;
            while (last <= end)
            {
                yield return last;
                last += steps;
            }
        }
    }
}