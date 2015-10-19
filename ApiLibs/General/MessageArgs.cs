using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiLibs
{
    public class MessageArgs
    {
        private readonly string _message;
        private bool _isImportant;

        String Message
        {
            get
            {
                return _message;
            }
        }

        Boolean isImportant
        {
            get { return _isImportant; }
        }

        public MessageArgs(string message)
        {
            _message = message;
        }

        public MessageArgs(string message, bool isImportant)
        {
            _message = message;
            _isImportant = isImportant;
        }


    }
}
