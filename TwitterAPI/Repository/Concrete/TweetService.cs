using AutoMapper;
using TwitterAPI.Data.Contracts;
using TwitterAPI.Helper;
using TwitterAPI.Models.Entities;
using TwitterAPI.Models.Projections;
using TwitterAPI.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TwitterAPI.Repository.Concrete
{
    public class TweetService : ITweetService
    {
        private readonly IMapper _mapper;

        private readonly ITweetDataService _tweetDataService;

        public TweetService(IMapper mapper, ITweetDataService tweetDataService)
        {
            Guard.ArgumentNotNull(mapper, nameof(mapper));
            Guard.ArgumentNotNull(tweetDataService, nameof(tweetDataService));

            _mapper = mapper;
            _tweetDataService = tweetDataService;
        }

        public async Task<List<TweetProjection>> GetTweets()
        {
            try
            {
                var tweets = await _tweetDataService.GetCurrentTweets();
                return _mapper.Map<List<TweetProjection>>(tweets);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message, nameof(GetTweets));
                return await Task.FromResult(new List<TweetProjection>());
            }
        }

        public Task<List<TweetProjection>> UpdateTweets(List<TweetProjection> tweetProjections)
        {
            try
            {
                if (tweetProjections != null && tweetProjections.Count > 0)
                {
                    var tweets = _mapper.Map<List<Tweet>>(tweetProjections);
                    _tweetDataService.UpdateTweets(tweets);
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message, nameof(UpdateTweets));
            }

            return Task.FromResult(tweetProjections);
        }
    }
}