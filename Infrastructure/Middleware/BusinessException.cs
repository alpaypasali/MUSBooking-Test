using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.NewFolder
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

}
