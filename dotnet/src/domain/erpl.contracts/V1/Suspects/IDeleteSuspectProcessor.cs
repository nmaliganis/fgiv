using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;

namespace erpl.contracts.V1.Suspects;

public interface IDeleteSuspectProcessor
{
    Task<BusinessResult<SuspectDto>> DeleteSuspectAsync(DeleteSuspectCommand deleteCommand);
}