using System;

namespace erpl.common.infrastructure.Exceptions.Wiretaps;

public class InvalidWiretapException : Exception
{
    public string BrokenRules { get; private set; }

    public InvalidWiretapException(string brokenRules)
    {
        BrokenRules = brokenRules;
    }
}