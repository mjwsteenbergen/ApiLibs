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

            if (text != "")
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)text;
                }

                return JsonConvert.DeserializeObject<T>(text);
            }
            else
            {
                T res = default(T);
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
