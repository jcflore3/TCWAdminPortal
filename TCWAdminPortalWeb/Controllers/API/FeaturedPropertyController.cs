using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;
using System.Linq;

namespace TCWAdminPortalWeb.Controllers.Api
{
    [EnableCors(origins: "http://localhost:8080,http://texascrosswayrealty.com", headers: "*", methods:"*")]
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

            // return only the Featured Properties that are flagged as enabled
            return results.Where(x => x.Enabled);
        }
    }
}