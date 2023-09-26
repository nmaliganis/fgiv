using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using erpl.common.infrastructure.Exceptions.Wiretaps;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.Queries;
using erpl.contracts.ContractRepositories;
using erpl.model.Wiretaps;
using JsonFlatFileDataStore;
using Serilog;

namespace erpl.repository.Repositories.Wiretaps;

public class WiretapRepository : IWiretapRepository
{
    public DataStore Store { get; private set; }
    
    private readonly string _folderDetails = string.Empty;
    
    public WiretapRepository()
    {
        this._folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\Wiretaps.json");  
        try
        {
            Store = new DataStore(this._folderDetails);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Loading Json Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }
    
    public Task<Wiretap> FindOneWiretapById(string idWiretap)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");

        var fetchedWiretaps = collection
            .AsQueryable()
            .Where(p => p.Id == idWiretap).ToList();
        
        if (fetchedWiretaps.Count > 1)
            throw new MultipleWiretapWhereFoundException($"More than one with same Id:{idWiretap}");
        if (fetchedWiretaps.Count < 0)
            throw new WiretapDoesNotExistException($"More than one with same Id:{idWiretap}");
        
        return Task.Run(() => fetchedWiretaps.FirstOrDefault());
    }
    

    public async Task CreateWiretap(Wiretap newWiretap)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");
        try
        {
            await collection.InsertOneAsync(newWiretap);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Insertion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task UpdateWiretap(string idWiretap, Wiretap modifiedWiretap)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");
        try
        {
            await collection.UpdateOneAsync(s=>s.Id == idWiretap, modifiedWiretap);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Insertion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task DeleteWiretap(string idWiretap)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");
        try
        {
            await collection.DeleteOneAsync(s=>s.Id == idWiretap);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Deletion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task<QueryResult<Wiretap>> FindWiretapsPagedOf(int? pageNum, int? pageSize)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");
        
        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Wiretap>(collection.AsQueryable(),
                    collection.Count,
                    (int) pageSize)
                ;
        }

        return new QueryResult<Wiretap>(collection.AsQueryable()
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                    .Take((int) pageSize).AsQueryable(),
                collection.Count,
                (int) pageSize)
            ;
    }

    public Task<Wiretap> FindOneWiretapByFilename(string filename)
    {
        var collection = Store.GetCollection<Wiretap>("Wiretaps");

        var fetchedWiretaps = collection
            .AsQueryable()
            .Where(p => p.Filename == filename).ToList();
        
        if (fetchedWiretaps.Count > 1)
            throw new MultipleWiretapWhereFoundException($"More than one with same Id:{filename}");
        if (fetchedWiretaps.Count < 0)
            throw new WiretapDoesNotExistException($"More than one with same Id:{filename}");
        
        return Task.Run(() => fetchedWiretaps.FirstOrDefault());
    }
}