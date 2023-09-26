using System;

namespace erpl.common.infrastructure.Exceptions.Wiretaps;

public class WiretapAlreadyExistsException : Exception
{
    public string Name { get; }
    public string BrokenRules { get; }

    public WiretapAlreadyExistsException(string name, string brokenRules)
    {
        Name = name;
        BrokenRules = brokenRules;
    }

    public override string Message => $" Wiretap with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
}