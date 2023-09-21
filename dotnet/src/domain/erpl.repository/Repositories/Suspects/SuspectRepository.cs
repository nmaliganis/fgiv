using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using erpl.common.infrastructure.Exceptions.Suspects;
using erpl.common.infrastructure.Paging;
using erpl.common.infrastructure.Queries;
using erpl.contracts.ContractRepositories;
using erpl.model.Suspects;
using JsonFlatFileDataStore;
using Serilog;

namespace erpl.repository.Repositories.Suspects;

public class SuspectRepository : ISuspectRepository
{
    public DataStore Store { get; private set; }
    
    private readonly string _folderDetails = string.Empty;
    
    public SuspectRepository()
    {
        this._folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\suspects.json");  
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
    
    public Task<Suspect> FindOneSuspectById(string idSuspect)
    {
        var collection = Store.GetCollection<Suspect>("suspects");

        var fetchedSuspects = collection
            .AsQueryable()
            .Where(p => p.Id == idSuspect).ToList();
        
        if (fetchedSuspects.Count > 1)
            throw new MultipleSuspectWhereFoundException($"More than one with same Id:{idSuspect}");
        if (fetchedSuspects.Count < 0)
            throw new SuspectDoesNotExistException($"More than one with same Id:{idSuspect}");
        
        return Task.Run(() => fetchedSuspects.FirstOrDefault());
    }

    public Task<Suspect> FindOneSuspectByFirstnameAndLastname(string firstname, string lastname)
    {
        var collection = Store.GetCollection<Suspect>("suspects");

        var fetchedSuspects = collection
            .AsQueryable()
            .Where(p => p.Firstname == firstname)
            .Where(p=> p.Lastname == lastname)
            .ToList();
        
        if (fetchedSuspects.Count > 1)
            throw new MultipleSuspectWhereFoundException($"More than one with same Name:{firstname} {lastname}");
        
        return Task.Run(() => fetchedSuspects.FirstOrDefault());
    }

    public async Task CreateSuspect(Suspect newSuspect)
    {
        var collection = Store.GetCollection<Suspect>("suspects");
        try
        {
            await collection.InsertOneAsync(newSuspect);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Insertion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task UpdateSuspect(string idSuspect, Suspect modifiedSuspect)
    {
        var collection = Store.GetCollection<Suspect>("suspects");
        try
        {
            await collection.UpdateOneAsync(s=>s.Id == idSuspect, modifiedSuspect);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Insertion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task DeleteSuspect(string idSuspect)
    {
        var collection = Store.GetCollection<Suspect>("suspects");
        try
        {
            await collection.DeleteOneAsync(s=>s.Id == idSuspect);
        }
        catch (Exception e)
        {
            Log.Error(
                $"Deletion Error:{e.Message} and {e.InnerException}");
            throw;
        }
    }

    public async Task<QueryResult<Suspect>> FindSuspectsPagedOf(int? pageNum, int? pageSize)
    {
        var collection = Store.GetCollection<Suspect>("suspects");
        
        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Suspect>(collection.AsQueryable(),
                    collection.Count,
                    (int) pageSize)
                ;
        }

        return new QueryResult<Suspect>(collection.AsQueryable()
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                    .Take((int) pageSize).AsQueryable(),
                collection.Count,
                (int) pageSize)
            ;
    }

    public Task<int> FindSuspectsCount()
    {
        throw new NotImplementedException();
    }
}