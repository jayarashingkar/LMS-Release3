using System;
using System.Text;

namespace UAC.LMS.Common.Utilities
{
    /// <summary>
    /// Random String to generate temporary password
    /// </summary>
    public static class Extensions
    {
        public static string RandomString(this int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
