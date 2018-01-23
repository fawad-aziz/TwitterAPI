using TwitterAPI.Data.Contracts;
using TwitterAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwitterAPI.Helper;

namespace TwitterAPI.Data.Concrete
{
    public class SqliteTweetDataService : ITweetDataService
    {
        private readonly IDataContext _context;

        public SqliteTweetDataService(IDataContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            _context = context;
        }

        public async Task<List<Tweet>> GetCurrentTweets()
        {
            try
            {
                var tweets = await _context.Set<Tweet>().Where(t => t.IsCurrent == true).ToListAsync();
                return tweets;
            }
            catch (Exception ex)
            {
                Trace.Write(ex, nameof(GetCurrentTweets));
                throw;
            }
        }

        public async Task<List<Tweet>> UpdateTweets(List<Tweet> tweets)
        {
            using (var dbTran = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var tweet in _context.Set<Tweet>())
                    {
                        tweet.IsCurrent = false;
                    }

                    foreach (var tweet in tweets)
                    {
                        tweet.IsCurrent = true;
                        _context.Set<Tweet>().Add(tweet);
                    }

                    await _context.SaveChangesAsync();
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    Trace.Write(ex, nameof(GetCurrentTweets));
                    throw;
                }
            }

            using (var dbTran = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var tweet in _context.Set<Tweet>().Where(t => t.IsCurrent == false))
                    {
                        _context.Set<Tweet>().Remove(tweet);
                    }

                    await _context.SaveChangesAsync();
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    Trace.Write(ex, nameof(GetCurrentTweets));
                    throw;
                }
            }

            return tweets;
        }
    }
}