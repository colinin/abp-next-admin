using System;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    public class TestSendEmailException : Exception, IHasNotifierErrorMessage
    {
        public TestSendEmailException(string message) 
            : base(message)
        {

        }

        public TestSendEmailException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }
    }
}
