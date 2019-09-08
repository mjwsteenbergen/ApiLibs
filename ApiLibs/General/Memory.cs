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
    public class Memory : AsyncMemory
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

        public override async Task<string> Read(string filename)
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

        public override async Task WriteString(string filename, string text)
        {
            var filePath = ApplicationDataPath + filename;
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteAsync(text);
            }
        }

        public static T Synchronously<T>(Task<T> task) {
            task.Wait();
            return task.Result;
        }
    }
}
