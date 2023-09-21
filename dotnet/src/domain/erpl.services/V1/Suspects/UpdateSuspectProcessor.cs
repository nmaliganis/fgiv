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

public class UpdateSuspectProcessor : IUpdateSuspectProcessor, IRequestHandler<UpdateSuspectCommand, BusinessResult<SuspectDto>>
{
    private readonly ISuspectRepository _suspectRepository;
    private readonly IAutoMapper _autoMapper;

    public UpdateSuspectProcessor(ISuspectRepository suspectRepository, IAutoMapper autoMapper)
    {
        this._suspectRepository = suspectRepository;
        this._autoMapper = autoMapper;
    }
    public async Task<BusinessResult<SuspectDto>> UpdateSuspectAsync(UpdateSuspectCommand cmd)
    {
        var bc = new BusinessResult<SuspectDto>(new SuspectDto());

        if (cmd.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL_SUSPECT"));
            return await Task.FromResult(bc);
        }

        try
        {
            var suspectToBeModified = await _suspectRepository.FindOneSuspectById(cmd.SuspectIdToBeModified);

            if (suspectToBeModified.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(cmd.SuspectIdToBeModified), "ERROR_UPDATE_COMMAND_SUSPECT_NOT_EXIST"));
                return bc;
            }
            
            suspectToBeModified.InjectWithValues(
                cmd.SuspectForModificationParameters.Gender, 
                cmd.SuspectForModificationParameters.Firstname, 
                cmd.SuspectForModificationParameters.Lastname, 
                cmd.SuspectForModificationParameters.Dob, 
                cmd.SuspectForModificationParameters.Calls, 
                cmd.SuspectForModificationParameters.Title,
                cmd.SuspectForModificationParameters.Street,
                cmd.SuspectForModificationParameters.City,
                cmd.SuspectForModificationParameters.Postcode,
                cmd.SuspectForModificationParameters.Country
                );
            
            this.ThrowExcIfSuspectCannotBeCreated(suspectToBeModified);
            await this.ThrowExcIfThisSuspectAlreadyExist(suspectToBeModified);
            
            Log.Information(
                $"Update Suspect: {cmd.SuspectForModificationParameters.Firstname} {cmd.SuspectForModificationParameters.Lastname}" +
                "--UpdateSuspectAsync--  @NotComplete@ [UpdateSuspectProcessor]. " +
                "Message: Just Before MakeItPersistence");

            MakeSuspectPersistent(suspectToBeModified);

            Log.Information(
                $"Update Suspect: {cmd.SuspectForModificationParameters.Firstname} {cmd.SuspectForModificationParameters.Lastname}" +
                "--UpdateSuspectAsync--  @NotComplete@ [UpdateSuspectProcessor]. " +
                "Message: Just After MakeItPersistence");

            bc.Model = this._autoMapper.Map<SuspectDto>(suspectToBeModified);
        }       
        catch (Exception exxx)
        {
            string errorMessage = "ERROR_CREATE_COMMAND_SUSPECT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update Suspect: {cmd.SuspectForModificationParameters.Firstname} {cmd.SuspectForModificationParameters.Lastname}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateSuspectAsync--  @fail@ [UpdateSuspectProcessor]. " +
                $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<SuspectDto>> Handle(UpdateSuspectCommand cmd, CancellationToken cancellationToken)
    {
        return await UpdateSuspectAsync(cmd);
    }
    
    private void MakeSuspectPersistent(Suspect suspectToBeModified)
    {
        _suspectRepository.UpdateSuspect(suspectToBeModified.Id, suspectToBeModified);
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
}