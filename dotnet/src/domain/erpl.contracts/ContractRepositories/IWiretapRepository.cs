using System;
using System.Threading.Tasks;
using erpl.common.infrastructure.Queries;
using erpl.model.Wiretaps;

namespace erpl.contracts.ContractRepositories;

public interface IWiretapRepository
{
    Task<Wiretap> FindOneWiretapById(string idWiretap);

    Task CreateWiretap(Wiretap newWiretap);
    Task UpdateWiretap(string idWiretap, Wiretap modifiedWiretap);
    Task DeleteWiretap(string idWiretap);

    Task<QueryResult<Wiretap>> FindWiretapsPagedOf(int? pageNum, int? pageSize);
    Task<Wiretap> FindOneWiretapByFilename(string filename);
}