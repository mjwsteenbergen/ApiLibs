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

        private string ApplicationName => AppDomain.CurrentDomain.FriendlyName;

        public string ApplicationDataPath => DirectoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +
                                             ApplicationName + Path.DirectorySeparatorChar;

        public Memory() { }

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
            var s = obj as string;
            File.WriteAllText(ApplicationDataPath + v, s ?? JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
