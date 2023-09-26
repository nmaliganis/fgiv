using System;

namespace erpl.common.infrastructure.Exceptions.Wiretaps
{
    public class MultipleWiretapWhereFoundException : Exception
    {
        public string Detail { get; }

        public MultipleWiretapWhereFoundException(string detail)
        {
            this.Detail = detail;
        }

        public override string Message => $" Multiple Wiretap with :{Detail} found.";
    }
}