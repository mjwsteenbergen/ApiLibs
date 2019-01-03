﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public class Memory
    {
        public readonly string BaseUrl;

        public string Application { get; set; }
        private string GeneratedApplicationName => Application ?? GetType().Assembly.GetName().Name;

        public string ApplicationDataPath => BaseUrl ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +
                                             GeneratedApplicationName + Path.DirectorySeparatorChar;

        public Memory() { }

        public Memory(string BaseUrl)
        {
            this.BaseUrl = BaseUrl;
        }

        public T ReadFile<T>(string filename)
        {
            string text = ReadFile(filename);

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

        public T ReadFile<T>(string filename, T @default)
        {
            try
            {
                return ReadFile<T>(filename);
            }
            catch (FileNotFoundException)
            {
                WriteFile(filename, @default);
                return @default;
            }
        }

        public T ReadFileWithDefault<T>(string filename)
        {
            try
            {
                return ReadFile<T>(filename);
            }
            catch (FileNotFoundException)
            {
                var constr = typeof(T).GetConstructor(new Type[] { });
                T res = default(T);
                if (constr != null)
                {
                    res = (T)constr.Invoke(new object[] { });
                }
                WriteFile(filename, res);
                return res;
            }
        }



        public string ReadFile(string filename)
        {
            string filePath = ApplicationDataPath + filename;

            string fileDirectoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(fileDirectoryPath))
            {
                Directory.CreateDirectory(fileDirectoryPath);
            }

            FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            string text;
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
                reader.Close();
            }
            stream.Close();
            return text;
        }

        public void WriteFile(string v, object obj)
        {
            string str = null;
            if(obj is string)
            {
                str = obj as string;
            }

            File.WriteAllText(ApplicationDataPath + v, str ?? JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
