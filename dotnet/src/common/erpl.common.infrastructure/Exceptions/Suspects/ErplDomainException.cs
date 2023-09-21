using System;

namespace erpl.common.infrastructure.Exceptions.Suspects;

/// <summary>
/// Exception type for domain exceptions
/// </summary>
public class ErplDomainException : Exception {
    public ErplDomainException() { }

    public ErplDomainException(string message)
        : base(message) { }

    public ErplDomainException(string message, Exception innerException)
        : base(message, innerException) { }
    
}//Class : ErplDomainException