using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using erpl.api.Controllers.API.Base;
using erpl.api.Validators;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.dtos.ResourceParameters.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.TypeHelpers;
using erpl.model.Wiretaps;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace erpl.api.Controllers.API.V1;

/// <summary>
/// Wiretap Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
//[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class WiretapsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IHostingEnvironment _environment;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="environment"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    public WiretapsController(
        IMediator mediator,
        IUrlHelper urlHelper,
        IHostingEnvironment environment,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService)
    {
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
        
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    /// <summary>
    /// Get - Fetch all Wiretaps
    /// </summary>
    /// <param name="parameters">Wiretap parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Wiretaps </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchWiretapsRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(BusinessResult<PagedList<WiretapDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchWiretapsAsync(
        [FromQuery] GetWiretapsResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<WiretapDto, Wiretap>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<WiretapDto>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var fetchedItineraries = await this._mediator.Send(new GetWiretapsQuery(parameters));

        if (fetchedItineraries.IsNull())
        {
            return this.NotFound("ERROR_FETCH_WiretapS");
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
            parameters, this._urlHelper, "GetWiretapByIdAsync", "FetchWiretapsAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one Wiretap
    /// </summary>
    /// <param name="id">Wiretap Id for fetching</param>
    /// <remarks>Fetch one Wiretap </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetWiretapByIdAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(WiretapDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetWiretapByIdAsync(string id)
    {
        var fetchedWiretap = await this._mediator.Send(new GetWiretapByIdQuery(id));

        if (fetchedWiretap.IsNull())
        {
            return this.NotFound("ERROR_FETCH_Wiretap");
        }

        if (!fetchedWiretap.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedWiretap.BrokenRules);
        }

        if (fetchedWiretap.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedWiretap.BrokenRules);
        }

        return this.Ok(fetchedWiretap);
    }

    /// <summary>
    /// Post - Upload a new wiretap recording
    /// </summary>
    /// <param name="file">File Media for Creation supportedTypes "wav", "mp3"</param>
    /// <param name="wiretapForCreationParameters">CreateWiretapResourceParameters for creation</param>
    /// <remarks>Create Wiretap </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "CreateWiretapRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(WiretapDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateWiretapAsync( IFormFile file, [FromBody] CreateWiretapResourceParameters wiretapForCreationParameters)
    {
        var supportedTypes = new[] { "wav", "mp3" };
        var fileExt = System.IO.Path.GetExtension(file.FileName)?.Substring(1);
        if (!((IList)supportedTypes).Contains(fileExt))
        {
            string errorMessage = "File Extension is InValid";
            return this.BadRequest(new { errorMessage });
        }
        
        
        if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
        {
            _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");
        }

        string audioPath = DateTime.UtcNow.ToString(CultureInfo.CurrentCulture).Replace("/", "").Replace(" ", "").Replace(":", "");

        try
        {
            var folder = Path.Combine(_environment.WebRootPath, audioPath);

            try
            {
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            }
            catch (Exception e)
            {
                string errorMessage = "Audio Storage Path is InValid";
                return this.BadRequest(new { errorMessage });
            }

            if (file.Length > 0)
            {
                await using var fileStream = new FileStream(Path.Combine(folder, file.FileName), FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "Audio File Upload failed";
            return this.BadRequest(new { errorMessage });
        }
        
        
        var createdWiretap = await this._mediator.Send(new CreateWiretapCommand(wiretapForCreationParameters));
        
        if (!createdWiretap.IsSuccess())
        {
            return this.OkOrBadRequest(createdWiretap.BrokenRules);
        }

        if (createdWiretap.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostWiretapRouteAsync -- Message:Wiretap_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- WiretapInfo:{createdWiretap.Model.Id}");
            return this.OkOrNoResult(createdWiretap.BrokenRules);
        }
        return this.CreatedOrNoResult(createdWiretap);
    }

    /// <summary>
    /// Put - Update a Wiretap
    /// </summary>
    /// <param name="id">Wiretap Id for modification</param>
    /// <param name="request">UpdateWiretapResourceParameters for modification</param>
    /// <remarks>Update Wiretap </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateWiretapAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(WiretapDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateWiretapAsync(string id, [FromBody] UpdateWiretapResourceParameters request)
    {
        var response = await this._mediator.Send(new UpdateWiretapCommand(id, request));

        if (response.IsNull())
        {
            return this.NotFound("Wiretap_NOT_FOUND");
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
    /// Delete - Remove an existing Wiretap
    /// </summary>
    /// <param name="id">Wiretap Id for deletion</param>
    /// <remarks>Delete existing Wiretap </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("{id}", Name = "DeleteWiretapAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(WiretapDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteWiretapAsync(string id)
    {
        var response = await this._mediator.Send(new DeleteWiretapCommand(id));

        if (response.IsNull())
        {
            return this.NotFound("ERROR_DELETE_Wiretap");
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
}//Class : WiretapsController