using TwitterAPI.Data.Contracts;
using TwitterAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterAPI.Data.Concrete
{
    public class SqliteTweetDataService : ITweetDataService
    {
        public SqliteTweetDataService() { }

        public async Task<List<Tweet>> GetCurrentTweets()
        {
            try
            {
                using (var db = new FullDbContext())
                {
                    var tweets = await db.Tweets.Where(t => t.IsCurrent == true).ToListAsync();
                    return tweets;
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex, nameof(GetCurrentTweets));
                throw;
            }
        }

        public async Task<List<Tweet>> UpdateTweets(List<Tweet> tweets)
        {
            using (var db = new FullDbContext())
            {
                using (var dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var tweet in db.Tweets)
                        {
                            tweet.IsCurrent = false;
                        }

                        foreach (var tweet in tweets)
                        {
                            tweet.IsCurrent = true;
                            db.Tweets.Add(tweet);
                        }

                        await db.SaveChangesAsync();
                        dbTran.Commit();

                        tweets = await db.Tweets.ToListAsync();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        Trace.Write(ex, nameof(GetCurrentTweets));
                        throw;
                    }
                }

                using (var dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var tweet in db.Tweets.Where(t => t.IsCurrent == false))
                        {
                            db.Tweets.Remove(tweet);
                        }

                        await db.SaveChangesAsync();
                        dbTran.Commit();

                        tweets = await db.Tweets.ToListAsync();
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
}