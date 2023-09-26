using System;
using System.Threading.Tasks;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.BrokenRules;

namespace erpl.contracts.V1.Wiretaps;

public interface IGetWiretapByIdProcessor
{
    Task<BusinessResult<WiretapDto>> GetWiretapByIdAsync(string id);
}