using System;

namespace erpl.common.infrastructure.Exceptions.Suspects;

public class SuspectDoesNotExistException : Exception
{
    public string SuspectId { get; }

    public SuspectDoesNotExistException(string suspectId)
    {
        this.SuspectId = suspectId;
    }

    public override string Message => $"Suspect with Id: {SuspectId}  doesn't exists!";
}