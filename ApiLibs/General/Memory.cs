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
        public readonly string DirectoryPath;

        public Memory(string baseUrl)
        {
            DirectoryPath = baseUrl;
        }

        public T ReadFile<T>(string filename) where T : new()
        {
            string text = ReadFile(filename);

            if (text != "")
            {
                T res = JsonConvert.DeserializeObject<T>(text);
                return res;
            }
            else
            {
                T res = new T();
                WriteFile(filename, res);
                return res;
            }
        }

        public string ReadFile(string filename)
        {
            string filePath = DirectoryPath + filename;

            string FileDirectoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(FileDirectoryPath))
            {
                Directory.CreateDirectory(FileDirectoryPath);
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
            var s = obj as string;
            File.WriteAllText(DirectoryPath + v, s ?? JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
