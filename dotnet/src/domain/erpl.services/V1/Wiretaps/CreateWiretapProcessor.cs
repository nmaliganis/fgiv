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

public class CreateWiretapProcessor : ICreateWiretapProcessor, IRequestHandler<CreateWiretapCommand, BusinessResult<WiretapDto>>
{
    private readonly IWiretapRepository _wiretapRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateWiretapProcessor(IWiretapRepository wiretapRepository, IAutoMapper autoMapper)
    {
        this._wiretapRepository = wiretapRepository;
        this._autoMapper = autoMapper;
    }

    public async Task<BusinessResult<WiretapDto>> CreateWiretapAsync(CreateWiretapCommand cmd)
    {
        var bc = new BusinessResult<WiretapDto>(new WiretapDto());

        if (cmd.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL_Wiretap"));
            return await Task.FromResult(bc);
        }

        try
        {
            var wiretapToBeCreated = new Wiretap();

            wiretapToBeCreated.InjectWithCreationValues(
                cmd.WiretapForCreationParameters.DateRecorded, 
                cmd.WiretapForCreationParameters.OfficerName, 
                cmd.WiretapForCreationParameters.SuspectNames, 
                cmd.WiretapForCreationParameters.Duration, 
                cmd.WiretapForCreationParameters.Filename,
                cmd.WiretapForCreationParameters.Filesize,
                cmd.WiretapForCreationParameters.File,
                cmd.WiretapForCreationParameters.Status
                );
            
            this.ThrowExcIfWiretapCannotBeCreated(wiretapToBeCreated);
            await this.ThrowExcIfThisWiretapAlreadyExist(wiretapToBeCreated);
            
            Log.Information(
                $"Create Wiretap: {cmd.WiretapForCreationParameters.Filename}" +
                "--CreateWiretapAsync--  @NotComplete@ [CreateWiretapProcessor]. " +
                "Message: Just Before MakeItPersistence");

            MakeWiretapPersistent(wiretapToBeCreated);

            Log.Information(
                $"Create Wiretap: {cmd.WiretapForCreationParameters.Filename}" +
                "--CreateWiretapAsync--  @NotComplete@ [CreateWiretapProcessor]. " +
                "Message: Just After MakeItPersistence");

            bc.Model = this._autoMapper.Map<WiretapDto>(wiretapToBeCreated);
        }       
        catch (Exception exxx)
        {
            string errorMessage = "ERROR_CREATE_COMMAND_Wiretap";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Create Wiretap: {cmd.WiretapForCreationParameters.Filename}" +
                $"Error Message:{errorMessage}" +
                $"--CreateWiretapAsync--  @fail@ [CreateWiretapProcessor]. " +
                $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void MakeWiretapPersistent(Wiretap wiretapToBeCreated)
    {
        _wiretapRepository.CreateWiretap(wiretapToBeCreated);
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

    public async Task<BusinessResult<WiretapDto>> Handle(CreateWiretapCommand cmd, CancellationToken cancellationToken)
    {
        return await CreateWiretapAsync(cmd);
    }
}//Class : CreateWiretapProcessor