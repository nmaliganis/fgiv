using System;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Suspects;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Exceptions.Suspects;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Suspects;
using MediatR;
using Serilog;

namespace erpl.services.V1.Suspects;

public class GetSuspectByIdProcessor : IGetSuspectByIdProcessor, IRequestHandler<GetSuspectByIdQuery, BusinessResult<SuspectDto>>
{
    private readonly ISuspectRepository _suspectRepository;
    private readonly IAutoMapper _autoMapper;
    
    public GetSuspectByIdProcessor(IAutoMapper autoMapper, ISuspectRepository suspectRepository)
    {
        this._autoMapper = autoMapper;
        this._suspectRepository = suspectRepository;
    }
    
    public async Task<BusinessResult<SuspectDto>> GetSuspectByIdAsync(string id)
    {
        var bc = new BusinessResult<SuspectDto>(new SuspectDto());
        
        if (string.IsNullOrEmpty(id))
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMMAND_SUSPECT_ID"));
            return await Task.FromResult(bc);
        }

        try
        {
            var fetchedSuspect = await _suspectRepository.FindOneSuspectById(id);
            
            if(!fetchedSuspect.IsNull())
                bc.Model = _autoMapper.Map<SuspectDto>(fetchedSuspect);
        }
        catch (SuspectDoesNotExistException e)
        {
            string errorMessage = "ERROR_FETCH_COMMAND_SUSPECT_NOT_EXIST";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Suspect: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetSuspectByIdAsync--  @fail@ [GetSuspectByIdProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }
        catch (MultipleSuspectWhereFoundException ex)
        {
            string errorMessage = "ERROR_FETCH_COMMAND_SUSPECT_MULTIPLE";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Suspect: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetSuspectByIdAsync--  @fail@ [GetSuspectByIdProcessor]. " +
                $"@innerfault:{ex.Message} and {ex.InnerException}");
        }
        catch (Exception exx)
        {
            string errorMessage = "OTHER_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Fetch Suspect: {id}" +
                $"Error Message:{errorMessage}" +
                $"--GetSuspectByIdAsync--  @fail@ [GetSuspectByIdProcessor]. " +
                $"@innerfault:{exx.Message} and {exx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<SuspectDto>> Handle(GetSuspectByIdQuery qry, CancellationToken cancellationToken)
    {
        return await GetSuspectByIdAsync(qry.Id);
    }
}//Class : GetSuspectByIdProcessor