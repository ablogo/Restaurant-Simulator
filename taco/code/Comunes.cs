using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taco.code
{
    static class Comunes
    {
        static Random rn;

        public static int get_random(int min, int max)
        {
            try
            {
                rn = new Random();
                max = rn.Next(min, max);
            }
            catch (Exception e) { }
            return max;
        }

        public static int get_random(int min, int max, int seed)
        {
            try
            {
                rn = new Random(seed);
                max = rn.Next(min, max);
            }
            catch (Exception e) { }
            return max;
        }

        public static bool is_time(DateTime date_to_compare)
        {
            try
            {
                if (DateTime.Now.CompareTo(date_to_compare) >= 0) return true;
            }
            catch (Exception e) { }
            return false;
        }

        public static bool is_time(DateTime date_to_compare, int minutes)
        {
            bool res = false;
            try
            {
                if (date_to_compare.ToString("HHmm") == DateTime.Now.AddMinutes(minutes).ToString("HHmm")) res = true;
            }
            catch (Exception e) { }
            return res;
        }

    }
}
