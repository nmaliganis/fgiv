using System;

namespace erpl.common.infrastructure.Exceptions.Suspects;

public class SuspectAlreadyExistsException : Exception
{
    public string Name { get; }
    public string BrokenRules { get; }

    public SuspectAlreadyExistsException(string name, string brokenRules)
    {
        Name = name;
        BrokenRules = brokenRules;
    }

    public override string Message => $" Suspect with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
}