﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Exceptions
{
    [Serializable]
    public class AuthenticationException:Exception
    {
        public AuthenticationException()
        {

        }
        public AuthenticationException(string message) :base(message)
        {

        }
        public AuthenticationException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
