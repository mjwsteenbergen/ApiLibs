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
    [Explicit]
    public class PasswordTest 
    {
        [Test]
        public async Task GetPasswords()
        {
            var p = await Passwords.ReadPasswords();
            Assert.NotNull(p.Telegram_token);
        }
    }
}
