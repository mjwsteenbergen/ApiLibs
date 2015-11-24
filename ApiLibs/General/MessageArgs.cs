using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiLibs
{
    public class MessageArgs
    {
        private string Message { get; }

        private bool isImportant { get; }

        public MessageArgs(string message)
        {
            Message = message;
        }

        public MessageArgs(string message, bool isImportant)
        {
            Message = message;
            this.isImportant = isImportant;
        }


    }
}
