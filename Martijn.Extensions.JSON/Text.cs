using System;
using System.Text;
using Martijn.Extensions.JSON.Compat;

namespace Martijn.Extensions.JSON.Text
{
    public static class TextExtensions
    {
        public static void Print(this object obj)
        {
            Console.WriteLine(NewtonsoftCompat.Serialize(obj, indented: true));
        }

        public static string Repeat(this string text, int amount) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                builder.Append(text);
            }

            return builder.ToString();
        }
    }
}