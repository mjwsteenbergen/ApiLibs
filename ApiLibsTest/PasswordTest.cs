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
        [Test]
        public void GetPasswords()
        {
            var p = Passwords.ReadPasswords();
            Assert.NotNull(p.Telegram_token);
        }
    }
}
