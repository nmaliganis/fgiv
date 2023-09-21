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

public class UpdateSuspectProcessor : IUpdateSuspectProcessor, IRequestHandler<UpdateSuspectCommand, BusinessResult<SuspectModificationDto>>
{
    public Task<BusinessResult<SuspectModificationDto>> UpdateSuspectAsync(UpdateSuspectCommand updateCommand)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessResult<SuspectModificationDto>> Handle(UpdateSuspectCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}