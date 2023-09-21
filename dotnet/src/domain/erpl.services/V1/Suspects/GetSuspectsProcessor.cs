using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Suspects;
using erpl.model.Suspects;
using MediatR;

namespace erpl.services.V1.Suspects;

public class GetSuspectsProcessor : IGetSuspectsProcessor, IRequestHandler<GetSuspectsQuery, BusinessResult<PagedList<SuspectDto>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ISuspectRepository _suspectRepository;

    public GetSuspectsProcessor(ISuspectRepository suspectRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _suspectRepository = suspectRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<SuspectDto>>> Handle(GetSuspectsQuery qry, CancellationToken cancellationToken)
    {
        return await GetSuspectsAsync(qry);
    }

    public async Task<BusinessResult<PagedList<SuspectDto>>> GetSuspectsAsync(GetSuspectsQuery qry)
    {
        var collectionBeforePaging =
            await _suspectRepository.FindSuspectsPagedOf(qry.PageIndex, qry.PageSize);
            
        collectionBeforePaging.ApplySort(qry.OrderBy + " " + qry.SortDirection,
            _propertyMappingService.GetPropertyMapping<SuspectDto, Suspect>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<Suspect>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        var afterPaging = PagedList<Suspect>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<SuspectDto>>(afterPaging);

        var result = new PagedList<SuspectDto>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<SuspectDto>>(result);

        return await Task.FromResult(bc);
    }
}