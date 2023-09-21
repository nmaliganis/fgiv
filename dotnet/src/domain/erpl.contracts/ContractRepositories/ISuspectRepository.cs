using System;
using System.Threading.Tasks;
using erpl.common.infrastructure.Queries;
using erpl.model.Suspects;

namespace erpl.contracts.ContractRepositories;

public interface ISuspectRepository
{
    Task<Suspect> FindOneSuspectById(string idSuspect);

    Task CreateSuspect(Suspect newSuspect);
    Task UpdateSuspect(Suspect modifiedSuspect);
    Task DeleteSuspect(string idSuspect);

    Task<QueryResult<Suspect>> FindSuspectsPagedOf(int? pageNum, int? pageSize);
    Task<int> FindSuspectsCount();
}