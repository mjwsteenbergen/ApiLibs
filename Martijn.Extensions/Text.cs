using System;
using System.Text;
using Newtonsoft.Json;

namespace Martijn.Extensions.Text
{
    public static class TextExtensions
    {
        public static void Print(this object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
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