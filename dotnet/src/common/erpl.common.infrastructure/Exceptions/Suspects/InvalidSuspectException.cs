using System;

namespace erpl.common.infrastructure.Exceptions.Suspects;

public class InvalidSuspectException : Exception
{
    public string BrokenRules { get; private set; }

    public InvalidSuspectException(string brokenRules)
    {
        BrokenRules = brokenRules;
    }
}