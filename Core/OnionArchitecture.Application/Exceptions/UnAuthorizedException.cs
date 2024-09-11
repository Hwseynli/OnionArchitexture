using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Exceptions
{
    public class UnAuthorizedException:Exception
    {
        public UnAuthorizedException() { }
        public UnAuthorizedException(string message) : base(message) { }
        public UnAuthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
