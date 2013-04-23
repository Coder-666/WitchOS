using System;

namespace WitchOS.Core
{
    public static class Math
    {
        public const double E = 2.7182818284590451;
        public const double PI = 3.1415926535897931;

        public static uint Pow(this int inp, uint power)
        {
            if (inp == 0) return 0;
            int output = inp;
            for (uint i = 0; i < power; i++)
            {
                output *= inp;
            }
            if (output < 0) output *= -1;
            return (uint)output;
        }

        public static uint Factorial(this int inp)
        {
            if (inp == 0) return 1;
            uint output = 1;
            for (uint i = 0; i < inp; i++)
            {
                output *= i;
            }
            return output;
        }

        public static bool IsPositive(this int inp)
        {
            if (inp > 0) return true;
            return false;
        }

        public static int DivRem(int a, int b, out int result)
        {
            result = a % b;
            return (a / b);
        }
    }
}
