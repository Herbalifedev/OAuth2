using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HL.OAuth2.Models
{
    public class ErrorResponse : BaseResponse
    {
        public Error error;

        public class Error
        {
            public string code;
            public string message;
        }
    }
}
