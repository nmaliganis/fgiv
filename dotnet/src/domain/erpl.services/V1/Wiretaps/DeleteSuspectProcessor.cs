using System;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Wiretaps;
using MediatR;
using Serilog;

namespace erpl.services.V1.Wiretaps;

public class DeleteWiretapProcessor : IDeleteWiretapProcessor, IRequestHandler<DeleteWiretapCommand,
    BusinessResult<WiretapDto>>
{
    private readonly IWiretapRepository _wiretapRepository;
    private readonly IAutoMapper _autoMapper;

    public DeleteWiretapProcessor(IWiretapRepository wiretapRepository, IAutoMapper autoMapper)
    {
        this._wiretapRepository = wiretapRepository;
        this._autoMapper = autoMapper;
    }
    
    public async Task<BusinessResult<WiretapDto>> DeleteWiretapAsync(DeleteWiretapCommand cmd)
    {
        var bc = new BusinessResult<WiretapDto>(new WiretapDto());

        if (cmd.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_DELETE_COMMAND_MODEL_WIRETAP"));
            return await Task.FromResult(bc);
        }

        try
        {
            await _wiretapRepository.DeleteWiretap(cmd.WiretapIdToBeDeleted);
        }
        catch (Exception e)
        {
            string errorMessage = "ERROR_CREATE_COMMAND_Wiretap";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Delete Wiretap: {cmd.WiretapIdToBeDeleted}" +
                $"Error Message:{errorMessage}" +
                $"--DeleteWiretapAsync--  @fail@ [DeleteWiretapProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }
        
        return await Task.FromResult(bc);
    }

    public Task<BusinessResult<WiretapDto>> Handle(DeleteWiretapCommand cmd, CancellationToken cancellationToken)
    {
        return this.DeleteWiretapAsync(cmd);
    }
} //Class : DeleteHardWiretapProcessor