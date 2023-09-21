using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;

namespace erpl.contracts.V1.Suspects;

public interface IUpdateSuspectProcessor
{
    Task<BusinessResult<SuspectModificationDto>> UpdateSuspectAsync(UpdateSuspectCommand updateCommand);
}