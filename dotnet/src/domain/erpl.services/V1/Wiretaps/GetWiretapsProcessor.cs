using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Wiretaps;
using erpl.model.Wiretaps;
using MediatR;

namespace erpl.services.V1.Wiretaps;

public class GetWiretapsProcessor : IGetWiretapsProcessor, IRequestHandler<GetWiretapsQuery, BusinessResult<PagedList<WiretapDto>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IWiretapRepository _wiretapRepository;

    public GetWiretapsProcessor(IWiretapRepository wiretapRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _wiretapRepository = wiretapRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<WiretapDto>>> Handle(GetWiretapsQuery qry, CancellationToken cancellationToken)
    {
        return await GetWiretapsAsync(qry);
    }

    public async Task<BusinessResult<PagedList<WiretapDto>>> GetWiretapsAsync(GetWiretapsQuery qry)
    {
        var collectionBeforePaging =
            await _wiretapRepository.FindWiretapsPagedOf(qry.PageIndex, qry.PageSize);
            
        collectionBeforePaging.ApplySort(qry.OrderBy + " " + qry.SortDirection,
            _propertyMappingService.GetPropertyMapping<WiretapDto, Wiretap>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<Wiretap>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        var afterPaging = PagedList<Wiretap>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<WiretapDto>>(afterPaging);

        var result = new PagedList<WiretapDto>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<WiretapDto>>(result);

        return await Task.FromResult(bc);
    }
}