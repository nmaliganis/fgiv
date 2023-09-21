using System;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.contracts.V1.Suspects;
using MediatR;
using Serilog;

namespace erpl.services.V1.Suspects;

public class CreateSuspectProcessor : ICreateSuspectProcessor, IRequestHandler<CreateSuspectCommand, BusinessResult<SuspectDto>>
{
    public Task<BusinessResult<SuspectDto>> CreateSuspectAsync(CreateSuspectCommand createCommand)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessResult<SuspectDto>> Handle(CreateSuspectCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}//Class : CreateSuspectProcessor