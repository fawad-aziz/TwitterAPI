using TwitterAPI.Helper;
using TwitterAPI.Models.Projections;
using TwitterAPI.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace TwitterAPI.Controllers
{
    [RoutePrefix("api/tweets")]
    public class TweetController : ApiController
    {
        private readonly ITweetService _tweetService;

        public TweetController(ITweetService tweetService)
        {
            Guard.ArgumentNotNull(tweetService, nameof(tweetService));

            _tweetService = tweetService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetTweets()
        {
            var tweets = await _tweetService.GetTweets();
            if (tweets == null)
            {
                return InternalServerError(new Exception("Something went wrong, please try again."));
            }

            return Ok(tweets);
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> UpdateTweets(List<TweetProjection> tweets)
        {
            var updateTweets = await _tweetService.UpdateTweets(tweets);
            return Ok(tweets);
        }
    }
}
