using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using erpl.common.dtos.Cqrs.Wiretaps;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;
using erpl.common.infrastructure.Exceptions.Wiretaps;
using erpl.common.infrastructure.Exceptions.Wiretaps;
using erpl.common.infrastructure.Extensions;
using erpl.common.infrastructure.Types;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Wiretaps;
using erpl.model.Wiretaps;
using MediatR;
using Serilog;

namespace erpl.services.V1.Wiretaps;

public class UpdateWiretapProcessor : IUpdateWiretapProcessor, IRequestHandler<UpdateWiretapCommand, BusinessResult<WiretapDto>>
{
    private readonly IWiretapRepository _wiretapRepository;
    private readonly IAutoMapper _autoMapper;

    public UpdateWiretapProcessor(IWiretapRepository wiretapRepository, IAutoMapper autoMapper)
    {
        this._wiretapRepository = wiretapRepository;
        this._autoMapper = autoMapper;
    }
    public async Task<BusinessResult<WiretapDto>> UpdateWiretapAsync(UpdateWiretapCommand cmd)
    {
        var bc = new BusinessResult<WiretapDto>(new WiretapDto());

        if (cmd.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL_Wiretap"));
            return await Task.FromResult(bc);
        }

        try
        {
            var wiretapToBeModified = await _wiretapRepository.FindOneWiretapById(cmd.WiretapIdToBeModified);

            if (wiretapToBeModified.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(cmd.WiretapIdToBeModified), "ERROR_UPDATE_COMMAND_Wiretap_NOT_EXIST"));
                return bc;
            }
            
            wiretapToBeModified.InjectWithModificationValues(
                cmd.WiretapForModificationParameters.DateRecorded, 
                cmd.WiretapForModificationParameters.OfficerName, 
                cmd.WiretapForModificationParameters.SuspectNames, 
                cmd.WiretapForModificationParameters.Duration, 
                cmd.WiretapForModificationParameters.Transcription, 
                cmd.WiretapForModificationParameters.Filename,
                cmd.WiretapForModificationParameters.Filesize,
                cmd.WiretapForModificationParameters.File,
                cmd.WiretapForModificationParameters.Status
                );
            
            this.ThrowExcIfWiretapCannotBeCreated(wiretapToBeModified);
            await this.ThrowExcIfThisWiretapAlreadyExist(wiretapToBeModified);
            
            Log.Information(
                $"Create Wiretap: {cmd.WiretapForModificationParameters.Filename}" +
                "--UpdateWiretapAsync--  @NotComplete@ [UpdateWiretapProcessor]. " +
                "Message: Just Before MakeItPersistence");

            MakeWiretapPersistent(wiretapToBeModified);

            Log.Information(
                $"Create Wiretap: {cmd.WiretapForModificationParameters.Filename}" +
                "--UpdateWiretapAsync--  @NotComplete@ [UpdateWiretapProcessor]. " +
                "Message: Just After MakeItPersistence");

            bc.Model = this._autoMapper.Map<WiretapDto>(wiretapToBeModified);
        }       
        catch (Exception exxx)
        {
            string errorMessage = "ERROR_CREATE_COMMAND_Wiretap";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Create Wiretap: {cmd.WiretapForModificationParameters.Filename}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateWiretapAsync--  @fail@ [UpdateWiretapProcessor]. " +
                $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<WiretapDto>> Handle(UpdateWiretapCommand cmd, CancellationToken cancellationToken)
    {
        return await UpdateWiretapAsync(cmd);
    }
    
    private void MakeWiretapPersistent(Wiretap wiretapToBeModified)
    {
        _wiretapRepository.UpdateWiretap(wiretapToBeModified.Id, wiretapToBeModified);
    }
    
    private async Task ThrowExcIfThisWiretapAlreadyExist(Wiretap wiretapToBeCreated)
    {
        var wiretapRetrieved = await this._wiretapRepository.FindOneWiretapByFilename(wiretapToBeCreated.Filename);
        if (!wiretapRetrieved.IsNull())
        {
            throw new WiretapAlreadyExistsException($"{wiretapToBeCreated.Filename}",
                wiretapToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void ThrowExcIfWiretapCannotBeCreated(Wiretap wiretapToBeCreated)
    {
        bool canBeCreated = !wiretapToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidWiretapException(wiretapToBeCreated.GetBrokenRulesAsString());
        }
    }
}