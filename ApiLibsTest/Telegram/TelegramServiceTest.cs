using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Telegram;
using Martijn.Extensions.Memory;
using NUnit.Framework;

namespace ApiLibsTest.Telegram
{
    class TelegramServiceTest
    {
        private TelegramService Telegram;
        

        [OneTimeSetUp]
        public void GetTodoistService()
        {
            Passwords passwords = Passwords.ReadPasswords(new Memory());
            Telegram = new TelegramService(passwords.Telegram_token,"");
        }

        [Test]
        public async Task SendTest()
        {
            await Telegram.SendMessage(13173126, "*ZeusAction NightLightAction has had 4 fatal errors in a row. It has now been automatically blocked until Tuesday%2C April 16%2C 2019 9:59 PM.* %0AIts message: Could not load type 'ApiLibs.General.RequestException`1' from assembly 'ApiLibs%2C Version%3D1.1.0.0%2C Culture%3Dneutral%2C PublicKeyToken%3Dnull'..", ParseMode.Markdown);
        }
    }
}
