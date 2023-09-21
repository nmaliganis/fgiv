using System;

namespace erpl.common.infrastructure.Exceptions.Suspects
{
    public class MultipleSuspectWhereFoundException : Exception
    {
        public string Detail { get; }

        public MultipleSuspectWhereFoundException(string detail)
        {
            this.Detail = detail;
        }

        public override string Message => $" Multiple Suspect with :{Detail} found.";
    }
}