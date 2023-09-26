using System;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Exceptions.Wiretaps;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Wiretaps;
using MediatR;
using Serilog;

namespace erpl.services.V1.Wiretaps;

public class GetWiretapByIdProcessor : IGetWiretapByIdProcessor, IRequestHandler<GetWiretapByIdQuery, BusinessResult<WiretapDto>>
{
    private readonly IWiretapRepository _wiretapRepository;
    private readonly IAutoMapper _autoMapper;
    
    public GetWiretapByIdProcessor(IAutoMapper autoMapper, IWiretapRepository wiretapRepository)
    {
        this._autoMapper = autoMapper;
        this._wiretapRepository = wiretapRepository;
    }
    
    public async Task<BusinessResult<WiretapDto>> GetWiretapByIdAsync(string id)
    {
        var bc = new BusinessResult<WiretapDto>(new WiretapDto());
        
        if (string.IsNullOrEmpty(id))
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMMAND_Wiretap_ID"));
            return await Task.FromResult(bc);
        }

        try
        {
            var fetchedWiretap = await _wiretapRepository.FindOneWiretapById(id);
            
            if(!fetchedWiretap.IsNull())
                bc.Model = _autoMapper.Map<WiretapDto>(fetchedWiretap);
        }
        catch (WiretapDoesNotExistException e)
        {
            string errorMessage = "ERROR_FETCH_COMMAND_Wiretap_NOT_EXIST";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Wiretap: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetWiretapByIdAsync--  @fail@ [GetWiretapByIdProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }
        catch (MultipleWiretapWhereFoundException ex)
        {
            string errorMessage = "ERROR_FETCH_COMMAND_Wiretap_MULTIPLE";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Wiretap: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetWiretapByIdAsync--  @fail@ [GetWiretapByIdProcessor]. " +
                $"@innerfault:{ex.Message} and {ex.InnerException}");
        }
        catch (Exception exx)
        {
            string errorMessage = "OTHER_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Wiretap: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetWiretapByIdAsync--  @fail@ [GetWiretapByIdProcessor]. " +
                $"@innerfault:{exx.Message} and {exx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<WiretapDto>> Handle(GetWiretapByIdQuery qry, CancellationToken cancellationToken)
    {
        return await GetWiretapByIdAsync(qry.Id);
    }
}//Class : GetWiretapByIdProcessor