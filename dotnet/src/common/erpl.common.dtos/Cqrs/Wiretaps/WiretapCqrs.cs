using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.dtos.ResourceParameters.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Paging;
using MediatR;

namespace erpl.common.dtos.Cqrs.Wiretaps;

// Queries
public record GetWiretapByIdQuery(string Id) : IRequest<BusinessResult<WiretapDto>>;

public class GetWiretapsQuery : GetWiretapsResourceParameters, IRequest<BusinessResult<PagedList<WiretapDto>>> {
    public GetWiretapsQuery(GetWiretapsResourceParameters parameters) : base() {
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
public record CreateWiretapCommand(CreateWiretapResourceParameters WiretapForCreationParameters)
    : IRequest<BusinessResult<WiretapDto>>;

public record UpdateWiretapCommand(string WiretapIdToBeModified, UpdateWiretapResourceParameters WiretapForModificationParameters)
    : IRequest<BusinessResult<WiretapDto>>;

public record DeleteWiretapCommand(string WiretapIdToBeDeleted)
    : IRequest<BusinessResult<WiretapDto>>;