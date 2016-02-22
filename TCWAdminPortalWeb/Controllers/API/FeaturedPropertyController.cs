using System.Collections.Generic;
using System.Web.Http;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Api
{
    public class FeaturedPropertyController : ApiController
    {
        private TCWAdminRepository<FeaturedProperty> _repository;

        public FeaturedPropertyController()
        {
            _repository = new TCWAdminRepository<FeaturedProperty>();
        }

        /// <summary>
        /// REST Get of all Featured Properties
        /// </summary>
        /// <returns>An IEnumerable of all the featured properties.</returns>
        [HttpGet()]
        public IEnumerable<FeaturedPropertyViewModel> Get()
        {
            var results = AutoMapperConfig.TCWMapper.Map<IEnumerable<FeaturedPropertyViewModel>>(_repository.GetAll());
            return results;
        }
    }
}