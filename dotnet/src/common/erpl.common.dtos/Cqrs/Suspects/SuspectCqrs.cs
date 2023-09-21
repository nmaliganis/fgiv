using erpl.common.dtos.DTOs.Suspects;
using erpl.common.dtos.ResourceParameters.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Paging;
using MediatR;

namespace erpl.common.dtos.Cqrs.Suspects;

// Queries
public record GetSuspectByIdQuery(string Id) : IRequest<BusinessResult<SuspectDto>>;

public class GetSuspectsQuery : GetSuspectsResourceParameters, IRequest<BusinessResult<PagedList<SuspectDto>>> {
    public GetSuspectsQuery(GetSuspectsResourceParameters parameters) : base() {
        this.Filter = parameters.Filter;
        this.SearchQuery = parameters.SearchQuery;
        this.Fields = parameters.Fields;
        this.OrderBy = parameters.OrderBy;
        this.SortDirection = parameters.SortDirection;
        this.PageSize = parameters.PageSize;
        this.PageIndex = parameters.PageIndex;
    }
}

// Commands
public record CreateSuspectCommand(CreateSuspectResourceParameters SuspectForCreationParameters)
    : IRequest<BusinessResult<SuspectDto>>;

public record UpdateSuspectCommand(string SuspectIdToBeModified, UpdateSuspectResourceParameters SuspectForModificationParameters)
    : IRequest<BusinessResult<SuspectDto>>;

public record DeleteSuspectCommand(string SuspectIdToBeDeleted)
    : IRequest<BusinessResult<SuspectDto>>;