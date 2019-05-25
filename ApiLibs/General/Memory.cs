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

        public async override Task<string> Read(string filename)
        {
            string filePath = ApplicationDataPath + filename;

            string fileDirectoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(fileDirectoryPath))
            {
                Directory.CreateDirectory(fileDirectoryPath);
            }

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }

        public override Task WriteString(string filename, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(ApplicationDataPath + filename,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                return sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        public static T Synchronously<T>(Task<T> task) {
            task.Wait();
            return task.Result;
        }
    }
}
