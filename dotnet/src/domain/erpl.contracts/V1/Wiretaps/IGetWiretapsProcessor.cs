using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Paging;

namespace erpl.contracts.V1.Wiretaps;

public interface IGetWiretapsProcessor
{
    Task<BusinessResult<PagedList<WiretapDto>>> GetWiretapsAsync(GetWiretapsQuery qry);
}