using AutoMapper;
using StructureMap;
using StructureMap.Graph;
using System;
using TwitterAPI.Data;
using TwitterAPI.Data.Concrete;
using TwitterAPI.Data.Contracts;
using TwitterAPI.Models.Entities;
using TwitterAPI.Models.Projections;
using TwitterAPI.Repository.Concrete;
using TwitterAPI.Repository.Contracts;

namespace TwitterAPI.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tweet, TweetProjection>();
                cfg.CreateMap<TweetProjection, Tweet>();
            });

            var mapper = mapperConfig.CreateMapper();

            For<IMapper>().Use(mapper);
            For<IDataContext>().Use<FullDbContext>();
            For<ITweetDataService>().Use<SqliteTweetDataService>();
            For<ITweetService>().Use<TweetService>().Ctor<IMapper>().Is(mapper);
            //For<IDisposable>().Add<FullDbContext>();
        }
    }
}