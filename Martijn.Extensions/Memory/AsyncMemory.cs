using System.Threading.Tasks;
using System.IO;
using System;
using Newtonsoft.Json;

namespace Martijn.Extensions.Memory
{
    public abstract class AsyncMemory
    {

        public bool? WriteIndented { get; set; }

        public async Task<T> Read<T>(string filename)
        {
            string text = await Read(filename);

            if (text == "")
            {
                throw new FileNotFoundException();
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)text;
            }

            return JsonConvert.DeserializeObject<T>(text);
        }

        public async Task<T> Read<T>(string filename, T @default)
        {
            try
            {
                return await Read<T>(filename);
            }
            catch (FileNotFoundException)
            {
                await Write(filename, @default);
                return @default;
            }
        }

        public async Task<T> ReadWithDefault<T>(string filename)
        {
            try
            {
                return await Read<T>(filename);
            }
            catch (FileNotFoundException)
            {
                var constr = typeof(T).GetConstructor(new Type[] { });
                T res = default(T);
                if (constr != null)
                {
                    res = (T)constr.Invoke(new object[] { });
                }
                await Write(filename, res);
                return res;
            }
        }

        public async Task<T> ReadOrCalculate<T>(string filename, Func<T> func) 
        {
            try
            {
                return await Read<T>(filename);
            }
            catch (FileNotFoundException)
            {
                T calculatedValue = func();
                await Write(filename, calculatedValue);
                return calculatedValue;
            }
        }

        public async Task<T> ReadOrCalculate<T>(string filename, Func<Task<T>> func)
        {
            try
            {
                return await Read<T>(filename);
            }
            catch (FileNotFoundException)
            {
                T calculatedValue = await func();
                await Write(filename, calculatedValue);
                return calculatedValue;
            }
        }

        public abstract Task<string> Read(string filename);

        public Task Write(string filename, object? obj)
        {
            if (obj is string stringContent)
            {
                return WriteString(filename, stringContent);
            }

            return WriteString(filename, JsonConvert.SerializeObject(obj, (WriteIndented ?? true) ? Formatting.Indented : Formatting.None));
        }


        public abstract Task WriteString(string filePath, string text);
    }
}