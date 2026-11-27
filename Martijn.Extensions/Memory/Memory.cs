using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Martijn.Extensions.Memory
{
    public class Memory : AsyncMemory
    {
        public readonly string BaseUrl;

        public string Application { get; set; }

        public Assembly Assembly { get; set; }

        public bool UseAssemblyLocation { get; set; }

        public bool CreateDirectoryIfNotExists { get; set; }
        private string GeneratedApplicationName => Application ?? Assembly.GetName().Name;

        public string ApplicationDataPath
        {
            get
            {
                if(BaseUrl != null) {
                    return BaseUrl;
                }

                if(UseAssemblyLocation) {
                    return Path.GetDirectoryName(Assembly.Location) + Path.DirectorySeparatorChar;
                } else {
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +
                                                 GeneratedApplicationName + Path.DirectorySeparatorChar;
                }
            }
        }


        public Memory() { }

        public Memory(string BaseUrl)
        {
            this.BaseUrl = BaseUrl;
            this.Assembly = System.Reflection.Assembly.GetEntryAssembly();
        }

        public override async Task<string> Read(string filename)
        {
            string filePath = ApplicationDataPath + filename;

            string fileDirectoryPath = Path.GetDirectoryName(filePath);

            if (CreateDirectoryIfNotExists && !Directory.Exists(fileDirectoryPath))
            {
                Directory.CreateDirectory(fileDirectoryPath);
            }

            var reader = File.OpenText(filePath);
            string res = await reader.ReadToEndAsync();
            reader.Close();
            return res;
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

        public static T Synchronously<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
