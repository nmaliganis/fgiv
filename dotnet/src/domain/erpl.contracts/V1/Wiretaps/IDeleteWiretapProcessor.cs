using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;

namespace erpl.contracts.V1.Wiretaps;

public interface IDeleteWiretapProcessor
{
    Task<BusinessResult<WiretapDto>> DeleteWiretapAsync(DeleteWiretapCommand deleteCommand);
}