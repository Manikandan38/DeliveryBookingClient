using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.ExceptionHandler
{
    public class Exception : ApplicationException
    {
        
    }
    public class NullException : ApplicationException
    {
        public NullException(string Message) : base(Message)
        {

        }
    }

    public class SomethingWentWrongException : ApplicationException
    {
        public SomethingWentWrongException(string Message) : base(Message)
        {

        }
    }

    public class InvalidCredentialsException : ApplicationException
    {
        public InvalidCredentialsException(string Message) : base(Message)
        {

        }
    }

}
