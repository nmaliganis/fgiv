using System;
using System.Linq;
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
using erpl.model.Suspects;
using MediatR;
using Serilog;

namespace erpl.services.V1.Suspects;

public class CreateSuspectProcessor : ICreateSuspectProcessor, IRequestHandler<CreateSuspectCommand, BusinessResult<SuspectDto>>
{
    private readonly ISuspectRepository _suspectRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateSuspectProcessor(ISuspectRepository suspectRepository, IAutoMapper autoMapper)
    {
        this._suspectRepository = suspectRepository;
        this._autoMapper = autoMapper;
    }

    public async Task<BusinessResult<SuspectDto>> CreateSuspectAsync(CreateSuspectCommand cmd)
    {
        var bc = new BusinessResult<SuspectDto>(new SuspectDto());

        if (cmd.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL_SUSPECT"));
            return await Task.FromResult(bc);
        }

        try
        {
            var suspectToBeCreated = new Suspect();

            suspectToBeCreated.InjectWithValues(
                cmd.SuspectForCreationParameters.Gender, 
                cmd.SuspectForCreationParameters.Firstname, 
                cmd.SuspectForCreationParameters.Lastname, 
                cmd.SuspectForCreationParameters.Dob, 
                cmd.SuspectForCreationParameters.Calls, 
                cmd.SuspectForCreationParameters.Title,
                cmd.SuspectForCreationParameters.Street,
                cmd.SuspectForCreationParameters.City,
                cmd.SuspectForCreationParameters.Postcode,
                cmd.SuspectForCreationParameters.Country
                );
            
            this.ThrowExcIfSuspectCannotBeCreated(suspectToBeCreated);
            await this.ThrowExcIfThisSuspectAlreadyExist(suspectToBeCreated);
            
            Log.Information(
                $"Create Suspect: {cmd.SuspectForCreationParameters.Firstname} {cmd.SuspectForCreationParameters.Lastname}" +
                "--CreateSuspectAsync--  @NotComplete@ [CreateSuspectProcessor]. " +
                "Message: Just Before MakeItPersistence");

            MakeSuspectPersistent(suspectToBeCreated);

            Log.Information(
                $"Create Suspect: {cmd.SuspectForCreationParameters.Firstname} {cmd.SuspectForCreationParameters.Lastname}" +
                "--CreateSuspectAsync--  @NotComplete@ [CreateSuspectProcessor]. " +
                "Message: Just After MakeItPersistence");

            bc.Model = this._autoMapper.Map<SuspectDto>(suspectToBeCreated);
        }       
        catch (Exception exxx)
        {
            string errorMessage = "ERROR_CREATE_COMMAND_SUSPECT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Create Suspect: {cmd.SuspectForCreationParameters.Firstname} {cmd.SuspectForCreationParameters.Lastname}" +
                $"Error Message:{errorMessage}" +
                $"--CreateSuspectAsync--  @fail@ [CreateSuspectProcessor]. " +
                $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void MakeSuspectPersistent(Suspect suspectToBeCreated)
    {
        _suspectRepository.CreateSuspect(suspectToBeCreated);
    }
    
    private async Task ThrowExcIfThisSuspectAlreadyExist(Suspect suspectToBeCreated)
    {
        var suspectRetrieved = await this._suspectRepository.FindOneSuspectByFirstnameAndLastname(suspectToBeCreated.Firstname, suspectToBeCreated.Lastname);
        if (!suspectRetrieved.IsNull())
        {
            throw new SuspectAlreadyExistsException($"{suspectToBeCreated.Firstname} {suspectToBeCreated.Lastname}",
                suspectToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void ThrowExcIfSuspectCannotBeCreated(Suspect suspectToBeCreated)
    {
        bool canBeCreated = !suspectToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidSuspectException(suspectToBeCreated.GetBrokenRulesAsString());
        }
    }

    public async Task<BusinessResult<SuspectDto>> Handle(CreateSuspectCommand cmd, CancellationToken cancellationToken)
    {
        return await CreateSuspectAsync(cmd);
    }
}//Class : CreateSuspectProcessor