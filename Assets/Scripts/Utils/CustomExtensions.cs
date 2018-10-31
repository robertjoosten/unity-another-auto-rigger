using System;

namespace AnotherAutoRigger
{
    public static class StringExtension
    {
        public static bool IsDigit(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}