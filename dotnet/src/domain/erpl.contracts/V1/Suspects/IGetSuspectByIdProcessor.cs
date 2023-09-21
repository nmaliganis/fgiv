using System;
using System.Threading.Tasks;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.BrokenRules;

namespace erpl.contracts.V1.Suspects;

public interface IGetSuspectByIdProcessor
{
    Task<BusinessResult<SuspectDto>> GetSuspectByIdAsync(string id);
}