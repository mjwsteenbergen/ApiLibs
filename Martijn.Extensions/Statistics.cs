using System;
using System.Collections.Generic;
using System.Linq;

namespace Martijn.Extensions.Statistics
{
    public static class StatisticsExtensions
    {
        public static double StandardDeviation(this IEnumerable<int> enumerable)
        {
            return enumerable.Cast<double>().StandardDeviation();
        }

        public static double StandardDeviation(this IEnumerable<double> enumerable)
        {
            var list = enumerable.ToList();
            var mean = list.Average();
            var summed = list.Select(i => (i - mean) * (i - mean)).Sum();

            return Math.Sqrt((1.0 / (list.Count - 1)) * summed);
        }

        public static (double min, double max) ConfidenceInterval(this IEnumerable<double> enumerable, double confidence = 0.95)
        {
            var t = confidence switch {
                0.9 => 1.282,
                0.95 => 1.645,
                0.99 => 2.326,
                0.995 => 2.576,
                0.9975 => 2.807,
                0.999 => 3.090,
                0.9995 => 3.291,
                _ => throw new ArgumentException("Unknown confidence")
            };
            var list = enumerable.ToList();
            var avg = list.Average();
            var std = list.StandardDeviation();
            var n = Math.Sqrt(list.Count);
            var right = (t*std)/n;
            return (avg - right, avg + right);
        }
    }
}