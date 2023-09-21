using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Paging;

namespace erpl.contracts.V1.Suspects;

public interface IGetSuspectsProcessor
{
    Task<BusinessResult<PagedList<SuspectDto>>> GetSuspectsAsync(GetSuspectsQuery qry);
}