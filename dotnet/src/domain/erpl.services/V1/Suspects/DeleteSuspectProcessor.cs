using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.contracts.V1.Suspects;
using MediatR;

namespace erpl.services.V1.Suspects;

public class DeleteSuspectProcessor : IDeleteSuspectProcessor, IRequestHandler<DeleteSuspectCommand,
    BusinessResult<SuspectDto>>
{
    public Task<BusinessResult<SuspectDto>> DeleteSuspectAsync(DeleteSuspectCommand deleteCommand)
    {
        throw new System.NotImplementedException();
    }

    public Task<BusinessResult<SuspectDto>> Handle(DeleteSuspectCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
} //Class : DeleteHardSuspectProcessor