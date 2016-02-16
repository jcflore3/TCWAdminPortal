using AutoMapper;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.ViewModels;
using System.Web.Mvc;

namespace TCWAdminPortalWeb
{
    public static class AutoMapperConfig
    {
        public static IMapper TCWMapper { get; private set; }

        public static void InitializeAutoMapper()
        {
            // Configure the mappings
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<FeaturedProperty, FeaturedPropertyViewModel>().ReverseMap();
            });

            // set static instance of the mapper
            TCWMapper = mapperConfig.CreateMapper();
        }
    }
}