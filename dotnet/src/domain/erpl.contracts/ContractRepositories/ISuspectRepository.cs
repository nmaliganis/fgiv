using System;
using System.Threading.Tasks;
using erpl.common.infrastructure.Queries;
using erpl.model.Suspects;

namespace erpl.contracts.ContractRepositories;

public interface ISuspectRepository
{
    Task<Suspect> FindOneSuspectById(string idSuspect);
    Task<Suspect> FindOneSuspectByFirstnameAndLastname(string firstname, string lastname);

    Task CreateSuspect(Suspect newSuspect);
    Task UpdateSuspect(string idSuspect, Suspect modifiedSuspect);
    Task DeleteSuspect(string idSuspect);

    Task<QueryResult<Suspect>> FindSuspectsPagedOf(int? pageNum, int? pageSize);
    Task<int> FindSuspectsCount();
}