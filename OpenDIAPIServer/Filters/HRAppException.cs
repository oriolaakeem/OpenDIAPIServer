using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDIAPIServer.Filters
{
    public class HRAppException : Exception
    {
        public HRAppException()
        { }

        public HRAppException(string message)
        : base(message)
    { }

        public HRAppException(string message, Exception innerException)
        : base(message, innerException)
    { }
    }
}
