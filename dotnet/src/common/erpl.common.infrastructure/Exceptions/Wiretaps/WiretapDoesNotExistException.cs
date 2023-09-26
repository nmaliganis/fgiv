using System;

namespace erpl.common.infrastructure.Exceptions.Wiretaps;

public class WiretapDoesNotExistException : Exception
{
    public string WiretapId { get; }

    public WiretapDoesNotExistException(string wiretapId)
    {
        this.WiretapId = wiretapId;
    }

    public override string Message => $"Wiretap with Id: {WiretapId}  doesn't exists!";
}