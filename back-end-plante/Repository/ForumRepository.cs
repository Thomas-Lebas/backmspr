using back_end_plante.Common.Models;
using back_end_plante.Configurations;
using back_end_plante.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace back_end_plante.Repository;

public class ForumRepository : MsprPlanteRepositoryBase, IForumRepository
{
    private readonly IMongoCollection<Forum> _forumCollection;
    
    public ForumRepository(IOptions<ConnectionStringConfiguration> connectionStringConfiguration) : base(connectionStringConfiguration)
    {
        try
        {
            var db = GetClient().WithReadPreference(ReadPreference.Secondary).GetDatabase(MongoPocProviderName);
            _forumCollection = db.GetCollection<Forum>("Forum");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Forum>> GetForums()
    {
        var response = await _forumCollection.FindAsync(Builders<Forum>.Filter.Empty);
        return response.ToList();
    }

    public async Task<Forum> GetForumById(string forumId)
    {
        var filter = Builders<Forum>.Filter.Eq(f => f.Id, forumId);

        var reponse = await _forumCollection.FindAsync(filter);

        return reponse.ToList().FirstOrDefault();
    }

    public async Task CreateForum(Forum forum)
    {
        await _forumCollection.InsertOneAsync(forum);
    }

    public async Task DeleteForum(string forumId)
    {
        var filter = Builders<Forum>.Filter.Eq(f => f.Id, forumId);

        await _forumCollection.DeleteOneAsync(filter);
    }

    public async Task UpdateForum(string forumId, Forum forum)
    {
        var builder = Builders<Forum>.Filter;

        var filter = builder.Eq(f => f.Id, forumId);

        var result = await _forumCollection.ReplaceOneAsync(filter, forum);

        if (result.ModifiedCount == 0)
        {
            throw new BadHttpRequestException($"Failed to update forum with id {forumId}");
        }
    }
}