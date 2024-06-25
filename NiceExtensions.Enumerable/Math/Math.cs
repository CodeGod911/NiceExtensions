using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceExtensions.Enumerable.Math
{
    public static class MathExtensions
    {
        public static decimal Median(this IEnumerable<decimal> ll)
        {
            var l = ll.ToList();
            l.Sort();
            decimal median;
            if (l.Count % 2 == 0)
            {
                median = l[(l.Count - 1) / 2];
                median += l[(l.Count - 1) / 2 + 1];
                median /= 2;
            }
            else
            {
                median = l[(l.Count - 1) / 2];
            }
            return median;
        }

        public static IEnumerable<decimal> Modi(this IEnumerable<decimal> ll)
        {
            var l = ll.ToList();
            l.Sort();
            var max = l.Max(d => l.Where(dd => dd == d).Count());
            IEnumerable<decimal> mosts = l.Distinct().Where(d => l.Where(dd => dd == d).Count() == max);
            return mosts;
        }

        public static double Median(this IEnumerable<double> ll)
        {
            var l = ll.ToList();
            l.Sort();
            double median;
            if (l.Count % 2 == 0)
            {
                median = l[(l.Count - 1) / 2];
                median += l[(l.Count - 1) / 2 + 1];
                median /= 2;
            }
            else
            {
                median = l[(l.Count - 1) / 2];
            }
            return median;
        }

        public static IEnumerable<double> Modi(this IEnumerable<double> ll)
        {
            var l = ll.ToList();
            l.Sort();
            var max = l.Max(d => l.Where(dd => dd == d).Count());
            IEnumerable<double> mosts = l.Distinct().Where(d => l.Where(dd => dd == d).Count() == max);
            return mosts;
        }
    }
}
