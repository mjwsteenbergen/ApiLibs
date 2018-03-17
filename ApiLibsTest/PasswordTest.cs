using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ApiLibs.General;
using System.IO;

namespace ApiLibsTest
{
    public class PasswordTest 
    {
        public static readonly string GlobalAppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +
            "Laurentia" + Path.DirectorySeparatorChar;

        [Test]
        public void GetPasswords()
        {
            var p = Passwords.ReadPasswords(GlobalAppDataPath);
            Assert.NotNull(p.Telegram_token);
        }
    }
}
