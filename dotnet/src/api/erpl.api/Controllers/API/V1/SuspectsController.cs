using System;
using System.Net;
using System.Threading.Tasks;
using erpl.api.Controllers.API.Base;
using erpl.api.Validators;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.dtos.ResourceParameters.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.TypeHelpers;
using erpl.model.Suspects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace erpl.api.Controllers.API.V1;

/// <summary>
/// Suspect Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SuspectsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;


    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    public SuspectsController(
        IMediator mediator,
        IUrlHelper urlHelper,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService)
    {
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
    }

    /// <summary>
    /// Get - Fetch all Suspects
    /// </summary>
    /// <param name="parameters">Suspect parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Suspects </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchSuspectsRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(BusinessResult<PagedList<SuspectDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchSuspectsAsync(
        [FromQuery] GetSuspectsResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<SuspectDto, Suspect>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<SuspectDto>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var fetchedItineraries = await this._mediator.Send(new GetSuspectsQuery(parameters));

        if (fetchedItineraries.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SUSPECTS");
        }

        if (!fetchedItineraries.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedItineraries.BrokenRules);
        }

        if (fetchedItineraries.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedItineraries.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedItineraries.Model, mediaType,
            parameters, this._urlHelper, "GetSuspectByIdAsync", "FetchSuspectsAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one Suspect
    /// </summary>
    /// <param name="id">Suspect Id for fetching</param>
    /// <remarks>Fetch one Suspect </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetSuspectByIdAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(SuspectDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetSuspectByIdAsync(string id)
    {
        var fetchedSuspect = await this._mediator.Send(new GetSuspectByIdQuery(id));

        if (fetchedSuspect.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SUSPECT");
        }

        if (!fetchedSuspect.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedSuspect.BrokenRules);
        }

        if (fetchedSuspect.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedSuspect.BrokenRules);
        }

        return this.Ok(fetchedSuspect);
    }

    /// <summary>
    /// Post - Create a Suspect
    /// </summary>
    /// <param name="suspectForCreationParameters">CreateSuspectResourceParameters for creation</param>
    /// <remarks>Create Suspect </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "CreateSuspectRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(SuspectDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateSuspectAsync([FromBody] CreateSuspectResourceParameters suspectForCreationParameters)
    {
        var createdSuspect = await this._mediator.Send(new CreateSuspectCommand(suspectForCreationParameters));
        
        if (!createdSuspect.IsSuccess())
        {
            return this.OkOrBadRequest(createdSuspect.BrokenRules);
        }

        if (createdSuspect.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostSuspectRouteAsync -- Message:SUSPECT_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- SuspectInfo:{createdSuspect.Model.Id}");
            return this.OkOrNoResult(createdSuspect.BrokenRules);
        }
        return this.CreatedOrNoResult(createdSuspect);
    }

    /// <summary>
    /// Put - Update a Suspect
    /// </summary>
    /// <param name="id">Suspect Id for modification</param>
    /// <param name="request">UpdateSuspectResourceParameters for modification</param>
    /// <remarks>Update Suspect </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateSuspectAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(SuspectDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateSuspectAsync(string id, [FromBody] UpdateSuspectResourceParameters request)
    {
        var response = await this._mediator.Send(new UpdateSuspectCommand(id, request));

        if (response.IsNull())
        {
            return this.NotFound("SUSPECT_NOT_FOUND");
        }

        if (!response.IsSuccess())
        {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }
    

    /// <summary>
    /// Delete - Delete an existing Suspect - Delete
    /// </summary>
    /// <param name="id">Suspect Id for deletion</param>
    /// <remarks>Delete existing Suspect </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("{id}", Name = "DeleteSuspectAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(SuspectDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteSuspectAsync(string id)
    {
        var response = await this._mediator.Send(new DeleteSuspectCommand(id));

        if (response.IsNull())
        {
            return this.NotFound("ERROR_DELETE_SUSPECT");
        }

        if (!response.IsSuccess())
        {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }
}//Class : SuspectsController