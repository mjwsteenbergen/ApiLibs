using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiLibs.General
{
    public abstract class AsyncMemory
    {

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

        public abstract Task<string> Read(string filename);

        public Task Write(string filename, object obj) 
        {
            if (obj is string)
            {
                return WriteString(filename, obj as string);
            }

            return WriteString(filename, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public abstract Task WriteString(string filePath, string text);
    }
}