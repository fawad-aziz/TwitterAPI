using TwitterAPI.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterAPI.Data.Contracts
{
    public interface ITweetDataService
    {
        Task<List<Tweet>> GetCurrentTweets();

        Task<List<Tweet>> UpdateTweets(List<Tweet> tweets);
    }
}
